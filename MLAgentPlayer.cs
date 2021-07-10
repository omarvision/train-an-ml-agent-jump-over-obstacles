using Unity.MLAgents;
using UnityEngine;

public class MLAgentPlayer : Agent //MonoBehaviour
{
    public float force = 15f;
    public Transform reset = null;
    public TextMesh score = null;
    public GameObject thrust = null;
    private Rigidbody rb = null;
    private float points = 0;
    
    public override void Initialize()
    {
        rb = this.GetComponent<Rigidbody>();
        ResetMyAgent();
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        if (vectorAction[0] == 1)
        {
            UpForce();
            thrust.SetActive(true);
        }
        else
        {
            thrust.SetActive(false);
        }            
    }
    public override void OnEpisodeBegin()
    {
        ResetMyAgent();
    }
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.UpArrow) == true)
            actionsOut[0] = 1;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle") == true)
        {
            AddReward(-1.0f);          
            Destroy(collision.gameObject);
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("walltop") == true)
        {
            AddReward(-0.9f);
            EndEpisode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wallreward") == true)
        {
            AddReward(0.1f);
            points++;
            score.text = points.ToString();
        }     
    }
    private void UpForce()
    {
        rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
    }
    private void ResetMyAgent()
    {
        this.transform.position = new Vector3(reset.position.x, reset.position.y, reset.position.z);
    }
}
