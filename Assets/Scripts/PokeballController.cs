using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class PokeballController : MonoBehaviour
{
    private GameObject charmander;
    private GameObject terrian;
    private int animationStage;
    public Animator pokeballAnimator;
    private bool didOnce;
    public ParticleSystem pokeflashPF;
    private bool escaped;
    public Transform trainer;
    private bool checkForEscape;
    // Start is called before the first frame update
    void Start()
    {
        escaped = false;
        checkForEscape = true;
        pokeballAnimator.speed = 0;
        //get trainer position (cameraFcus is a child of the trainer)
        trainer = GameObject.Find("Trainer").transform;
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
                case 2: 
                    //Hang in thin air, rotate towards pokeball, open the pokeball, spawn a paticle on the pokeball,remove the charmander
                    rb.isKinematic = true; //Hang in thin air
                    Quaternion rotationTowardsCharmander = Quaternion.LookRotation(charmander.transform.position - transform.position);
                    //rotate the ball
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotationTowardsCharmander, Time.deltaTime * 3);
                    pokeballAnimator.speed = 4;
                    if (!didOnce) 
                    {
                        Instantiate(pokeflashPF, charmander.transform.position, Quaternion.identity);
                        didOnce = true;
                    }
                    charmander.SetActive(false);
                    if (pokeballAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f && pokeballAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN_Pokeball_Open")) 
                    {
                        animationStage = 3;
                    }                    
                    break;
                case 3: //Close the pokeball
                    pokeballAnimator.SetInteger("State",1);
                    if (pokeballAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f && pokeballAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN_Pokeball_Close")) 
                    {   
                        animationStage = 4;
                    }   
                    break;
                case 4: //Rotate towards player, and drop to the ground
                    transform.LookAt(trainer);
                    rb.isKinematic = false;
                    if (terrian != null) 
                    {
                        Debug.Log("Terrian is not null");
                        animationStage = 5;
                    }
                    break;
                case 5: //Stop phisics, Wiggle
                    rb.isKinematic = true;
                    pokeballAnimator.SetInteger("State",2);
                    pokeballAnimator.speed = 1.5f;
                    
                    if (checkForEscape) 
                    {
                        float r = Random.Range(1, 10);

                        if (r == 1) 
                        {
                            escaped = true;
                            pokeballAnimator.speed = 0;
                            didOnce = false;
                            animationStage = 6;
                        }
                        StartCoroutine(WaitForCheck(1));
                        checkForEscape = false;
                    }
                    
                    if (pokeballAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 3.0f && pokeballAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN_Pokeball_Wiggle")) 
                    {   
                        pokeballAnimator.speed = 0;
                        didOnce = false;
                        animationStage = 6;
                    }  
                    break;
                case 6: //Escape or not
                    Debug.Log(escaped);
                    if (escaped) 
                    {
                        //Instatiate only once 
                        if (!didOnce) 
                        {
                            Instantiate(pokeflashPF, charmander.transform.position, Quaternion.identity);
                            didOnce = true;
                        }
                        Destroy(gameObject);
                        charmander.SetActive(true);
                        
                    }
                    else 
                    {
                        Destroy(gameObject);    
                    }
                    break;
                    
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("charmander") && charmander == null) 
        {
            charmander = collision.gameObject;
        }
        if (collision.gameObject.name == "Terrain")
        {
            terrian = collision.gameObject;
        }
    }

    IEnumerator WaitForCheck(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        checkForEscape = true;
    }
}
