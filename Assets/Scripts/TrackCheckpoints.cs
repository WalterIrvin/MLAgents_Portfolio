using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

    [SerializeField] private List<Transform> carList;
    private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointIDXList;

    bool initialized = false;
    private void Awake()
    {
        carList = new List<Transform>();
        nextCheckpointIDXList = new List<int>();
        checkpointList = new List<Checkpoint>();
        foreach (Transform checkpointSingle in transform)
        {
            Checkpoint single = checkpointSingle.GetComponent<Checkpoint>();
            single.setTrackCheckpoints(this);
            checkpointList.Add(single);
        }
    }
    public void InitCars(List<Transform> cars)
    {
        if (!initialized)
        {
            carList = cars;
            foreach (Transform car in carList)
            {
                nextCheckpointIDXList.Add(0);
            }
            initialized = true;
        }
    }
    public void CarThroughCheckpoint(Checkpoint checkpoint, Transform car)
    {
        if (initialized)
        {
            int carIdx = carList.IndexOf(car);
            int nextCheckpointIdx = nextCheckpointIDXList[carIdx];
            int passedIdx = checkpointList.IndexOf(checkpoint);
            if (passedIdx == nextCheckpointIdx)
            {
                Debug.Log("good work");
                nextCheckpointIdx = (nextCheckpointIdx + 1) % checkpointList.Count;
                nextCheckpointIDXList[carIdx] = nextCheckpointIdx;
                OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.Log("wrong");
                OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void ResetCheckpoint(Transform obj)
    {

    }

    public Transform GetNextCheckpoint(Transform obj)
    {
        return transform;
    }
}
