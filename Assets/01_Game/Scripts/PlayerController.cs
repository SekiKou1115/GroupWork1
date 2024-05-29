using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;


namespace UnityChan
{

    // 必要なコンポーネントの列記
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;

        public float animSpeed = 1.5f;              // アニメーション再生速度設定
        public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
        public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
                                                    // このスイッチが入っていないとカーブは使われない
        public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

        // ノックバック宣言
        private bool isMove = true;
        private Vector3 knockbackVelocity = Vector3.zero;

        // 以下キャラクターコントローラ用パラメタ
        [Header("パラメータ")]
        [SerializeField, Tooltip("移動速度")] private float _speed = 3;
        [SerializeField, Tooltip("回転速度")] private float _rotateSpeed = 1f;
        [SerializeField, Tooltip("ジャンプ力")] private float _jumpPower = 3.0f;
        [SerializeField, Tooltip("ノックバック力")] private float _knockbackPower = 1f;

        // アイテムによるスピードへの効果格納用 1f=等倍
        private float _itemSpeed = 1f;
        public float itemSpeed
        {
            get { return _itemSpeed; }
            set { _itemSpeed = value; }
        }
        // オブジェクトによるスピードへの効果格納用 1f=等倍
        private float _objectSpd = 1f;
        public float objectSpd
        {
            get { return _objectSpd; }
            set { _objectSpd = value; }
        }

        // プレイヤーのムテキ状態格納
        [Header("無敵"), SerializeField]
        private bool _invincible = false;
        public bool invincible
        {
            get { return _invincible; }
            set { _invincible = value; }
        }

        // ムテキ時のエフェクト
        [SerializeField]
        public ParticleSystem _invincibleEffect;

        [Header("カメラ"), SerializeField]
        private Camera _targetCamera;

        // Rayの長さ
        [SerializeField] private float _rayLength = 1f;

        // Rayをどれくらい身体にめり込ませるか
        [SerializeField] private float _rayOffset;

        // Rayの判定に用いるLayer
        [SerializeField] private LayerMask _layerMask = default;

        [SerializeField] bool _isGameOver = false;   //ゲームオーバー判定
        [SerializeField] GameObject _gameOverMenu;

        private bool _isGround = true;

        private Transform _transform;

        private Vector2 _inputMove;

        // キャラクターコントローラ（カプセルコライダ）の参照
        private CapsuleCollider col;
        private Rigidbody rb;
        // キャラクターコントローラ（カプセルコライダ）の移動量
        private Vector3 velocity;
        // CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
        private float orgColHight;
        private Vector3 orgVectColCenter;
        private Animator _animation;                        // キャラにアタッチされるアニメーターへの参照
        private AnimatorStateInfo currentBaseState;         // base layerで使われる、アニメーターの現在の状態の参照

        private GameObject cameraObject;    // メインカメラへの参照


        // アニメーター各ステートへの参照
        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int locoState = Animator.StringToHash("Base Layer.Locomotion");
        static int jumpState = Animator.StringToHash("Base Layer.Jump");
        static int restState = Animator.StringToHash("Base Layer.Rest");

        // ----------------------------------------------------------------------- InputSystem
        public void OnMove(InputAction.CallbackContext context)
        {
            _inputMove = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            //アニメーションのステートがLocomotionの最中のみジャンプできる
            if (currentBaseState.nameHash == locoState)
            {
                //ステート遷移中でなかったらジャンプできる
                if (!_animation.IsInTransition(0) && _isGround)
                {
                    rb.AddForce(transform.up * _jumpPower, ForceMode.VelocityChange);
                    _animation.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                }
            }
        }

        // ゲームオーバー処理
        public void GameOver()
        {
            _animation.SetBool("Reflesh", true);
            _isGameOver = true;
            _gameOverMenu.SetActive(true);
            Time.timeScale = 0;
        }

        // ノックバック処理
        public async void KnockBack()
        {
            // ノックバック中は処理しない
            if (!isMove)
            {
                return;
            }

            isMove = false;

            //ダメージアニメーションを再生
            if (_animation != null)
            {
                _animation.SetBool("Damage", true);
            }

            rb.AddForce(-transform.forward * _knockbackPower, ForceMode.VelocityChange);

            await UniTask.Delay(TimeSpan.FromSeconds(2.5f));

            _animation.SetBool("Damage", false);

            isMove = true;
        }

