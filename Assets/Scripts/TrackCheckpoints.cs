using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    private List<Checkpoint> checkpointList;
    private void Awake()
    {
        checkpointList = new List<Checkpoint>();
        foreach (Transform checkpointSingle in transform)
        {
            Checkpoint single = checkpointSingle.GetComponent<Checkpoint>();
            single.setTrackCheckpoints(this);
            checkpointList.Add(single);
        }
    }
    public void PlayerThroughCheckpoint(Checkpoint checkpoint)
    {
        Debug.Log(checkpointList.IndexOf(checkpoint));
    }

    public void ResetCheckpoint(Transform obj)
    {

    }

    public Transform GetNextCheckpoint(Transform obj)
    {
        return transform;
    }
}
