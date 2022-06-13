using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class DriverAgent : Agent
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPos;
    private CarDriver carDriver;
    public class RewardSystem
    {
        // Current total of all punish/reward hyperparameters
        public float redLightPunish = -1f;
        public float greenLightReward = 0.26f;
        public float correctCheckpoint = 0.25f;
        public float incorrectCheckpoint = -0.23f;
        public float barrierInitHit = -0.5f;
        public float barrierStick = -0.01f;
    }
    private RewardSystem rewardsys;
    void Awake()
    {
        rewardsys = new RewardSystem();
        carDriver = GetComponent<CarDriver>();
    }
    public void setActiveTrack(TrackCheckpoints track)
    {
        trackCheckpoints = track;
    }
    public void PunishmentCheckpoint()
    {
        // Punish car for running red light
        AddReward(rewardsys.redLightPunish);
    }
    public void RewardCheckpoint()
    {
        // Reward car for running green light
        AddReward(rewardsys.greenLightReward);
    }
    private void Start()
    {
        trackCheckpoints.OnCarCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        trackCheckpoints.OnCarWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
    }
    private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, EventArgs e)
    {
        TrackCheckpoints.CarCheckpointEventArgs args = (TrackCheckpoints.CarCheckpointEventArgs)e;
        if (args.car == transform)
        {
            AddReward(rewardsys.incorrectCheckpoint);
            EndEpisode();
        }
    }
    private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, EventArgs e)
    {
        TrackCheckpoints.CarCheckpointEventArgs args = (TrackCheckpoints.CarCheckpointEventArgs)e;
        if (args.car == transform)
        {
            AddReward(rewardsys.correctCheckpoint);
        }
    }

    public override void OnEpisodeBegin()
    {
        //Vector3 offset = new Vector3(UnityEngine.Random.Range(0, 4), 0, UnityEngine.Random.Range(0, 4));
        transform.position = spawnPos.position;
        transform.rotation = spawnPos.rotation;
        trackCheckpoints.ResetCheckpoint(transform);
        carDriver.StopCompletely();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        Transform nextCheck = trackCheckpoints.GetNextCheckpoint(transform);
        float isAllowed = 1;
        if (nextCheck.gameObject.TryGetComponent<Intersection>(out Intersection inter))
        {
            if (!inter.isAllowed)
            {
                isAllowed = 0;
            }
        }
        Vector3 checkpointForward = nextCheck.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);
        sensor.AddObservation(isAllowed);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmt = 0f;
        float turnAmt = 0f;
        switch (actions.DiscreteActions[0])
        {
            case 0:
                forwardAmt = 0f;
                break;
            case 1:
                forwardAmt = 1f;
                break;
            case 2:
                forwardAmt = -1f;
                break;
            default:
                forwardAmt = 0f;
                break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0:
                turnAmt = 0f;
                break;
            case 1:
                turnAmt = 1f;
                break;
            case 2:
                turnAmt = -1f;
                break;
            default:
                turnAmt = 0f;
                break;
        }
        carDriver.SetInputs(forwardAmt, turnAmt);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAct = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            forwardAct = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            forwardAct = 2;
        }
        int turnAct = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            turnAct = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turnAct = 2;
        }
        ActionSegment<int> discreteAct = actionsOut.DiscreteActions;
        discreteAct[0] = forwardAct;
        discreteAct[1] = turnAct;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Barrier>(out Barrier barrier))
        {
            AddReward(rewardsys.barrierInitHit);
            EndEpisode();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Barrier>(out Barrier barrier))
        {
            AddReward(rewardsys.barrierStick);
            EndEpisode();
        }
    }
}
