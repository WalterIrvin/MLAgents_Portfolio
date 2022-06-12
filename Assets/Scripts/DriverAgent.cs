using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class DriverAgent : Agent
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPos;
    private CarDriver carDriver;
    void Awake()
    {
        carDriver = GetComponent<CarDriver>();
    }
    private void Start()
    {
        
    }
    //private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e)
    //{

    //}
    //private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e)
    //{

    //}
    public override void OnEpisodeBegin()
    {
        transform.position = spawnPos.position;
        transform.forward = spawnPos.forward;
        trackCheckpoints.ResetCheckpoint(transform);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 checkpointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);
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
            AddReward(-0.5f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Barrier>(out Barrier barrier))
        {
            AddReward(-0.1f);
        }
    }
}