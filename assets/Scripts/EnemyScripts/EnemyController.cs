using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL, //巡逻
    CHASE, //追
    ATTACK //攻击
}
public class EnemyController : MonoBehaviour
{
    private EnemyState enemy_state;
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    public float run_speed = 3.0f;
    public float walk_speed = 0.5f;
    public float chase_Distance = 10.0f;
    public float current_Chase_Distance;
    public float attact_Distance = 1.8f;
    public float chase_after_attack_distance = 2.0f;

    public float patrol_Radius_min = 50.0f, patrol_Radius_max = 80.0f;
    public float patrol_For_This_Time = 15.0f;
    public float patrol_Timer;

    public float wait_Before_Attack = 2.0f;

    private float attack_Timer;
    private Transform target;

    public GameObject attack_point;

    private EnemySound enemySound;
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        if (!navAgent.isOnNavMesh)
        {
            //Vector3 warpPosition; //Set to position you want to warp to
            navAgent.transform.position = transform.position;
            navAgent.enabled = false;
            navAgent.enabled = true;
        }
        enemy_Anim = GetComponent<EnemyAnimator>();


        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
        enemySound = GetComponentInChildren<EnemySound>();

    }
    // Start is called before the first frame update
    void Start()
    {
        enemy_state = EnemyState.PATROL;
        patrol_Timer = patrol_For_This_Time;

        //enemy 第一次攻击是立即攻击
        attack_Timer = wait_Before_Attack;

        //memorize the value of chase distance
        //so that we can put it back
        current_Chase_Distance = chase_Distance;

    }//Start

    // Update is called once per frame
    void Update()
    {
        if (enemy_state == EnemyState.PATROL)
        {
            Patrol();
        }
        if (enemy_state == EnemyState.CHASE)
        {
            Chase();
        }
        if (enemy_state == EnemyState.ATTACK)
        {
            Attack();
        }
    }//update

    //巡逻
    void Patrol()
    {
        navAgent.isStopped = false;
        navAgent.speed = this.walk_speed;
        patrol_Timer += Time.deltaTime;
        if (patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0;
        }

        //print(navAgent.velocity.sqrMagnitude.ToString() + "AAAA" + this.walk_speed.ToString());
        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }

        //test the distance from player
        if (Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_state = EnemyState.CHASE;
            //Play spotted audio
            enemySound.play_ScreamSound();
        }
        else
        {
            enemy_state = EnemyState.PATROL;
        }
    }
    //追
    void Chase()
    {
        navAgent.isStopped = false;
        navAgent.speed = run_speed;
        //set the target position to navAgent's target, GOTO Line
        navAgent.SetDestination(target.position);
        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }

        if (Vector3.Distance(transform.position, target.position) <= attact_Distance)
        {
            //stop animations
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_state = EnemyState.ATTACK;
            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
        else if (Vector3.Distance(transform.position, target.position) > chase_Distance)
        {
            //enemy_Anim.Walk(true);
            enemy_Anim.Run(false);
            enemy_state = EnemyState.PATROL;
            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }



    }

    //攻击
    void Attack()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;
        attack_Timer += Time.deltaTime;
        if (attack_Timer > wait_Before_Attack)
        {
            enemy_Anim.Attack();
            attack_Timer = 0.0f;
            //play attack sound
            enemySound.play_AttackSound();
        }
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attact_Distance + chase_after_attack_distance)
        {
            enemy_state = EnemyState.CHASE;
        }
        else if (distance > attact_Distance)
        {
            enemy_state = EnemyState.PATROL;
        }
    }

    void SetNewRandomDestination()
    {
        float rand_radius = Random.Range(patrol_Radius_min, patrol_Radius_max);

        Vector3 randDir = Random.insideUnitSphere * rand_radius;
        randDir += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, rand_radius, -1);
        navAgent.SetDestination(navHit.position);
    }

    void Turn_On_AttackPoint()
    {
        attack_point.SetActive(true);
    }
    void Turn_Off_AttackPoint()
    {
        if (attack_point.activeInHierarchy)
        {
            attack_point.SetActive(false);
        }
    }

    public EnemyState Enemy_State
    {
        get
        {
            return enemy_state;
        }
        set
        {
            enemy_state = value;
        }
    }
} //class
