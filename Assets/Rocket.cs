using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        Thrust();
        Rotate();
	}


    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                {
                    print("Collided - Friendly");
                    break;
                }
       
            case "Fuel":
                {
                    print("Collided - Fuel");
                    break;
                }
            default:
                {
                    print("Collided - Dead");  // kill player
                    break;
                }
        }

        
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying) // so it doesn't layer and buzz
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }


    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        
        float rotationThisFrame = rcsThrust * Time.deltaTime; //Adjust rotation based on frame rate.      

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // physics control of rotation
    }

   
}
