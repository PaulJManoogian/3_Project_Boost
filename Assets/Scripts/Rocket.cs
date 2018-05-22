//using System;
//using System.Collections;
//using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//todo - Fix lighting bug

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float sceneSwitchWait = 1f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { alive, dying, transcending }
    State state = State.alive;

    bool collisionsDisabled = false;



	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {    
        if (state == State.alive)
        {
            RespondToThrust();
            RespondToRotation();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
	}


    //todo debug keys - remove?
    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // toggle collision
            collisionsDisabled = !collisionsDisabled;
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
      if (state != State.alive || collisionsDisabled) { return; }
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                   // print("Collided - Friendly");
                    break;
                }
       
            case "Finish":
                {
                    StartSuccessSequence();

                    break;
                }
            default:
                {
                    StartDeathSequence();
                    break;
                }
        }

        
    }
 

    private void StartSuccessSequence()
    {
        state = State.transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextScene", sceneSwitchWait); //param this time
    }


    private void StartDeathSequence()
    {
        state = State.dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstScene", sceneSwitchWait); //param this time
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0); //allow for more than two levels
    }


    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; // loop back to start
        }
        SceneManager.LoadScene(nextSceneIndex); //allow for more than two levels
    }


    private void RespondToThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }

    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying) // so it doesn't layer and buzz
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }



    private void RespondToRotation()
    {
        rigidBody.angularVelocity = Vector3.zero;
        //rigidBody.freezeRotation = true; // take manual control of rotation
        
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
