using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM;
    public UniversalRendererData rendererData;

    public Camera mainCam;
    public GameObject fishPrefab;
    public int numFish = 20;
    public Vector3 swimLimits = new Vector3(5, 5, 5);
    public Vector3 goalPosition = Vector3.zero;

    public List<GameObject> fishes = new List<GameObject>();

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(0.0f, 10.0f)]
    public float neighbourDistance;
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;

    bool shouldFollowMouse = false;
    bool isUnderwater = false;
    void Start()
    {
        
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y), Random.Range(-swimLimits.z, swimLimits.z));

            GameObject newFish = Instantiate(fishPrefab, pos, Quaternion.identity, this.transform);
            fishes.Add(newFish);
        }

        FM = this;
        goalPosition = this.transform.position;
    }

    public void FishFollowMouse()
    {
        shouldFollowMouse = !shouldFollowMouse;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public void SetMaxSpeed(float speed)
    {
        maxSpeed = speed;
    }

    public void SetNeighbourDistance(float distance)
    {
        neighbourDistance = distance;
    }

    public void AddFish()
    {
        Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
               Random.Range(-swimLimits.y, swimLimits.y), Random.Range(-swimLimits.z, swimLimits.z));
        GameObject newFish = Instantiate(fishPrefab, pos, Quaternion.identity);
        fishes.Add(newFish);
    }

    public void TurnOnWaterCamera(bool value)
    {
        isUnderwater = !isUnderwater;
        if (rendererData == null)
        {
            Debug.LogError("Renderer Data is not assigned.");
            return;
        }

        // Enable all renderer features
        foreach (var feature in rendererData.rendererFeatures)
        {
            if (feature != null)
            {
                feature.SetActive(value);
            }
        }
    }
    void Update()
    {
        if (shouldFollowMouse)
        {
            Debug.Log("Shoud FOllow");
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    goalPosition = hit.point;
                    foreach (GameObject fish in fishes)
                    {
                        fish.GetComponent<Flock>().MoveToGoalPosition(goalPosition);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Stop moving");
            foreach (GameObject fish in fishes)
            {
                if (fish.GetComponent<Flock>().IsMovingToGoal())
                {

                    fish.GetComponent<Flock>().StopMovingToGoal();
                }
            }
        }
    }
}