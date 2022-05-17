using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class SnowFolk : MonoBehaviour
{
    public enum SnowfolkState { Wander, Chase, Attack, Damaged, Dead };
    public SnowfolkState currentState;

    Health health;
    FieldOfView fov;
    Health currentTarget;
    NavMeshAgent navAgent;

    //Animation Variables
    Animator animator;
    public float stunDelay;

    public float chaseGiveUpTime;
    float chaseTimer;
    public float wanderSpeed;
    public float chaseSpeed;
    public float attackRange;
    public float attackSpeed;
    float attackTimer;
    public float snowballArcHeight = 2f;

    public GameObject snowballPrefab;
    public Transform snowballHoldingPoint;
   
    Rigidbody snowballRigidbody;
    Projectile heldSnowball;

    //public AudioSource source;
    //public AudioClip clip1;
    //public AudioClip clip2;
    // Start is called before the first frame update
    void Start()
    {
        
        health = GetComponent<Health>();
        heldSnowball = null;
        currentState = SnowfolkState.Wander;
        currentTarget = null;
        fov = GetComponentInChildren<FieldOfView>();
        navAgent = GetComponent<NavMeshAgent>();
        SpawnSnowball();
        attackTimer = 0;
        animator = GetComponent<Animator>();

        //AudioSource[] audioSources = GetComponents<AudioSource>();
        //clip1 = audioSources[1].clip;
        //clip2 = audioSources[2].clip;
    }

    bool CanSeeAttackableHealth()
    {
        if(fov.visibleHealths.Count > 0)
        {
            foreach(Health seenHealth in fov.visibleHealths)
            {
                if(!seenHealth.isEnemyGolem && !seenHealth.isEnemyIcicle && !seenHealth.isDead)
                {
                    currentTarget = seenHealth;
                    currentState = SnowfolkState.Chase;
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

        if (currentState == SnowfolkState.Wander)
        {
            Wander();
        }
        else if(currentState == SnowfolkState.Chase)
        {
            Chase();
        }
        else if(currentState == SnowfolkState.Attack)
        {
            Attack();
        }
        else if (currentState == SnowfolkState.Damaged)
        {
            TookDamage();
        }
        else if (currentState == SnowfolkState.Dead)
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

        if(navAgent.remainingDistance <= navAgent.stoppingDistance)
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

            if(chaseTimer <= 0)
            {
                currentState = SnowfolkState.Wander;
                return;
            }
        }

        if(navAgent.remainingDistance <= attackRange)
        {
            currentState = SnowfolkState.Attack;
            return;
        }

        navAgent.SetDestination(currentTarget.transform.position);
        navAgent.speed = chaseSpeed;
    }

    void Attack()
    {
        Vector3 distanceToTarget = (transform.position - currentTarget.transform.position);
        if(distanceToTarget.magnitude > attackRange)
        {
            navAgent.SetDestination(currentTarget.transform.position);
            currentState = SnowfolkState.Chase;
            return;
        }

        navAgent.SetDestination(transform.position);
        Vector3 adjustedForHeight = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);
        transform.LookAt(adjustedForHeight);

        if(currentTarget.isDead || currentTarget == null)
        {
            currentState = SnowfolkState.Wander;
            return;
        }

        // we can actually throw a snowball at a live enemy
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0)
        {
            //Animation of Animator
            animator.SetBool("IsWandering", false);
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsDead", false);
            animator.SetBool("IsDamaged", false);

            ThrowSnowball();
            attackTimer = attackSpeed;
        }
    }

    void TookDamage()
    {
        
        animator.SetBool("IsWandering", false);
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsDamaged", true);
        //source.PlayOneShot(clip1);

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
        //source.PlayOneShot(clip2);
    }

    void SpawnSnowball()
    {
        GameObject spawnedSnowball = Instantiate(snowballPrefab, snowballHoldingPoint);
        heldSnowball = spawnedSnowball.GetComponent<Projectile>();
        snowballRigidbody = spawnedSnowball.GetComponent<Rigidbody>();
        snowballRigidbody.useGravity = false;
        snowballRigidbody.isKinematic = true;
        
        spawnedSnowball.GetComponent<SphereCollider>().enabled = false;
        //spawnedSnowball.GetComponent<MeshRenderer>().enabled = false;
    }

    void ThrowSnowball()
    {
        //GameObject spawnedSnowball = snowballPrefab;
        heldSnowball.transform.parent = null;
        snowballRigidbody.useGravity = true;
        snowballRigidbody.isKinematic = false;
        heldSnowball.GetComponent<SphereCollider>().enabled = true;
        snowballRigidbody.AddForce(CalculateLaunchVelocity(), ForceMode.VelocityChange);
        StartCoroutine(SpawnSnowballAfterSeconds());
        StartCoroutine(DestroyAfterSeconds(heldSnowball.gameObject));
        //spawnedSnowball.GetComponent<MeshRenderer>().enabled = true;
    }

    Vector3 CalculateLaunchVelocity()
    {
        float displacementY = currentTarget.transform.position.y - snowballRigidbody.position.y;
        Vector3 displacementXZ = new Vector3(currentTarget.transform.position.x - snowballRigidbody.position.x, 0, currentTarget.transform.position.z - snowballRigidbody.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * snowballArcHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * snowballArcHeight / Physics.gravity.y)
          + Mathf.Sqrt(2 * (displacementY - snowballArcHeight) / Physics.gravity.y));

        return velocityXZ + velocityY;
    }

    IEnumerator SpawnSnowballAfterSeconds()
    {
        yield return new WaitForSeconds(attackSpeed / 2);
        SpawnSnowball();
    }

    IEnumerator DestroyAfterSeconds(GameObject destroyThis)
    {
        yield return new WaitForSeconds(6f);
        if (destroyThis != null)
        {
            Destroy(destroyThis);
        }
    }

    IEnumerator StunDelay()
    {
        yield return new WaitForSeconds(stunDelay);

        if (!CanSeeAttackableHealth())
        {
            currentState = SnowfolkState.Wander;
        }
    }
}
