using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static State;

public class CrowdController : MonoBehaviour
{
    GameObject[] goalLocations;
    GameObject[] agents;
    

    public float detectionRadius = 10.0f;
    public float fleeRadius = 10.0f;

    public float minSpeed = 1.0f; 
    public float speedMult;

    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("Agent");
        goalLocations = GameObject.FindGameObjectsWithTag("Goal");
        int i = Random.Range(0, goalLocations.Length);

        foreach (GameObject go in agents)
        {
            go.GetComponent<NavMeshAgent>().SetDestination(goalLocations[i].transform.position);
            go.GetComponent<Animator>().SetFloat("wOffset", Random.Range(0.5f, 1.0f));
            ResetAgent(go);

        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject go in agents)
        {

            if (go.GetComponent<NavMeshAgent>().remainingDistance < 0.5f)
            {
                ResetAgent(go);
                int i = Random.Range(0, goalLocations.Length);
                go.GetComponent<NavMeshAgent>().SetDestination(goalLocations[i].transform.position);
            }
        }

    }
    public void DetectNewObstacle(Vector3 position)
    {
        foreach (GameObject go in agents)
        {
            NavMeshAgent agent = go.GetComponent<NavMeshAgent>();
            Animator anim = go.GetComponent<Animator>();
            if (Vector3.Distance(position, go.transform.position) < detectionRadius)
            {
                Vector3 fleeDirection = (go.transform.position - position).normalized;
                Vector3 newGoal = go.transform.position + fleeDirection * fleeRadius;

                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(newGoal, path);

                if (path.status != NavMeshPathStatus.PathInvalid)
                {
                    
                    agent.SetDestination(path.corners[path.corners.Length - 1]);
                    anim.SetTrigger("isRunning");
                    agent.speed = 5;
                    agent.angularSpeed = 500;
                }
            }
        }
    }

    void ResetAgent(GameObject go)
    {
        NavMeshAgent agent = go.GetComponent<NavMeshAgent>();
        Animator animator = go.GetComponent<Animator>();

        speedMult = Random.Range(0.5f, 1.1f);
        float newSpeed = agent.speed * speedMult;
        agent.speed = Mathf.Max(newSpeed, minSpeed); 
        animator.SetFloat("speedM", speedMult);
        animator.SetTrigger("isWalking");

        agent.angularSpeed = 120;
        agent.ResetPath();
    }

    public void OnValueChanged(float value)
    {
        detectionRadius = value;
    }

    public void OnValueChangedFleeRange(float value)
    {
        fleeRadius = value;
    }
}
