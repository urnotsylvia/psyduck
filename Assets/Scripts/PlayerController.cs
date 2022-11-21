using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] 
    [Tooltip("Insert character controller")]
    private CharacterController controller;

    [SerializeField] 
    [Tooltip("Insert camera")]
    private Camera mainCamera;

    [SerializeField] 
    [Tooltip("Insert animator controller")]
    private Animator trainerAnimator;


    private Vector3 velocity;
    private float gravity = -9.8f;
    private float jumpHeightWithoutGravity = 10f;
    public float maxDistanceToJump;

    public float speed = 5f;
    public float runSpeed = 10f;

    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Grabs the transforms
        Transform playerTransform = transform;
        Transform cameraTransform = mainCamera.transform;

        //Ground Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        grounded = Physics.Raycast(playerTransform.position, Vector3.down, maxDistanceToJump);
        
        //Rotate along the camera
        playerTransform.rotation = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y,Vector3.up);

        Vector3 movement = (playerTransform.right * x) + (playerTransform.forward * z);

        //Control player speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(movement * (runSpeed * Time.deltaTime));
        }
        else
        {
            controller.Move(movement * (speed * Time.deltaTime));   
        }
             
        //Gravity and Jumping
        if (!grounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
        }
        
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeightWithoutGravity);
        }
        controller.Move(velocity * Time.deltaTime);
    }
}

