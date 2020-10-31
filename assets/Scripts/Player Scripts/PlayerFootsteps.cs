using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource footstep_sound;

    [SerializeField]
    private AudioClip[] footstep_Clip;

    private CharacterController character_controller;

    [HideInInspector]
    public float volume_min, volume_max;

    //走了多远，可以播放走路的声音了没有
    private float accumulated_Distance;

    //走了多远才可以播放走路的声音
    [HideInInspector]
    public float step_Distance;

    void Awake()
    {
        footstep_sound = GetComponent<AudioSource>();
        character_controller = GetComponentInParent<CharacterController>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckToPlayFootstepSound();
    }

    void CheckToPlayFootstepSound()
    {
        if (!character_controller.isGrounded)
            return;

        if (character_controller.velocity.sqrMagnitude <= 0)
        {
            accumulated_Distance = 0;
            return;
        }

        accumulated_Distance += Time.deltaTime;
        if (accumulated_Distance > step_Distance)
        {
            footstep_sound.volume = Random.Range(volume_min, volume_max);
            footstep_sound.clip = footstep_Clip[Random.Range(0, footstep_Clip.Length)];
            footstep_sound.Play();
            accumulated_Distance = 0;
        }

    }


}
