using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed;
    float groupSpeed = 0.01f;
    float nDistance;
    int groupSize = 0;
    List<GameObject> allFish = new List<GameObject>();

    bool turning = false;
    bool moveToGoal = false;
    Vector3 currentGoal;

    void Start()
    {
        allFish = FlockManager.FM.fishes;
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
    }

    void Update()
    {
        Bounds bounds = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);


        if (moveToGoal)
        {
            MoveTowardsGoal();
        }
        else
        {
            if (!bounds.Contains(transform.position))
            {
                turning = true;
            }
            else
            {
                turning = false;
            }

            if (turning)
            {
                Vector3 direction = FlockManager.FM.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
            if (Random.Range(0, 100) < 10)
            {
                speed = Mathf.Lerp(speed, Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed), Time.deltaTime);
            }

            if (Random.Range(0, 100) < 10)
            {
                ApplyRules();
            }
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    public void StopMovingToGoal()
    {
        moveToGoal = false;
    }

    public bool IsMovingToGoal()
    {
        return moveToGoal;
    }

    public void MoveToGoalPosition(Vector3 goal)
    {
        speed = FlockManager.FM.maxSpeed;
        currentGoal = goal;
        moveToGoal = true;
    }

    void MoveTowardsGoal()
    {
        Vector3 direction = currentGoal - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentGoal) < 0.1f)
        {
            moveToGoal = false;
        }
    }

    void ApplyRules()
    {
        Vector3 center = Vector3.zero;
        Vector3 avoid = Vector3.zero;
        groupSpeed = 0.01f;
        groupSize = 0;

        foreach (GameObject fish in allFish)
        {
            if (fish != this.gameObject)
            {
                nDistance = Vector3.Distance(fish.transform.position, this.transform.position);
                if (nDistance <= FlockManager.FM.neighbourDistance)
                {
                    center += fish.transform.position;
                    groupSize++;

                    if (nDistance < 1.0f)
                    {
                        avoid += (this.transform.position - fish.transform.position);
                    }

                    Flock anotherFlock = fish.GetComponent<Flock>();
                    groupSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            center = center / groupSize + (FlockManager.FM.goalPosition - this.transform.position);
            speed = Mathf.Lerp(speed, groupSpeed / groupSize, Time.deltaTime);

            if (speed > FlockManager.FM.maxSpeed)
            {
                speed = FlockManager.FM.maxSpeed;
            }

            Vector3 direction = (center + avoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
        }
    }
}