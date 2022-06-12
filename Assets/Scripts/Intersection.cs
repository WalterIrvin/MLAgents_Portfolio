using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public bool isAllowed = false; // are cars allowed to drive through this intersection?
    public Material green;
    public Material red;
    void Start()
    {
        
    }

    public void UpdateState(bool newstate)
    {
        isAllowed = newstate;
        if (isAllowed)
        {
            gameObject.GetComponent<Renderer>().material = green;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = red;
        }
        
    }

}
