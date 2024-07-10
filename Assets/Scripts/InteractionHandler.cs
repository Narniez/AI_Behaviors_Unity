using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionHandler : MonoBehaviour
{
    private Camera cameraObject;
    public GameObject pressF;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = FindObjectOfType<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = cameraObject.ScreenPointToRay(Input.mousePosition);

        //Raycast for object detection using tags
        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            //Debug.DrawRay(cameraObject.transform.position, Input.mousePosition, Color.green);

            if (hit.transform.CompareTag("LevelOneButton"))
            {
                pressF.SetActive(true);
                //Debug.Log("Looking at Button");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("Activate AI");
                    GameEnviroment.Singleton.activateLevelOneAI = true;
                }
            }

            else if (hit.transform.CompareTag("LevelTwoButton"))
            {
                pressF.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                }
            }
            else
            {
                pressF.SetActive(false);
            }

        }
    }
}
