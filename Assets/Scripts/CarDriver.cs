using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    private float driveForce = 13500f;
    public List<Wheel> turning;
    public List<Wheel> driver;
    float trnAmt = 0;
    float fwdAmt = 0;

    void Update()
    {
        //ManualControl();
    }
    private void FixedUpdate()
    {
        ActInputs(fwdAmt, trnAmt);
    }
    public void ManualControl()
    {
        trnAmt = Input.GetAxis("Horizontal");
        fwdAmt = Input.GetAxis("Vertical");
    }
    public void SetInputs(float fwd, float trn)
    {
        fwdAmt = fwd;
        trnAmt = trn;
    }
    public void ActInputs(float fwd, float trn)
    {
        foreach (Wheel w in turning)
        {
            w.Steer(trn);
        }
        foreach (Wheel w in driver)
        {
            w.Accelerate(fwd * driveForce * Time.deltaTime);
        }

    }
    public void StopCompletely()
    {
        SetInputs(0, 0);
    }
}
