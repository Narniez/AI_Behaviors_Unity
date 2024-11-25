using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    float rotationSpeed = 2.0f;
    AudioSource shoot;
    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) 
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
        //shoot = _npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        anim.SetTrigger("isShooting");
        agent.isStopped = true;
        //shoot.Play();
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        //So we rotate the npc around but not tilt it on x or z axis 
        direction.y = 0;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player);
            npc.GetComponent<AI>().ChangeCurrentState(nextState);
            //stage = EVENT.EXIT;
        }
        if (!npc.GetComponent<AI>().CanAttack())
        {
            nextState = new Patrol(npc, agent, anim, player);
            npc.GetComponent<AI>().ChangeCurrentState(nextState);
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");
        //shoot.Stop();
        base.Exit(); 
    }
}
