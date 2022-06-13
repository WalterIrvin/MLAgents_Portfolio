using System;
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
            DriverAgent agento = player.GetComponent<DriverAgent>();
            agento.setActiveTrack(trackCheckpoints);
            trackCheckpoints.CarThroughCheckpoint(this, other.transform);
            if (gameObject.TryGetComponent<Intersection>(out Intersection inter))
            {
                // Check if agent is running a red or green light
                if (inter.isAllowed)
                {
                    RewardCard(agento);
                }
                else if (!inter.isAllowed)
                {
                    PunishCar(agento);
                }
            }
        }
    }
    public void PunishCar(DriverAgent agent)
    {
        agent.PunishmentCheckpoint();
    }

    public void RewardCard(DriverAgent agent)
    {
        agent.RewardCheckpoint();
    }
    public void setTrackCheckpoints(TrackCheckpoints track)
    {
        trackCheckpoints = track;
    }
}
