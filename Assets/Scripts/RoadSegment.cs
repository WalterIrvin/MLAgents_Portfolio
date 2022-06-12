using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    public List<TrackCheckpoints> trackcheckpoints;
    public void InitCars(List<Transform> cars)
    {
        foreach(TrackCheckpoints track in trackcheckpoints)
        {
            track.InitCars(cars);
        }
    }
}
