using System;
using UnityEngine;

public class PlayerSpirntAndCrouch : MonoBehaviour
{

    private PlayerMovement playerMovement;
    public float sprint_Speed = 10f;
    public float move_Speed = 5f;

    public float crouch_Speed = 2f;
    private Transform look_Root;
    private float stand_Height = 1.6f;
    private float crouch_Height = 1f;

    private bool is_Crouching;
    private PlayerFootsteps player_Footsteps;

    private float sprint_Volume = 1f;
    private float crouch_Volume = 0.1f;
    private float walk_Volume_min = 0.2f, walk_Volume_max = 0.6f;

    private float walk_Step_Distance = 0.4f;
    private float sprint_Step_Distance = 0.25f;
    private float crouch_Steep_Distance =0.5f ;




    private void Start()
    {
        player_Footsteps.volume_Min = walk_Volume_min;
        player_Footsteps.volume_Max = walk_Volume_max;
        player_Footsteps.step_Distance = walk_Step_Distance;

    }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        look_Root = transform.GetChild(0);

        player_Footsteps = GetComponentInChildren<PlayerFootsteps>();

    }
   

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching)
        {
            playerMovement.speed = sprint_Speed;
            player_Footsteps.step_Distance = sprint_Step_Distance;
            player_Footsteps.volume_Min = sprint_Volume;
            player_Footsteps.volume_Max = sprint_Volume;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching)
        {
            playerMovement.speed = move_Speed;

            player_Footsteps.step_Distance = walk_Step_Distance; 
            player_Footsteps.volume_Min = walk_Volume_min;
            player_Footsteps.volume_Max = walk_Volume_max;
           

        }



    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //if we are crouching - stand up
            if (is_Crouching)
            {
                look_Root.localPosition = new Vector3(0f, stand_Height, 0f);
                playerMovement.speed = move_Speed;

                is_Crouching = false;

            }
            else
            {
                //if we are not crouching - crouch
                 look_Root.localPosition = new Vector3(0f, crouch_Height , 0f);
                playerMovement.speed = crouch_Speed;

                player_Footsteps.step_Distance = crouch_Steep_Distance;

                player_Footsteps.step_Distance = walk_Step_Distance; 
                player_Footsteps.volume_Min = crouch_Volume;
                player_Footsteps.volume_Max = crouch_Volume;

                is_Crouching = false;

            }
        }
    }


}
