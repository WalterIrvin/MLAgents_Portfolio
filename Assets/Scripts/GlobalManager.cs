using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public List<Transform> cars;
    public List<RoadSegment> roadSegments;
    void Start()
    {
        foreach (RoadSegment roads in roadSegments)
        {
            roads.InitCars(cars);
        }
    }
}
