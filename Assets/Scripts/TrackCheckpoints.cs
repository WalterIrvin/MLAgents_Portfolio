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
    bool needsReset = false;
    private void Awake()
    {
        nextCheckpointIDXList = new List<int>();
        checkpointList = new List<Checkpoint>();
        foreach (Transform checkpointSingle in transform)
        {
            Checkpoint single = checkpointSingle.GetComponent<Checkpoint>();
            if (single.gameObject.activeSelf)
            {
                single.setTrackCheckpoints(this);
                checkpointList.Add(single);
            }
        }
        if (carList == null)
        {
            carList = new List<Transform>();
        }
        else
        {
            InitCars(carList);
        }
    }
    public void Update()
    {
       if (needsReset && initialized)
       {
            foreach(Transform car in carList)
            {
                ResetCheckpoint(car);
            }
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
            int nextCheckpointIdx = 0;
            if (carIdx < nextCheckpointIDXList.Count)
            {
                nextCheckpointIdx = nextCheckpointIDXList[carIdx];
            }
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
        if (initialized)
        {
            int carIdx = carList.IndexOf(car);
            nextCheckpointIDXList[carIdx] = 0;
            needsReset = false;
        }
        else
        {
            needsReset = true;
        }
    }

    public Transform GetNextCheckpoint(Transform car)
    {
        if (initialized)
        {
            if (carList != null && checkpointList != null)
            {
                int carIdx = carList.IndexOf(car);
                int nextCheckpointIdx = 0;
                if (carIdx < nextCheckpointIDXList.Count)
                {
                    nextCheckpointIdx = nextCheckpointIDXList[carIdx];
                }
                else
                {
                    Debug.Log(nextCheckpointIDXList.Count + " cur car idx: " + carIdx);
                }
                if (nextCheckpointIdx < checkpointList.Count)
                {
                    return checkpointList[nextCheckpointIdx].transform;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
