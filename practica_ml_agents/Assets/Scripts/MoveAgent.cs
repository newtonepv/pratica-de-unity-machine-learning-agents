using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveAgent : Agent
{
    [SerializeField]
    Material looseMaterial;
    [SerializeField]
    Material winMaterial;
    [SerializeField]
    MeshRenderer floorMeshRenderer;

    [SerializeField] 
    private Transform goal;
    [SerializeField] 
    private float altura;
    [SerializeField] 
    private Vector2 AgentXLimits;
    [SerializeField] 
    private Vector2 AgentZLimits;
    [SerializeField] 
    private Vector2 GoalXLimits;
    [SerializeField] 
    private Vector2 GoalZLimits;

    [SerializeField]
    private float speed = 1f;
    private Rigidbody rb;

    public override void OnEpisodeBegin()
    {
        goal.localPosition = new Vector3(Random.Range(GoalXLimits.x, GoalXLimits.y), 0, Random.Range(GoalZLimits.x, GoalZLimits.y));
        transform.localPosition = new Vector3(Random.Range(AgentXLimits.x, AgentXLimits.y), altura, Random.Range(AgentZLimits.x, AgentZLimits.y));
        rb = GetComponent<Rigidbody>();
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(goal.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        rb.velocity = new Vector3(actions.ContinuousActions[0]*speed, 0, actions.ContinuousActions[1]*speed);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer);
        if (other.gameObject.layer == 6)
        {
            floorMeshRenderer.material = winMaterial;
            SetReward(1f);
            EndEpisode();
        }
        else if( other.gameObject.layer == 7)
        {
            floorMeshRenderer.material = looseMaterial;
            SetReward(-1f);
            EndEpisode();
        }
    }   
}
