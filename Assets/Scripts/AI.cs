using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public Transform player;
    State currentState;

    [SerializeField] private bool canPatrol = false;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private bool canRunAway = false;
    [SerializeField] private bool canPursue;
    [SerializeField] private bool canRandomPatrol = false;
    [SerializeField] private bool showExtraInfo = false;





    // Start is called before the first frame update
    void Start()
    {
        GameEnviroment.Singleton.CanRandomPatrol = canRandomPatrol;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

    }

    public void ChangeCurrentState(State state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
    public bool CanBeScared()
    {
        return canRunAway;
    }

    public bool CanPatrol()
    {
        return canPatrol;
    }

    public bool CanRandomPatrol()
    {
        return canRandomPatrol;
    }

    public bool CanAttack()
    {
        return canAttack;
    }

    public bool CanShowExtraInfo()
    {
        return showExtraInfo;
    }

    void TurnOnRandomPatrol()
    {
        canRandomPatrol = !canRandomPatrol;
        GameEnviroment.Singleton.CanRandomPatrol = canRandomPatrol;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            canPatrol = true;
            Debug.Log("Change State");
            currentState = new Patrol(this.gameObject, agent, anim, player);
            currentState.Enter();

        }

        if(Input.GetKeyDown(KeyCode.F2))
        {
            canAttack = !canAttack;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
           TurnOnRandomPatrol();
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            canRunAway = !canRunAway;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            showExtraInfo = !showExtraInfo;
        }

        if (currentState != null)
        {

            currentState.Update();
        }
    }
}
