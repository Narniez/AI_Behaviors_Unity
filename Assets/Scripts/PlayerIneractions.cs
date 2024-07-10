using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIneractions : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float raycastRange = 10f;

    public GameObject pressF;

    public LevelChanger levelChanger;
    // Update is called once per frame
    void Update()
    {
        CheckForLevelButton();
    }

    private void CheckForLevelButton()
    {
        // Perform the raycast from the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            if (hit.collider.CompareTag("LevelButton"))
            {
                pressF.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {

                    if (hit.collider.GetComponent<LevelButton>())
                    {
                        levelChanger.GoToLevel(hit.collider.GetComponent<LevelButton>().level);
                    }
                }
            }
            else
            {
                pressF.SetActive(false);
            }
        }
    }
}
