using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsController : MonoBehaviour{

    [SerializeField] private CharacterController controller;

    [SerializeField] private float smoothInputAmount = 10f;

    [SerializeField] private float walkSpeed = 10f;
    [Range(1f,2f)][SerializeField] private float runMultiplaier = 1.5f;
    [Range(0f,1f)][SerializeField] private float backwardsSpeedPercent = 0.5f;
    [Range(0f,1f)][SerializeField] private float sideSpeedPercent = 0.75f;

    [SerializeField] private float jumpForce = 10f;

    private Vector2 smoothMoveInput = Vector2.zero;
    [SerializeField] private float currentVelocity;

    [SerializeField] private float airSmoothDecayAmount = 3f;

    [SerializeField] private Transform groundCheckTranform;
    [SerializeField] private float gravity = -10f;
    private bool isGrounded = false;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float stickToGroundForce = -5f;
    [Range(0.01f,0.5f)][SerializeField] private float raySphereRadius = 0.1f;

    [SerializeField] private Vector2 moveInput = Vector2.zero;
    private float yVel = 0f;

    [SerializeField] private GameObject camera;
    private bool isRunning = false;

    [SerializeField] private Animator CharacterAnimator;

    //  TODO: SMOOTH THE TRANSITION BETWEEN SPEEDS
    //  TODO: HEAD BOB

    //private float maxBackwardSpeed, maxFowardSpeed, minSideSpeed, maxSideSpeed = 0f;
    private Vector2 resultInput;

    private void Awake(){
        currentVelocity = walkSpeed;
        controller = this.GetComponent<CharacterController>();

        // maxBackwardSpeed = walkSpeed * backwardsSpeedPercent *
        // maxFowardSpeed = maxSideSpeed * runMultiplaier;
        // minSideSpeed =
        // maxSideSpeed = 
    }

    private void Update(){

        //  GRAVITY
        isGrounded = Physics.CheckSphere(groundCheckTranform.position, raySphereRadius, groundLayer);

        if(isGrounded && yVel < 0){
            yVel = stickToGroundForce;
        }

        // HANDLE INPUTS
        if(isGrounded){
            moveInput = new Vector2(
                Input.GetAxisRaw("Horizontal"), 
                Input.GetAxisRaw("Vertical")  
            );

            moveInput.Normalize();
        }else{
            moveInput = Vector2.Lerp(moveInput, Vector2.zero, Time.deltaTime * airSmoothDecayAmount);
        }

        smoothMoveInput = Vector2.Lerp(smoothMoveInput, moveInput, Time.deltaTime * smoothInputAmount); 

        // HANDLE VELOCITIES
        if(moveInput.y == 1){
            currentVelocity = walkSpeed;
        }else if(moveInput.y == -1){
            currentVelocity = walkSpeed * backwardsSpeedPercent;
        }else if(moveInput.x > 0.5 ||  moveInput.x < -0.5 && moveInput.y == 0 ){
            currentVelocity = walkSpeed * sideSpeedPercent;
        }

        //  RUNNING
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            isRunning = true;
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            isRunning = false;
        }

        // if(isRunning && moveInput != Vector2.zero && currentVelocity < currentVelocity * runMultiplaier){
        //     currentVelocity = currentVelocity * runMultiplaier;
        // }

        //  JUMPING
        if(Input.GetButtonDown("Jump") && isGrounded){
            yVel = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        //  RELATIVE MOVIMENT
        Vector3 moveDirection = transform.right * smoothMoveInput.x + transform.forward * smoothMoveInput.y;

        //  APPLY GRAVITY
        yVel += gravity * Time.deltaTime;
        //moveDirection.y += yVel * Time.deltaTime;

        //  APPLY MOVIMENT
        if(isRunning)
            controller.Move(moveDirection * currentVelocity * runMultiplaier * Time.deltaTime);
        else
            controller.Move(moveDirection * currentVelocity * Time.deltaTime);
        controller.Move(new Vector3(0f, yVel, 0f) * Time.deltaTime);

        //  APPLY RUNNING FOV
        if(moveInput.y != 0f)
            camera.GetComponent<FpsCamera>().HandleZoom(isRunning, moveInput.y == 1);

        //  APPLY SWAY
        camera.GetComponent<FpsCamera>().HandleSway(moveInput.x);

        //  ANIMATION
        if(isRunning)
            resultInput = new Vector2(smoothMoveInput.x * 2, smoothMoveInput.y * 2);
        else
            resultInput = smoothMoveInput;

        CharacterAnimator.SetFloat("Xspeed", scale(-1, 1, -2f, 2f, resultInput.x) );
        CharacterAnimator.SetFloat("Yspeed", scale(-1, 1, -2f, 2f, resultInput.y) );

    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){
 
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
 
        return(NewValue);
    }
}
