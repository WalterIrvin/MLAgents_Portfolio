using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic_Controller : MonoBehaviour
{
    public float min_time = 5f;
    public float max_time = 10f;
    private float lightTimer;
    private float curLightTime = 0;
    public List<TrafficLight> lights;  // x, z
    public List<Intersection> intersectionX;
    public List<Intersection> intersectionZ;
    void Start()
    {
        InitTimer();
        InitLight();
    }

    void Update()
    {
        curLightTime += Time.deltaTime;
        if (curLightTime >= lightTimer)
        {
            SwitchLight();
            curLightTime = 0;
        }
    }
    private void UpdateRoadIntersections()
    {
        // Updates the intersections internal state based on if their associated light axis is green or red, used by the agents to determine light color instead of looking for the lights directly.
        bool xGreen = lights[0].IsGreen();
        bool zGreen = lights[1].IsGreen();
        foreach (Intersection intersect in intersectionX)
        {
            intersect.UpdateState(xGreen);
        }
        foreach (Intersection intersect in intersectionZ)
        {
            intersect.UpdateState(zGreen);
        }
    }
    private void InitTimer()
    {
        // Initializes the light switch timer to a random amount between minimum and maximum timing
        float randomTime = Random.Range(min_time, max_time);
        lightTimer = randomTime;
    }
    private void InitLight()
    {
        // Inits a random axis of lights to be green
        foreach (TrafficLight light in lights)
        {
            light.resetLight();
        }
        int randomizer = Random.Range(0, 1);
        if (randomizer == 1)
        {
            lights[0].turnLight();
        }
        else
        {
            lights[1].turnLight();
        }
        UpdateRoadIntersections();


    }
    public void SwitchLight()
    {
        foreach (TrafficLight light in lights)
        {
            light.turnLight();
        }
        UpdateRoadIntersections();
    }
}
