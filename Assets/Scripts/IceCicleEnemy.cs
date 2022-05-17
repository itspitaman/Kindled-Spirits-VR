using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class IceCicleEnemy : MonoBehaviour
{
    public enum IcicleState { Wander, Chase, Attack, Damaged, Dead };
    public IcicleState currentState;

    Health health;
    FieldOfView fov;
    Health currentTarget;
    NavMeshAgent navAgent;

    public float chaseGiveUpTime;
    float chaseTimer;

    public float wanderSpeed;
    public float chaseSpeed;
    public float attackRange;
    public float attackSpeed;
    float attackTimer;

    //Animation Variables
    Animator animator;
    public float stunDelay;

    // Melee hitbox
    public GameObject meleeAttackHitBox;
    BoxCollider hitBoxCollider;
    public float hitBoxActivationDelay;
    public float hitBoxDeactivationTime;

    // Start is called before the first frame update
    void Start()
    {
        hitBoxCollider = meleeAttackHitBox.GetComponent<BoxCollider>();
        hitBoxCollider.enabled = false;
        health = GetComponent<Health>();
        currentState = IcicleState.Wander;
        currentTarget = null;
        fov = GetComponentInChildren<FieldOfView>();
        navAgent = GetComponent<NavMeshAgent>();
        attackTimer = 0;
        animator = GetComponent<Animator>();
    }

    bool CanSeeAttackableHealth()
    {
        if (fov.visibleHealths.Count > 0)
        {
            foreach (Health seenHealth in fov.visibleHealths)
            {
                if (!seenHealth.isEnemyGolem && !seenHealth.isEnemyIcicle && !seenHealth.isDead)
                {
                    currentTarget = seenHealth;
                    currentState = IcicleState.Chase;
                    return true;
                }
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        // Force death animation if Health = 0
        if (health.currentHitPoints <= 0)
        {
            Die();
        }

        if (currentState == IcicleState.Wander)
        {
            Wander();
        }
        else if (currentState == IcicleState.Chase)
        {
            Chase();
        }
        else if (currentState == IcicleState.Attack)
        {
            Attack();
        }
        else if (currentState == IcicleState.Damaged)
        {
            TookDamage();
        }
        else if (currentState == IcicleState.Dead)
        {
            Die();
        }
    }

    void Wander()
    {
        if (CanSeeAttackableHealth())
        {
            return;
        }

        navAgent.speed = wanderSpeed;

        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            Vector3 randomDirection = transform.position + Random.insideUnitSphere * 5;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 5, 1);
            navAgent.SetDestination(hit.position);
        }

        //Animation of Animator
        animator.SetBool("IsWandering", true);
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsDamaged", false);
    }

    void Chase()
    {
        //Animation of Animator
        animator.SetBool("IsWandering", false);
        animator.SetBool("IsChasing", true);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsDamaged", false);

        if (CanSeeAttackableHealth())
        {
            chaseTimer = chaseGiveUpTime;
        }
        else
        {
            chaseTimer -= Time.deltaTime;

            if (chaseTimer <= 0)
            {
                currentState = IcicleState.Wander;
                return;
            }
        }

        if (navAgent.remainingDistance <= attackRange)
        {
            currentState = IcicleState.Attack;
            return;
        }

        navAgent.SetDestination(currentTarget.transform.position);
        navAgent.speed = chaseSpeed;
    }

    void Attack()
    {
        Vector3 distanceToTarget = (transform.position - currentTarget.transform.position);
        if (distanceToTarget.magnitude > attackRange)
        {
            navAgent.SetDestination(currentTarget.transform.position);
            currentState = IcicleState.Chase;
            return;
        }

        navAgent.SetDestination(transform.position);
        Vector3 adjustedForHeight = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);
        transform.LookAt(adjustedForHeight);

        if (currentTarget.isDead || currentTarget == null)
        {
            currentState = IcicleState.Wander;
            return;
        }

        // we can actually throw a snowball at a live enemy
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            //Animation of Animator
            animator.SetBool("IsWandering", false);
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsDead", false);
            animator.SetBool("IsDamaged", false);

            attackTimer = attackSpeed;
            StartCoroutine(HitBoxActiveTime());
        }
    }

    void TookDamage()
    {
        animator.SetBool("IsWandering", false);
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsDamaged", true);

        StartCoroutine(StunDelay());
    }

    void Die()
    {
        navAgent.speed = 0;

        animator.SetBool("IsWandering", false);
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDamaged", false);
        animator.SetBool("IsDead", true);
    }

    IEnumerator StunDelay()
    {
        yield return new WaitForSeconds(stunDelay);

        if (!CanSeeAttackableHealth())
        {
            currentState = IcicleState.Wander;
        }
    }

    IEnumerator HitBoxActiveTime()
    {
        yield return new WaitForSeconds(hitBoxActivationDelay);
        hitBoxCollider.enabled = true;
        yield return new WaitForSeconds(hitBoxDeactivationTime);
        hitBoxCollider.enabled = false;
    }
}
