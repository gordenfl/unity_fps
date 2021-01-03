using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerSprintAndCrouch : MonoBehaviour
{

    [SerializeField]
    public float sprint_Speed = 10.0f;
    [SerializeField]
    public float move_Speed = 5.0f;

    [SerializeField]
    public float crouch_Speed = 3.0f;

    private Transform look_Root;
    private float stand_Height = 1.6f;
    private PlayerMovement playerMovement;

    private float crouch_Height = 0.8f;

    private bool is_Crouching;

    private PlayerFootsteps player_footsteps;

    private float sprint_volume = 1.0f;
    private float crouch_volume = 0.1f;
    private float walk_volume_min = 0.2f, walk_volume_max = 0.6f;
    private float walk_step_distance = 0.4f;
    private float sprint_step_distance = 0.25f;
    private float crouch_step_distance = 0.5f;

    private PlayerStats playerStats;
    private float sprint_value = 100f;
    private float sprint_treshold = 10f;
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        player_footsteps = GetComponentInChildren<PlayerFootsteps>();
        look_Root = transform.GetChild(0);//get Look Root Child Object
        playerStats = GetComponent<PlayerStats>();
    }
    void Start()
    {
        player_footsteps.volume_min = walk_volume_min;
        player_footsteps.volume_max = walk_volume_max;
        player_footsteps.step_Distance = walk_step_distance;
    }

    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()//快跑
    {
        if (sprint_value > 0f)
        {
            //只在按下键最近的一帧调用
            if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching) 
            {
                playerMovement.speed = sprint_Speed;
                player_footsteps.step_Distance = sprint_step_distance;
                player_footsteps.volume_max = sprint_volume;
                player_footsteps.volume_min = sprint_volume;
                sprint_value -= sprint_treshold * Time.deltaTime;
                if (sprint_value < 0)
                {
                    sprint_value = 0.0f;
                }
            }
        }

        //只在抬起键最近的一帧调用
        if (Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching)
        {
            playerMovement.speed = move_Speed;
            player_footsteps.step_Distance = walk_step_distance;
            player_footsteps.volume_max = walk_volume_max;
            player_footsteps.volume_min = walk_volume_min;
        }

        //每一帧都会调用
        if (Input.GetKey(KeyCode.LeftShift) && !is_Crouching)
        {
            sprint_value -= sprint_treshold * Time.deltaTime;
            if (sprint_value < 0f)
            {
                sprint_value = 0.0f;
                playerMovement.speed = move_Speed;
                player_footsteps.step_Distance = walk_step_distance;
                player_footsteps.volume_max = walk_volume_max;
                player_footsteps.volume_min = walk_volume_min;
            }
        }
        else
        {
            if (sprint_value < 100f)
            {

                sprint_value += Time.deltaTime * (sprint_treshold / 2.0f);
                if (sprint_value > 100.0f)
                {
                    sprint_value = 100.0f;
                }
            }
        }
        playerStats.Display_StaminaStats(sprint_value);
    }

    void Crouch()//蹲下
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (is_Crouching)
            {
                //localPosition  means delta with it's parent 相对 位置
                look_Root.localPosition = new Vector3(0f, stand_Height, 0f);
                playerMovement.speed = move_Speed;
                player_footsteps.step_Distance = walk_step_distance;
                player_footsteps.volume_max = walk_volume_max;
                player_footsteps.volume_min = walk_volume_min;
                is_Crouching = false;
            }
            else
            {
                look_Root.localPosition = new Vector3(0f, crouch_Height, 0.0f);
                playerMovement.speed = crouch_Speed;
                player_footsteps.step_Distance = crouch_step_distance;
                player_footsteps.volume_max = crouch_volume;
                player_footsteps.volume_min = crouch_volume;
                is_Crouching = true;
            }
        }
    }
}