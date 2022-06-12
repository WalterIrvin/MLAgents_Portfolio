using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private Vector3 old_pos;
    private float brakeTor = 10000f;
    public bool powered = false;
    private float maxAngle = 80;
    public float offset = 0f;
    private float old_torque = 0f;
    private float turnAngle;
    private float dist_cutoff = 0.015f; // anything less than this and the cars exits braking mode and applies new torque
    public WheelCollider wcol;
    public Transform wmesh;

    public void Steer(float steerInput)
    {
        
        turnAngle = steerInput * maxAngle + offset;
        wcol.steerAngle = turnAngle;
        Debug.Log(wcol.steerAngle);
        UpdatePosition();
    }

    public void Accelerate(float powerInput)
    {
        if (powered)
        {
            
            if ((powerInput > 0 && old_torque < 0) || (powerInput < 0 && old_torque > 0))
            {
                Debug.Log("cur tor: " + powerInput + " old tor: " + old_torque);
                float diff = (transform.position - old_pos).magnitude;
                Debug.Log(diff);
                if (diff < dist_cutoff)
                {
                    old_torque = powerInput;
                    wcol.brakeTorque = 0;
                }
                else
                {
                    Debug.Log("eepo :" + old_torque);
                    wcol.brakeTorque = brakeTor;
                    old_pos = transform.position;
                }
            }
            else if (Mathf.Abs(powerInput) > 0)
            {
                old_torque = powerInput;
                old_pos = transform.position;
                wcol.motorTorque = powerInput;
            }
            else
            {
                float diff = (transform.position - old_pos).magnitude;
                if (diff <= dist_cutoff + 0.05)
                {
                    old_torque = powerInput;
                    wcol.brakeTorque = 0;
                }
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
