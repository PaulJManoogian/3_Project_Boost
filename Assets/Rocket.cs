//using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//todo - Fix lighting bug

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float sceneSwitchWait = 1f;

    Rigidbody rigidBody;
    AudioSource audioSource;

  enum State { alive, dying, transcending }
    State state = State.alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        //todo somewhere stop sound on death
        if (state == State.alive)
        {
            Thrust();
            Rotate();
        }
	}


    private void OnCollisionEnter(Collision collision)
    {
      if (state != State.alive)
        {
            return; // ignore collisions when dead
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                   // print("Collided - Friendly");
                    break;
                }
       
            case "Finish":
                {
                    state = State.transcending;
                    print("Collided - Hit Finish");
                    Invoke("LoadNextScene",sceneSwitchWait); //param this time

                    break;
                }
            default:
                {
                    print("Collided - Dead");  // kill player
                    state = State.dying;
                    print("Collided - Hit Finish");
                    Invoke("LoadFirstScene", sceneSwitchWait); //param this time
                    break;
                }
        }

        
    }


    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0); //allow for more than two levels
    }


    private void LoadNextScene()
    {
        SceneManager.LoadScene(1); //allow for more than two levels
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
