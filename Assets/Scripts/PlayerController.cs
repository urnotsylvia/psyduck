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

    [SerializeField]
    private GameObject pokeBall;

    [SerializeField]
    private Transform pokeBallSpawn;

    [SerializeField]
    private float throwStrength = 100f;

    private GameObject currentPokeBall;

    private Vector3 velocity;
    private float gravity = -9.8f;
    private float jumpHeightWithoutGravity = 10f;
    public float maxDistanceToJump;

    public float speed = 5f;
    public float runSpeed = 10f;

    public bool grounded;
    public bool throwing;

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

        //rotate the player
        if (movement.magnitude > 0.1f)
        {
            playerTransform.rotation = Quaternion.LookRotation(movement);
        }
        
        controller.Move(velocity * Time.deltaTime);

        //stop moving when throwing pokeball
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            throwing = true;
            trainerAnimator.SetBool("isThrowing", true);
            SpawnPokeballToBone();
        }
        
        if (!throwing)
        {
            //Control player speed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(movement * (runSpeed * Time.deltaTime));
                Debug.Log("Running");
                trainerAnimator.SetBool("isRunning", true);
                trainerAnimator.SetBool("isWalking", false);
            }
            else if (movement.magnitude > 0)
            {
                controller.Move(movement * (speed * Time.deltaTime)); 
                trainerAnimator.SetBool("isWalking", true);
                trainerAnimator.SetBool("isRunning", false);
            }
            else
            {
                trainerAnimator.SetBool("isWalking", false);
                trainerAnimator.SetBool("isRunning", false);
            }

            //Gravity and Jumping
            if (!grounded)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
                trainerAnimator.SetBool("isJumping", false);
            }
            
            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeightWithoutGravity);
                trainerAnimator.SetBool("isJumping", true);
            }
        }
    }

    public void ThrowEnded() 
    {
        throwing = false;
        trainerAnimator.SetBool("isThrowing", false);
        Debug.Log("ended");
    }

    public void SpawnPokeballToBone()
    {
        currentPokeBall = Instantiate(pokeBall, pokeBallSpawn, false);
    }

    public void ReleasePokeball()
    {
        if (currentPokeBall != null)
        {
            currentPokeBall.transform.parent = null;
            currentPokeBall.GetComponent<Rigidbody>().useGravity = true;
            currentPokeBall.GetComponent<SphereCollider>().enabled = true;
            Transform cameraTransform = mainCamera.transform;

            // throw pokeball up 45 degree
            Vector3 throwDirection = cameraTransform.forward + cameraTransform.up * 1.5f;
            currentPokeBall.GetComponent<Rigidbody>().AddForce(throwDirection * throwStrength);
            currentPokeBall = null;
        }
    }

    private void OnApplicationFocus(bool hasFocus) {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

