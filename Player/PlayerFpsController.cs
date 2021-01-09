using UnityEngine;

public class PlayerFpsController : MonoBehaviour{

    [Header("Controller")]
    [SerializeField] private CharacterController controller;

    [Header("Moviment Settings")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float jumpHeight = 10f;
    [Range(0.1f,1f)][SerializeField] private float airControllAmount = 0.5f;

    [Space(10)]

    [SerializeField] private float runMultiplaier = 1.5f;
    [SerializeField] private float backwardsSpeedPercent = 0.5f;
    [SerializeField] private float sideSpeedPercent = 0.75f;

    [Header("Moviment Smoothness")]

    //  less amount more smooth
    [SerializeField] private float smoothInputAmount = 8f;
    [SerializeField] private float smoothVelocityTransition = 8f;
    [SerializeField] private float airVelocityDecay = 3f;

    //  REDUCE GLOBAL VARIABLES ??
    private Vector2 moveInput;
    private Vector2 smoothMoveInput;

    private float desiredVelocity;
    private float currentVelocity;

    private float yVelocity;
    private bool isRunning;     //  NECESSARY ??

    [Header("Gravity Settings")]
    [SerializeField] private Transform groundCheckTranform;
    private bool isGrounded;

    [SerializeField] private float gravity = -10f;
    [SerializeField] private float stickToGroundForce = -5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raySphereRadius = 0.1f;

    [SerializeField] private bool drawSphereGizmos = false;

    private float yVel = 0f;

    [Header("Camera Settings")]
    [SerializeField] private GameObject camera;
    private FpsCamera cameraScript;

    [Header("Character Animator")]
    [SerializeField] private Animator characterAnimator;

    private void Awake(){
        controller = this.GetComponent<CharacterController>();
        cameraScript = camera.GetComponent<FpsCamera>();
    }

    private void Update(){
        GetInput();
        CheckGravity();
        HandleVelocities();
        ApplyMoviment();
        ApplyAnimation();

        //TODO: Handle Air Moviment
        //          The velocity of the player manteins the same
        //          But it's input (to change the direction) is reduced
        //TODO: Head Bob
    }

    private void GetInput(){
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")
        );

        moveInput.Normalize();

        smoothMoveInput = Vector2.Lerp(smoothMoveInput, moveInput, Time.deltaTime * smoothInputAmount);
    }

    private void CheckGravity(){
        isGrounded = Physics.CheckSphere(groundCheckTranform.position, raySphereRadius, groundLayer);
    
        if(isGrounded && yVel < 0){
            yVel = stickToGroundForce;
        }

        //  JUMPING
        if(Input.GetButtonDown("Jump") && isGrounded){
            //  y velocity = square root of (h * -2f * g)
            yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //  APPLY GRAVITY
        yVel += gravity * Time.deltaTime;
    }

    private void HandleVelocities(){
        if(moveInput.y == 1){
            desiredVelocity = walkSpeed;
        }else if(moveInput.x != 0){
            desiredVelocity = walkSpeed * sideSpeedPercent;
        }else if(moveInput.y == -1){
            desiredVelocity = walkSpeed * backwardsSpeedPercent;
        }

        //  TODO : Reduce the velocity when changing to oposit moviment
        currentVelocity = Mathf.Lerp(currentVelocity, desiredVelocity, Time.deltaTime * smoothVelocityTransition);
    
        if(!isGrounded){
            smoothMoveInput = new Vector2(smoothMoveInput.x * airControllAmount, smoothMoveInput.y * airControllAmount);
        }

        //  RUNNING
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            currentVelocity *= runMultiplaier;
            isRunning = true;
        }else{
            isRunning = false;
        }
    }

    private void OnDrawGizmosSelected(){
        if(!drawSphereGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheckTranform.position, raySphereRadius);
    }

    private void ApplyMoviment(){
        //  Relative Move
        Vector3 moveDirection = transform.right * smoothMoveInput.x + transform.forward * smoothMoveInput.y;
    
        controller.Move(new Vector3(0f, yVel, 0f) * Time.deltaTime);
        controller.Move(moveDirection * currentVelocity * Time.deltaTime);
    }

    private void ApplyAnimation(){
                            //  isRunning, movingForward
        //cameraScript.HandleZoom(isRunning, moveInput.y == 1);
        //cameraScript.HandleSway(moveInput.x);

        //  TODO: Apply Character Animation in an Blend Tree 
    }

}
