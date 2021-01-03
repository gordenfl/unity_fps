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
    void Awake()
    {
        if (is_boar || is_cannibal)
        {
            enemy_anim = GetComponent<EnemyAnimator>();
            enemy_controller = GetComponent<EnemyController>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        if (is_player)
        {

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

        onPlayerDamage();
        onEnemyDamage();
        if (health <= 0)
        {
            isDead = true;
        }
    }
    private void onPlayerDamage()
    {
        if (!is_player)
        {
            return;
        }

        onPlayerDie();
    }
    void onPlayerDie()
    {
        if (health > 0)
        {
            return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyController>().enabled = false;
        }

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);

        if (tag == Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3.0f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3.0f);
        }

    }
    private void onEnemyDamage()
    {
        if (!is_boar && !is_cannibal)
        {
            return;
        }

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
        if (is_cannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 50.0f);
            GetComponent<BoxCollider>().isTrigger = false;

            enemy_controller.enabled = false;
            navMeshAgent.enabled = false;
            enemy_anim.enabled = false;

            //start coroutine

            //enemy manager spawn more enemies
        }
        else if (is_boar)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            enemy_controller.enabled = false;
            enemy_anim.Dead();

            //startcoroutine

            // enemy manager spawn more enemies
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

}//class
