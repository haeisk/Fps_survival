using System;
using UnityEngine;
using UnityEngine.AI;






public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{


    private EnemyAnimator enemyAnimator;
    private NavMeshAgent navMeshAgent;
    private EnemyState enemy_State;

    public float walk_Speed = 2f;
    public float run_Speed = 5f;
    public float chase_Range = 10f;
    private float current_Chase_Distance;
    public float attack_Range = 2f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_min = 20f;
     public float patrol_Radius_max = 60f;

    public float patrol_for_This_Time = 10f;
    public float patrol_Timer;

    public float wait_Before_Attack = 1f;
    private float attack_Timer;
    private Transform target;


    private void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        navMeshAgent = GetComponent<NavMeshAgent>();



        target = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy_State = EnemyState.PATROL;

        patrol_Timer = patrol_for_This_Time;

        attack_Timer = wait_Before_Attack;

        current_Chase_Distance = chase_Range;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy_State == EnemyState.PATROL)
        {
            Patrol(); 
        }

        if (enemy_State == EnemyState.CHASE)
        {
            Chase();
        }
        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }

    private void Attack()
    {
        
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = walk_Speed;

        patrol_Timer += Time.deltaTime;

        if (patrol_Timer >= patrol_for_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }
    }

    private void SetNewRandomDestination()
    {
        float rand_Radius = UnityEngine.Random.Range(patrol_Radius_min, patrol_Radius_max);
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * rand_Radius;
        Vector3 randomDirection = new Vector3(randomCircle.x, transform.position.y, randomCircle.y) + transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, rand_Radius, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }

    private void Patrol()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = walk_Speed;

        patrol_Timer += Time.deltaTime;

        if (patrol_Timer >= patrol_for_This_Time || navMeshAgent.remainingDistance < 0.5f)
        {
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }

        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            enemyAnimator.Walk(true);
        }
        else
        {
            enemyAnimator.Walk(false);
        }


        if (Vector3.Distance(transform.position, target.position) <= chase_Range)
        {
            enemy_State = EnemyState.CHASE;
            enemyAnimator.Walk(false);
          
            
        }

    }

    private void Chase()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = run_Speed;
        navMeshAgent.SetDestination(target.position);

        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            enemyAnimator.Run(true);
        }else
        {
            enemyAnimator.Run(false);
        }
    }

    



}
