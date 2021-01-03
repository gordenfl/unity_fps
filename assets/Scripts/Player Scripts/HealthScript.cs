using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HealthScript : MonoBehaviour
{
    private EnemyAnimator enemy_anim;
    private NavMeshAgent navMeshAgent;
    private EnemyController enemy_controller;

    public float health = 100.0f;
    public bool is_player, is_boar, is_cannibal;

    private bool isDead;
    private EnemySound enemySound;

    private PlayerStats player_stats;

    void Awake()
    {
        if (is_boar || is_cannibal)
        {
            enemy_anim = GetComponent<EnemyAnimator>();
            enemy_controller = GetComponent<EnemyController>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            enemySound = GetComponentInChildren<EnemySound>();
        }
        if (is_player)
        {
            player_stats = GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
    }

    public void ApplyDamage(float val)
    {
        if (isDead)
            return;

        health -= val;
        if (is_player)
        {
            onPlayerDamage();
        }
        if (is_boar || is_cannibal)
        {
            onEnemyDamage();
        }
    }
    private void onPlayerDamage()
    {
        player_stats.Display_HealthStats(health);
        if (health <= 0)
        {
            onPlayerDie();
        }

    }
    void onPlayerDie()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyController>().enabled = false;
        }

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        isDead = true;
        Invoke("RestartGame", 3.0f);

    }
    private void onEnemyDamage()
    {
        if (enemy_controller.Enemy_State == EnemyState.PATROL)
        {
            enemy_controller.chase_Distance = 50.0f;
        }

        if (health <= 0)
        {
            onEnemyDie();
        }
    }
    private void onEnemyDie()
    {
        isDead = true;
        if (is_cannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 5.0f);


            enemy_controller.enabled = false;
            navMeshAgent.enabled = false;
            enemy_anim.enabled = false;
            EnemyManager.instance.onCannibalDead();

            //start coroutine
            StartCoroutine(DeadSound());
            //enemy manager spawn more enemies
            TurnOffGameObject();

        }
        else if (is_boar)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            enemy_controller.enabled = false;
            enemy_anim.Dead();
            EnemyManager.instance.onBoarDead();
            //start coroutine
            StartCoroutine(DeadSound());
            // enemy manager spawn more enemies
            Invoke("TurnOffGameObject", 3.0f);

        }


    }
    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    private void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemySound.play_DeadSound();
    }

}//class
