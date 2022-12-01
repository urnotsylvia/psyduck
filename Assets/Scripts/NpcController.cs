using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{    
    public enum BehaviorState
    {
        Idle,
        Walk,
        //Dance,
        RunAway
    }    
    BehaviorState currentState;

    private Vector3 velocity;
    public Transform target;
    public float idleTime = 5f;
    public float walkTime = 5f;
    public float chaseDistance = 3f;
    public float safeDistance = 8f;

    [SerializeField] 
    private CharacterController controller;


    Animator anim;
    [SerializeField] 

    float distanceToPlayer;
    float counter;
    private float gravity = -9.8f;

    // Start is called before the first frame update
    void Start()
    {
        if(target == null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        anim = GetComponent<Animator>();
        currentState = BehaviorState.Walk;
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, target.position);
        
        switch(currentState)
        {
            case BehaviorState.Idle:
                Idle();
                break;
            case BehaviorState.Walk:
                Walk();
                break;
            // case BehaviorState.Dance:
            //     Dance();
            //     break;
            case BehaviorState.RunAway:
                RunAway();
                break;
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
    }

    void Idle() 
    {
        anim.SetInteger("animState", 1);
        counter += Time.deltaTime;
        if (counter > idleTime)
        {
            currentState = BehaviorState.Walk;
            counter = 0;
        }
        if (distanceToPlayer <= chaseDistance)
        {
            currentState = BehaviorState.RunAway;
        }
    }

    void Walk()
    {
        anim.SetInteger("animState", 2);

        // Rotate randomly 15 degrees
        transform.Rotate(0, Random.Range(-15, 15), 0);
        controller.Move(transform.forward * 2 * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        counter += Time.deltaTime;
        if (counter > walkTime) 
        {
            currentState = BehaviorState.Idle;
            counter = 0;
        }

        if (distanceToPlayer <= chaseDistance)
        {
            currentState = BehaviorState.RunAway;
        }

    }

    void Dance()
    {
        // anim.SetInteger("animState", 3);
        // if (distanceToPlayer <= chaseDistance)
        // {
        //     currentState = BehaviorState.RunAway;
        // }
    }

    void RunAway()
    {
        anim.SetInteger("animState", 4);
        
        // rotate to face away from player
        Vector3 direction = transform.position - target.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);

        controller.Move(transform.forward * 5 * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);


        if (distanceToPlayer > safeDistance)
        {
            currentState = BehaviorState.Idle;
        }
    }




}
