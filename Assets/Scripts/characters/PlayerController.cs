using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private CharacterStats characterStats;
    private GameObject attackTarget;
    private float lastAttackTime;
    private bool isDead; 

    private float stopDistance;
    // private bool isAttack;

    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>(); 
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        stopDistance = agent.stoppingDistance;
    }

    void Start() 
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;

        GameManager.Instance.RigisterPlayer(characterStats);
        //characterStats.MaxHealth = 2;
    }



    void Update() 
    {
        isDead = characterStats.CurrentHealth == 0;
        if(isDead)
            GameManager.Instance.NotifyObservers();

        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
        
        // if(isAttack&&lastAttackTime<=0&&(attackTarget!=null))
        // {
        //     StopAllCoroutines();
        //     StartCoroutine(MoveToAttackTarget());   
        // }
        
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDead);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if(isDead) return;
        // isAttack = false;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if(isDead) return;
        if(target != null)
        {
            attackTarget = target;
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());   
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        while(Vector3.Distance(attackTarget.transform.position,transform.position) > characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;
        //Attack

        if(lastAttackTime < 0)
        {
            anim.SetBool("Critical",characterStats.isCritical);
            anim.SetTrigger("Attack");
            //重置冷却时间
            lastAttackTime = characterStats.attackData.coolDown;
            // isAttack = true;
        }

    }

    //Animation Event
    void Hit()
    {
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats.TakeDamage(characterStats, targetStats);
    }

}
