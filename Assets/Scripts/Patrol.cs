using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    int currentIndex = -1;

    AI npcScript;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
       : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROl;
        agent.speed = 2;
        agent.isStopped = false;
        npcScript = _npc.GetComponent<AI>();
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < GameEnviroment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWaypoint = GameEnviroment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWaypoint.transform.position);

            if (distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }

        Debug.Log("Change Animation");
        anim.SetTrigger("isWalking");
        base.Enter();
    }
    public override void Update()
    {
        //if (!GameEnviroment.Singleton.CanPatrol) return;

        if (agent.remainingDistance < 1)
        {
            if (!GameEnviroment.Singleton.CanRandomPatrol)
            {

                if (currentIndex >= GameEnviroment.Singleton.Checkpoints.Count - 1)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex++;
                }
                agent.SetDestination(GameEnviroment.Singleton.Checkpoints[currentIndex].transform.position);
            }
            else
            {

                agent.SetDestination(GameEnviroment.Singleton.Checkpoints[UnityEngine.Random.Range(0, GameEnviroment.Singleton.Checkpoints.Count)].transform.position);
            }
        }

        if (CanSeePlayer() && npcScript.CanAttack())
        {
            nextState = new Pursue(npc, agent, anim, player);

            npcScript.ChangeCurrentState(nextState);
            stage = EVENT.EXIT;
        }

        if (IsPlayerBehind() && npcScript.CanBeScared())
        {
            nextState = new RunAway(npc, agent, anim, player);
            npcScript.ChangeCurrentState(nextState);
            stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}
