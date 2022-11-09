using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] 
    [Tooltip("Insert character controller")]
    private CharacterController controller;

    private Vector3 velocity;
    private float gravity = -9.8f;
    private float jumpHeightWithoutGravity = 2f;
    public float speed = 5f;
    public float maxDistanceToJump;

    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Transform playerTransform = transform;
        grounded = Physics.Raycast(playerTransform.position, Vector3.down, maxDistanceToJump);
        

        Vector3 movement = (playerTransform.right * x) + (playerTransform.forward * z);
        controller.Move(movement * (speed * Time.deltaTime));        
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

