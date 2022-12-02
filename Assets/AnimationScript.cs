using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    // player object
    public GameObject player;

    void Start()
    {
        //get player object from parent
        player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowEnded() 
    {
        // call ThrowEnded function in PlayerController script
        player.GetComponent<PlayerController>().ThrowEnded();
    }

    public void ReleaseBall() {
        // call ReleaseBall function in PlayerController script
        player.GetComponent<PlayerController>().ReleasePokeball();
    }
}
