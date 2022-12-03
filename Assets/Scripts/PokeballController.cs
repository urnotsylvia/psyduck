using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeballController : MonoBehaviour
{
    private GameObject charmander;
    private int animationStage;
    public Animator pokeballAnimator;
    // Start is called before the first frame update
    void Start()
    {
        pokeballAnimator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (charmander != null) 
        {
            switch (animationStage) 
            {
                case 0: //Apply upward force from the charmander we just hit
                    rb.AddForce(Vector3.up * 4, ForceMode.Impulse);
                    animationStage = 1;
                    break;
                case 1: //Check for when the pokeball is coming down again
                    if (rb.velocity.y < 0) 
                    {
                        animationStage = 2;
                    }
                    break;
                case 2: //Hang in thin air, rotate towards pokeball, open the pokeball, spawn a paticle on the pokeball,remove the charmander
                    rb.isKinematic = true; //Hang in thin air
                    Quaternion rotationTowardsCharmander = Quaternion.LookRotation(charmander.transform.position - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotationTowardsCharmander, Time.deltaTime * 2);
                    pokeballAnimator.speed = 4;
                    //animationStage = 3;
                    break;
                case 3: //Close the pokeball
                    animationStage = 4;
                    break;
                case 4: //Rotate towards player, and drop to the ground
                    animationStage = 5;
                    break;
                case 5: //Stop phisics, Wiggle
                    animationStage = 6;
                    break;
                case 6: //Escape or not
                    break;
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("charmander") && charmander == null) 
        {
            charmander = collision.gameObject;
        }
    }
}
