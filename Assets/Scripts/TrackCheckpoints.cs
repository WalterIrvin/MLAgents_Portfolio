using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnCarCorrectCheckpoint;
    public event EventHandler OnCarWrongCheckpoint;
    public class CarCheckpointEventArgs : EventArgs
    {
        public Transform car;
    }

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
                nextCheckpointIdx = (nextCheckpointIdx + 1) % checkpointList.Count;
                nextCheckpointIDXList[carIdx] = nextCheckpointIdx;
                CarCheckpointEventArgs correct = new CarCheckpointEventArgs();
                correct.car = car;
                OnCarCorrectCheckpoint?.Invoke(this, correct);
            }
            else
            {
                CarCheckpointEventArgs incorrect = new CarCheckpointEventArgs();
                incorrect.car = car;
                OnCarWrongCheckpoint?.Invoke(this, incorrect);
            }
        }
    }

    public void ResetCheckpoint(Transform car)
    {
        int carIdx = carList.IndexOf(car);
        nextCheckpointIDXList[carIdx] = 0;
    }

    public Transform GetNextCheckpoint(Transform car)
    {
        int carIdx = carList.IndexOf(car);
        int nextCheckpointIdx = nextCheckpointIDXList[carIdx];
        return checkpointList[nextCheckpointIdx].transform;
    }
}
