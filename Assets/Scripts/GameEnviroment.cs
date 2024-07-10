using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.Rendering;

public sealed class GameEnviroment
{
    private static GameEnviroment instance;

    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints {  get { return checkpoints; } }

    private Transform safePlace;
    public Transform SafePlace { get { return safePlace; } }
    public bool activateLevelOneAI { get;  set; }

    public bool CanPatrol {  get; set; }
    public bool CanAttack { get; set; }
    public bool CanRunAway { get; set; }
    public bool CanPursue { get; set; }

    public bool CanRandomPatrol { get; set; }
    public static GameEnviroment Singleton
    {
        get
        {
            if(instance == null)
            {
                instance = new GameEnviroment();
                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                instance.checkpoints = instance.Checkpoints.OrderBy(waypoint => waypoint.name).ToList();
                instance.safePlace = GameObject.FindGameObjectWithTag("Safe").transform;

            }
            return instance;
        }
    }
}
