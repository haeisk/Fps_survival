using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController character_Controller;
    private Vector3 move_Direction;
    public float move_Speed = 5f;
    public float speed = 5f;

    public float jump_Force = 20f;
    private float gravity = 20f;
    public float vertical_Velocity;

    private void Awake()
    {
        character_Controller = GetComponent<CharacterController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        move_Direction = new Vector3(Input.GetAxis(Axis.HORIZANTAL), 0f,
                                     Input.GetAxis(Axis.VERTICAL));

        move_Direction = transform.TransformDirection(move_Direction);

        move_Direction *= move_Speed;

        ApplyGravity();

        // Apply vertical velocity to y axis
        move_Direction.y = vertical_Velocity;

        character_Controller.Move(move_Direction * Time.deltaTime);
    }


    void ApplyGravity()
    {
        if (character_Controller.isGrounded)
        {
            if (vertical_Velocity < 0f)
                vertical_Velocity = -2f; // Small negative value to keep grounded

            PlayerJump();
        }
        else
        {
            vertical_Velocity -= gravity * Time.deltaTime;
        }
    }

    void PlayerJump()
    {
        if (character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jump_Force;    
            
        }
    }        

}
