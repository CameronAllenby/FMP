using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health = 30;

    private Animator anim;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }

        if  (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            anim.SetBool("Attack", true);
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        {
            agent.SetDestination(walkPoint);
            anim.SetBool("idle", true);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1)
        {
            walkPointSet = true;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
 

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = false;
        }
    }
    private void ChasePlayer()
    {
        anim.SetBool("idle", false);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        anim.SetBool("Attack", true);
        
        //Attack code goes here


        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        anim.SetBool("Attack", false);
    }

    public void TakeDamage(int damage)
    {
        health -=  damage;
        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 2);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            health--;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}
