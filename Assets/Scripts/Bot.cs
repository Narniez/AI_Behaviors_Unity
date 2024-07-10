using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public Transform roomCenter; 


    CopMove copSpeed;
    Vector3 wanderTarget = Vector3.zero;

    //GameObject currentHidingSpot = null;

    GameObject currentHideSpot = null;
    bool isChangingHideSpot = false;

    public float stuckThreshold = 0.5f; 
    public float randomDirectionRadius = 5.0f; 
    private Vector3 lastPosition;    
    public float checkInterval = 0.5f; 
    private float lastCheckTime;

    bool canEvade = false;
    bool canHide = false;
    bool canChase = false;
    bool canWander = false;


    private void Start()
    {
        copSpeed = target.GetComponent<CopMove>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (canEvade)
        {
           CheckAndEvade();
        }
        if(canHide)
        {
            CleverHide();
        }
        if(canChase)
        {
            Pursue();
        }
        if(canWander)
        {
            Wander();
        }

        if (CanSeeTarget())
        {
        }
    }

    bool CanSeeTarget()
    {
        RaycastHit hit;
        Vector3 rayToTarget = target.transform.position - this.transform.position;

        float lookAngle = Vector3.Angle(this.transform.forward, rayToTarget);


        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), rayToTarget);
        if (lookAngle < 60 && Physics.Raycast(this.transform.position + new Vector3(0, 1, 0), rayToTarget, out hit))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeDirection = (this.transform.position - location).normalized * randomDirectionRadius;
        fleeDirection += this.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeDirection, out hit, randomDirectionRadius, 1))
        {
            fleeDirection = hit.position;
        }

        // Set the agent's destination to the new flee direction
        agent.SetDestination(fleeDirection);

        
       //Debug.DrawLine(this.transform.position, fleeDirection, Color.red, 2.0f);
    }

    void RandomFlee(Vector3 fleeTarget)
    {
        Vector3 fleeDirection = (this.transform.position - fleeTarget).normalized;
        Vector3 randomOffset = Random.insideUnitSphere * randomDirectionRadius;
        randomOffset.y = 0; 

        Vector3 randomFleeDirection = (fleeDirection + randomOffset).normalized * randomDirectionRadius;
        randomFleeDirection += this.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomFleeDirection, out hit, randomDirectionRadius, 1))
        {
            randomFleeDirection = hit.position;
        }

        // Set the agent's destination to the new random flee direction
        agent.SetDestination(randomFleeDirection);

        
        //Debug.DrawLine(this.transform.position, randomFleeDirection, Color.red, 2.0f);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));



        if ((toTarget > 90 && relativeHeading < 20) || copSpeed.currentSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / agent.speed + copSpeed.currentSpeed;
        Seek(target.transform.position + target.transform.forward * lookAhead);

    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / agent.speed + copSpeed.currentSpeed;
        RandomFlee(target.transform.position + target.transform.forward * lookAhead);
    }

    void CheckAndEvade()
    {
        Evade();

        // Check if the agent is stuck at regular intervals
        if (Time.time - lastCheckTime >= checkInterval)
        {
            if (agent.velocity.magnitude < stuckThreshold)
            {
                ChangeDirection();
            }

            lastCheckTime = Time.time;
        }
        AvoidWall();
    }

    void AvoidWall()
    {
        RaycastHit hit;
        Vector3 forward = agent.transform.forward;
        if (Physics.Raycast(agent.transform.position, forward, out hit, 3))
        {
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0; // Keep the direction on the horizontal plane
            Vector3 avoidDirection = Vector3.Cross(hitNormal, Vector3.up).normalized;
            Vector3 newDirection = (agent.transform.position + avoidDirection * 3);

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(newDirection, out navHit, 5, 3))
            {
                agent.SetDestination(navHit.position);
            }

            
           // Debug.DrawLine(agent.transform.position, navHit.position, Color.blue, 2.0f);
        }
    }

    void ChangeDirection()
    {
        // Calculate direction towards the center of the room
        Vector3 directionToCenter = (roomCenter.position - agent.transform.position).normalized;

        // Choose a random direction
        Vector3 randomDirection = Random.insideUnitSphere * randomDirectionRadius;
        randomDirection.y = 0; // Keep the direction on the horizontal plane

        // Combine the random direction with the direction to the center
        Vector3 combinedDirection = (randomDirection + directionToCenter).normalized * randomDirectionRadius;
        combinedDirection += agent.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(combinedDirection, out hit, randomDirectionRadius, 1))
        {
            combinedDirection = hit.position;
        }

        // Ensure the combined direction is not towards the target
        Vector3 directionToTarget = (target.transform.position - agent.transform.position).normalized;
        Vector3 directionToCombined = (combinedDirection - agent.transform.position).normalized;
        if (Vector3.Dot(directionToTarget, directionToCombined) > 0)
        {
            // If the combined direction is towards the target, invert it
            combinedDirection = agent.transform.position - directionToCombined * randomDirectionRadius;
            if (NavMesh.SamplePosition(combinedDirection, out hit, randomDirectionRadius, 1))
            {
                combinedDirection = hit.position;
            }
        }

        // Set the agent's destination to the new combined direction
        agent.SetDestination(combinedDirection);

        
       //Debug.DrawLine(agent.transform.position, combinedDirection, Color.green, 2.0f);
    }
    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 1;
        float wanderJitter = 3;

        // Add small random vector to the wander target
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);

        // Normalize the wander target to stay within the unit circle and then scale it by the wander radius
        wanderTarget = wanderTarget.normalized * wanderRadius;

        // Project the wander target forward by the wander distance
        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);

        // Convert the local target position to a world position
        Vector3 targetWorld = this.gameObject.transform.TransformPoint(targetLocal);

        Seek(targetWorld);
    }

   

    void CleverHide()
    {
        float distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);

        if (isChangingHideSpot)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isChangingHideSpot = false;
            }
        }

        if (distanceToTarget < 2 && !isChangingHideSpot)
        {
            ChangeToRandomHideSpot();
        }
        else if (!isChangingHideSpot)
        {
            PerformHidingBehavior();
        }
    }

    public void ChangeBehavior(string behavior)
    {
        switch (behavior)
        {
            case"evade":
                   canEvade = true;
                canHide = false;
                canWander = false;
                canChase = false;
                break;
            case "hide":
                canHide = true;
                canEvade = false;
                canWander = false;
                canChase = false;
                break;
            case "wander":
                canWander = true;
                canChase = false;
                canEvade = false;
                canHide = false;
                break;
            case "chase":
                canChase = true;
                canEvade= false;
                canHide= false;
                canWander= false;
                break;
        }


    }

    void ChangeToRandomHideSpot()
    {
        GameObject[] hidingSpots = World.Instance.GetHidingSpots();
        List<GameObject> validHidingSpots = new List<GameObject>(hidingSpots);

        // Remove current hiding spot from valid spots
        if (currentHideSpot != null)
        {
            validHidingSpots.Remove(currentHideSpot);
        }

        // Choose a new hiding spot randomly from valid spots
        if (validHidingSpots.Count > 0)
        {
            GameObject chosenHideSpot = validHidingSpots[Random.Range(0, validHidingSpots.Count)];

            // Calculate hide position slightly offset from the chosen hiding spot
            Vector3 hideDir = chosenHideSpot.transform.position - target.transform.position;
            Vector3 hidePos = chosenHideSpot.transform.position + hideDir.normalized * 2;

            currentHideSpot = chosenHideSpot;

            // Move towards the new hiding spot
            Seek(hidePos);

           
            isChangingHideSpot = true;
        }
    }

    void PerformHidingBehavior()
    {
        if (currentHideSpot != null)
        {
            float dist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;
            Vector3 chosenDirection = Vector3.zero;

            Vector3 hideDir = currentHideSpot.transform.position - target.transform.position;
            Vector3 hidePos = currentHideSpot.transform.position + hideDir.normalized * 1;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDirection = hideDir;
                dist = Vector3.Distance(this.transform.position, hidePos);
            }

            Collider hideCol = currentHideSpot.GetComponent<Collider>();
            Ray backRay = new Ray(chosenSpot, -chosenDirection.normalized);
            RaycastHit hit;
            float distance = 100.0f;

            if (hideCol.Raycast(backRay, out hit, distance))
            {
                Seek(hit.point + chosenDirection.normalized * 2);
            }
            else
            {
                Seek(chosenSpot);
            }
        }
    }
}