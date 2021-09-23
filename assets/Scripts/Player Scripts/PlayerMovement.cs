using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TagHolder;
using Photon;
using Photon.Bolt;

public class PlayerMovement : EntityBehaviour<IPlayerState>
{
    // Start is called before the first frame update

    private CharacterController character_controller = null;

    private Vector3 move_direction;
    public Camera camera;
    public Camera fp_camera;
    public float speed = 5f;
    private float gravity = 20f;
    public float jump_Force = 10.0f;
    private float vertical_Velocity;
    public bool _isPlayer = true;
    void Awake()
    {
        this.character_controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        //camera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }

    public override void Attached()
    {
        state.SetTransforms(state.transform, transform);
        this.setIsPlayer(false);
    }

    public void setCamera(bool state)
    {
        this.camera.enabled = state;
        //this.fp_camera.enabled = state;
        if(state == true)
        {
            Camera.SetupCurrent(this.camera);
        }
    }
    void ApplyGravity()
    {
        vertical_Velocity -= gravity * Time.deltaTime;
        PlayerJump();
        move_direction.y = vertical_Velocity * Time.deltaTime;
    }

    public void setIsPlayer(bool val)
    {
        this._isPlayer = val;

        if (this._isPlayer==true)
        {
            this.setCamera(true);
        }
        else
        {
            this.setCamera(false);
        }
    }
    void PlayerJump()
    {
        if (this._isPlayer == false) return;
        if (character_controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jump_Force;
        }

    }
    void MoveThePlayer()
    {
        if (this._isPlayer == false) return;
        float delta_x = Input.GetAxis(Axis.Horizontal);
        float delta_z = Input.GetAxis(Axis.Vertical);
        move_direction = new Vector3(delta_x, 0.0f, delta_z);
        move_direction = transform.TransformDirection(move_direction);
        move_direction *= speed * Time.deltaTime;
        ApplyGravity();
        this.character_controller.Move(move_direction);
    }

}
