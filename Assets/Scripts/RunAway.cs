using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAway : State
{
    private Transform safePlace;
    public RunAway(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.RUNAWAY;
      
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        agent.speed = 5;
        agent.isStopped = false;
        agent.SetDestination(GameEnviroment.Singleton.SafePlace.transform.position);
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            nextState = new Idle(npc, agent, anim, player);
            npc.GetComponent<AI>().ChangeCurrentState(nextState);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }

}
