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
    public float run_speed = 4.0f;
    public float walk_speed = 0.5f;
    public float chase_Distance = 7.0f;
    public float current_Chase_Distance;
    public float attact_Distance = 1.8f;
    public float chase_after_attack_distakce = 2.0f;

    public float patrol_Radius_min = 20.0f, patrol_Radius_max = 60.0f;
    public float patrol_For_This_Time = 15.0f;
    public float patrol_Timer;

    public float wait_Before_Attack = 2.0f;

    private float attack_Timer;
    private Transform target;
    void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

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
        patrol_Timer+=Time.deltaTime;
        if(patrol_Timer>patrol_For_This_Time) {
            SetNewRandomDestination();
            patrol_Timer = 0;
        }
    }
    //追
    void Chase()
    {

    }

    //攻击
    void Attack()
    {

    }

    void SetNewRandomDestination() {
        float rand_radius = Random.Range(patrol_Radius_min, patrol_Radius_max);

        Vector3 randDir = Random.insideUnitSphere*rand_radius;
        randDir+=transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, rand_radius, -1);
        navAgent.SetDestination(navHit.position);
    }
} //class
