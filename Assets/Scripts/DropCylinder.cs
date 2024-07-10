using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCylinder : MonoBehaviour
{
    public GameObject obstacle;
    public CrowdController controller;

    public GameObject text;
    public bool canPlace;
    GameObject[] agents;

    public int cylinderNum;

    private bool isLeftMouseButtonHeld = false; 
   // private bool isRightMouseButtonHeld = false; 
    // Start is called before the first frame update
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("Agent");
    }

    // Update is called once per frame
    void Update()
    {
        text.SetActive(canPlace);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
             bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            if (Input.GetMouseButtonDown(0) && !isLeftMouseButtonHeld && canPlace && !isOverUI)
            {

                isLeftMouseButtonHeld = true;
                Instantiate(obstacle, hit.point, obstacle.transform.rotation);
                controller.DetectNewObstacle(hit.point);
            }

            if (Input.GetMouseButtonUp(0))
            {
                isLeftMouseButtonHeld = false;
            }
            if (Input.GetMouseButton(1))
            {
                if (hit.transform.tag == "Cylinder")
                {
                    Destroy(hit.transform.gameObject);
                }
            }

        }
    }

    public void OnToggleChange(bool state)
    {
        canPlace = state;
    }
}
