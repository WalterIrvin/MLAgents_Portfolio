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
            trackCheckpoints.PlayerThroughCheckpoint(this);
        }
    }
    public void setTrackCheckpoints(TrackCheckpoints track)
    {
        trackCheckpoints = track;
    }
}
