using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCustomAxis : MonoBehaviour
{
    public bool xInversion, yInversion;

    void Start()
    {
        Cinemachine.CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            return Input.GetAxis(axisName) * (xInversion ? -1f : 1f);
        }
        else if (axisName == "Mouse Y")
        {
            return Input.GetAxis(axisName) * (yInversion ? -1 : 1);
        }

        return 0;
    }
}
