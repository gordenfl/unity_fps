using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField]
    public Transform[] cannibal_Points, boar_Points;
    public GameObject boar_Prefab, cannibal_Prefab;
    private int cannibal_count, boar_count;
    public int init_cannibal_count, init_boar_count;

    public float wait_before_spawn_time = 10.0f;

    void Awake()
    {
        MakeInstance();
    }
    void Start()
    {
        cannibal_count = init_cannibal_count;
        boar_count = init_boar_count;
        SpawnEnemies();
        StartCoroutine("CheckSpawnEnemies");
    }
    void SpawnEnemies()
    {
        SpawnCannibals();
        SpawnBoars();
    }
    public void SpawnOneCannibal(Entity entity)
    {
        float y = entity.position.y;
        if (entity.isOnGround)
        {
            y = 25.0f;
        }
        Vector3 newPos = new Vector3(entity.position.x, y, entity.position.z);
        entity.renderObj = Instantiate(cannibal_Prefab, newPos, Quaternion.Euler(new Vector3(entity.direction.y, entity.direction.z, entity.direction.x)));
        ((UnityEngine.GameObject)entity.renderObj).name = entity.className + "_" + entity.id;
    }
    public void SpawnOneBoar(Entity entity)
    {
        float y = entity.position.y;
        if (entity.isOnGround)
        {
            y = 25.0f;
        }
        Vector3 newPos = new Vector3(entity.position.x, y, entity.position.z);
        entity.renderObj = Instantiate(boar_Prefab, newPos, Quaternion.Euler(new Vector3(entity.direction.y, entity.direction.z, entity.direction.x)));
        ((UnityEngine.GameObject)entity.renderObj).name = entity.className + "_" + entity.id;
    }
    void SpawnCannibals()
    {
        for (int i = 0; i < cannibal_count; i++)
        {
            Instantiate(cannibal_Prefab, cannibal_Points[i % (cannibal_Points.Length)].position, Quaternion.identity);
        }
        cannibal_count = 0;
    }
    void SpawnBoars()
    {
        for (int i = 0; i < boar_count; i++)
        {
            Instantiate(boar_Prefab, boar_Points[i % (boar_Points.Length)].position, Quaternion.identity);
        }
        boar_count = 0;
    }

    IEnumerable CheckSpawnEnemies()
    {
        yield return new WaitForSeconds(wait_before_spawn_time);
        SpawnCannibals();
        SpawnBoars();
        StartCoroutine("CheckSpawnEnemies");
    }

    public void onCannibalDead()
    {
        cannibal_count += 1;
        if (cannibal_count >= init_cannibal_count)
        {
            SpawnCannibals();
        }
    }
    public void onBoarDead()
    {
        boar_count += 1;
        if (boar_count >= init_boar_count)
        {
            SpawnBoars();
        }
    }

    public void StopSpawning()
    {
        StopCoroutine("CheckSpawnEnemies");
    }
} //class
