using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
       
        Rotate();
	}

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        if (Input.GetKey(KeyCode.Space))
            Thrust();

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }

        rigidBody.freezeRotation = false; // physics control of rotation
    }

    private void Thrust()
    {
        rigidBody.AddRelativeForce(Vector3.up);
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
