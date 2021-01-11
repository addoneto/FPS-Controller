using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Footsteps : MonoBehaviour{
    
    public AudioSource audioSource;
    public float timeModifier = 0.5f;
    public float distanceOffset = 1f;
    public AudioClip[] stepClips;

    private CharacterController character;
    private FpsController controllerScript;
    private AudioClip previousClip;

    private float airTime;
    private float distanceCovered;

    private void Awake(){
        character = this.gameObject.GetComponent<CharacterController>();
        controllerScript = this.gameObject.GetComponent<FpsController>();
    }

    private void Update(){
        float currentSpeed = character.velocity.magnitude;
        if(currentSpeed != 0f){

            distanceCovered += currentSpeed * Time.deltaTime * timeModifier;
            if(distanceCovered > distanceOffset){
                TriggerClip();
                distanceCovered = 0f;
            }

        }

        FallingTrigger();

    }

    private AudioClip getRandomClip(){
        AudioClip selectedClip = stepClips[Random.Range(0, stepClips.Length - 1)];

        while(previousClip == selectedClip){
            selectedClip = stepClips[Random.Range(0, stepClips.Length - 1)];
        }

        previousClip = selectedClip;
        return selectedClip;
    }

    private void TriggerClip(){
        if(controllerScript.isGrounded){
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.volume = Random.Range(0.8f, 1f);

            audioSource.PlayOneShot(getRandomClip(), 1f);
        }
    }

    private void FallingTrigger(){
        if(!controllerScript.isGrounded){
            airTime += Time.deltaTime;
        }else{
            if(airTime > 0.25f){
                TriggerClip();
                airTime = 0f;
            }   
        }
    }
}