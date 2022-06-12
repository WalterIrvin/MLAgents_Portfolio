using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarDriver>(out CarDriver player))
        {
            trackCheckpoints.CarThroughCheckpoint(this, other.transform);
        }
    }
    public void setTrackCheckpoints(TrackCheckpoints track)
    {
        trackCheckpoints = track;
    }
}
