using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    Transform currentDestination;
    Transform targetPosition;


    public Transform playerTransform;
    [SerializeField] NavMeshAgent agent;
    Animator animator;

    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        targetPosition = endPos;  // Start by moving to the endPos
        currentDestination = startPos;
        agent.SetDestination(playerTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;


        if (timer < 0.0f)
        {
            float sqDistance = (playerTransform.position - agent.destination).sqrMagnitude;
            if (sqDistance > maxDistance * maxDistance)
            {
                agent.destination = playerTransform.position;

            }
            timer = maxTime;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);

    }

    // Switch the destination based on the current position
    void SwitchDestination()
    {
        if (currentDestination == startPos)
        {
            currentDestination = endPos;
            targetPosition = startPos;
        }
        else
        {
            currentDestination = startPos;
            targetPosition = endPos;
        }

        agent.destination = currentDestination.position;
    }
}