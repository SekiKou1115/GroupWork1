using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRamp : MonoBehaviour
{
    [SerializeField] private float jumpForce = 20.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Player ‚ªG‚ê‚½
        if (other.gameObject.CompareTag("Player"))
        {
            // G‚ê‚½‘Šè‚ÌRigidbody‚ğæ“¾‚µ‚ÄAãŒü‚«‚É—Í‚ğ‰Á‚¦‚é
            other.gameObject.GetComponent<Rigidbody>().
                AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
    }
}