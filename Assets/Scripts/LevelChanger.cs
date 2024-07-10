using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{

    public GameObject[] levels;

    public int level;

    public FlockManager flockManager;
    void Start()
    {

    }

    public void GoToLevel(int _level)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        foreach (var level in levels)
        {
            level.SetActive(false);
        }

        levels[_level].SetActive(true);
        if (flockManager != null && _level == 2)
        {
            flockManager.TurnOnWaterCamera(true);
        }

    }

    public void GoToLevel()
    {
        foreach (var level in levels)
        {
            level.SetActive(false);
        }
        levels[level].SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            flockManager.TurnOnWaterCamera(false);
            GoToLevel(0);
        }
    }
}
