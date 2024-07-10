using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
       : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        //if (!GameEnviroment.Singleton.activateLevelOneAI || !GameEnviroment.Singleton.CanPatrol) return;
        if (CanSeePlayer() && npc.GetComponent<AI>().CanAttack())
        {
            nextState = new Pursue(npc, agent, anim, player);
            npc.GetComponent<AI>().ChangeCurrentState(nextState);
            //stage = EVENT.EXIT;
        }
        else
        {
            nextState = new Patrol(npc, agent, anim, player);
            npc.GetComponent<AI>().ChangeCurrentState(nextState);
            //stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
