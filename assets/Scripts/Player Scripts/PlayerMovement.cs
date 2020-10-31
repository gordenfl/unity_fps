using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TagHolder;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    private CharacterController character_controller = null;

    private Vector3 move_direction;

    public float speed = 5f;
    private float gravity = 20f;
    public float jump_Force = 10.0f;
    private float vertical_Velocity;

    void Awake()
    {
        this.character_controller = GetComponent<CharacterController>();

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }

    void ApplyGravity()
    {

        vertical_Velocity -= gravity * Time.deltaTime;
        PlayerJump();
        move_direction.y = vertical_Velocity * Time.deltaTime;
    }

    void PlayerJump()
    {
        if (character_controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jump_Force;
        }

    }
    void MoveThePlayer()
    {
        float delta_x = Input.GetAxis(Axis.Horizontal);
        float delta_z = Input.GetAxis(Axis.Vertical);
        move_direction = new Vector3(delta_x, 0.0f, delta_z);
        move_direction = transform.TransformDirection(move_direction);
        move_direction *= speed * Time.deltaTime;
        ApplyGravity();
        this.character_controller.Move(move_direction);
    }
}