        // ------------------------------------------------------------------------ UnityMessage
        private void Awake()
        {
            Instance = this;

            _transform = transform;

            if (_targetCamera == null)
                _targetCamera = Camera.main;
        }

        // 初期化
        void Start()
        {
            // Animatorコンポーネントを取得する
            _animation = GetComponent<Animator>();
            // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            //メインカメラを取得する
            cameraObject = GameObject.FindWithTag("MainCamera");
            // CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
            orgColHight = col.height;
            orgVectColCenter = col.center;
            // ParticleSystemコンポーネント取得
            _invincibleEffect = GetComponentInChildren<ParticleSystem>();
        }

        // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
        void FixedUpdate()
        {
            _animation.speed = animSpeed;                             // Animatorのモーション再生速度に animSpeedを設定する
            currentBaseState = _animation.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
            rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする

            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * _inputMove.y + Camera.main.transform.right * _inputMove.x;

            // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            if (isMove && !_isGameOver)
            {
                rb.velocity = moveForward * (_speed * itemSpeed * _objectSpd) + new Vector3(0, rb.velocity.y, 0);
            }

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveForward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
            }

            _isGround = CheckGrounded();
            Debug.Log(_isGround);

            _animation.SetFloat("Speed", rb.velocity.magnitude);

            // 以下、Animatorの各ステート中での処理
            // Locomotion中
            // 現在のベースレイヤーがlocoStateの時
            if (currentBaseState.nameHash == locoState)
            {
                //カーブでコライダ調整をしている時は、念のためにリセットする
                if (useCurves)
                {
                    resetCollider();
                }
            }
            // JUMP中の処理
            // 現在のベースレイヤーがjumpStateの時
            else if (currentBaseState.nameHash == jumpState)
            {
                /*cameraObject.SendMessage("setCameraPositionJumpView");  */// ジャンプ中のカメラに変更
                                                                            // ステートがトランジション中でない場合
                if (!_animation.IsInTransition(0))
                {

                    // 以下、カーブ調整をする場合の処理
                    if (useCurves)
                    {
                        // 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
                        // JumpHeight:JUMP00でのジャンプの高さ（0〜1）
                        // GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
                        float jumpHeight = _animation.GetFloat("JumpHeight");
                        float gravityControl = _animation.GetFloat("GravityControl");
                        if (gravityControl > 0)
                            rb.useGravity = false;  //ジャンプ中の重力の影響を切る

                        // レイキャストをキャラクターのセンターから落とす
                        Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                        RaycastHit hitInfo = new RaycastHit();
                        // 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            if (hitInfo.distance > useCurvesHeight)
                            {
                                col.height = orgColHight - jumpHeight;          // 調整されたコライダーの高さ
                                float adjCenterY = orgVectColCenter.y + jumpHeight;
                                col.center = new Vector3(0, adjCenterY, 0); // 調整されたコライダーのセンター
                            }
                            else
                            {
                                // 閾値よりも低い時には初期値に戻す（念のため）
                                resetCollider();
                            }
                        }
                    }
                    // Jump bool値をリセットする（ループしないようにする）
                    _animation.SetBool("Jump", false);
                }
            }
            // IDLE中の処理
            // 現在のベースレイヤーがidleStateの時
            else if (currentBaseState.nameHash == idleState)
            {
                //カーブでコライダ調整をしている時は、念のためにリセットする
                if (useCurves)
                {
                    resetCollider();
                }
                // スペースキーを入力したらRest状態になる
                if (Input.GetButtonDown("Jump"))
                {
                    _animation.SetBool("Rest", true);
                }
            }
            // REST中の処理
            // 現在のベースレイヤーがrestStateの時
            else if (currentBaseState.nameHash == restState)
            {
                //cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
                // ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
                if (!_animation.IsInTransition(0))
                {
                    _animation.SetBool("Rest", false);
                }
            }
        }

        private bool CheckGrounded()
        {
            // 放つ光線の初期位置と姿勢
            // 若干身体にめり込ませた位置から発射しないと正しく判定できない時がある
            var ray = new Ray(origin: transform.position + Vector3.up * _rayOffset, direction: Vector3.down);

            // Raycastがhitするかどうかで判定
            // レイヤの指定を忘れずに
            return Physics.Raycast(ray, _rayLength, _layerMask);
        }

        // キャラクターのコライダーサイズのリセット関数
        void resetCollider()
        {
            // コンポーネントのHeight、Centerの初期値を戻す
            col.height = orgColHight;
            col.center = orgVectColCenter;
        }
    }
}