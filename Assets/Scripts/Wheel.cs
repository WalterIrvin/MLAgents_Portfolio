using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public bool powered = false;
    private float maxAngle = 80;
    public float offset = 0f;
    private float turnAngle;
    public WheelCollider wcol;
    public Transform wmesh;
    private float tor = 10000f;

    public void Steer(float steerInput)
    {
        
        turnAngle = steerInput * maxAngle + offset;
        wcol.steerAngle = turnAngle;
        UpdatePosition();
    }

    public void Accelerate(float powerInput)
    {
        if (powered)
        {
            
            if (Mathf.Abs(powerInput) > 0)
            {
                wcol.brakeTorque = 0;
                wcol.motorTorque = powerInput;
            }
            else
            {
                wcol.brakeTorque = tor;
                wcol.motorTorque = 0;
            }
        }
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        wcol.GetWorldPose(out pos, out rot);
        wmesh.transform.position = pos;
        wmesh.transform.rotation = rot;
    }
}
