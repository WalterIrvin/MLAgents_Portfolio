using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public Material green;
    public Material red;
    private bool isGreen = false;
    public bool IsGreen()
    {
        return isGreen;
    }
    public void resetLight()
    {
        // resets the light back to red
        isGreen = false;
        updateState();
    }
    public void turnLight()
    {
        // turns the light over to the next cycle
        isGreen = !isGreen;
        updateState();
    }
    private void updateState()
    {
        // updates the material color of the light
        if (isGreen)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Renderer>().material = green;
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Renderer>().material = red;
            }
        }
    }
}
