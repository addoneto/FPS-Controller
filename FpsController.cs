using UnityEngine;

public class FpsController : MonoBehaviour{

    [Header("Controller")]

    [SerializeField] private CharacterController controller;

    [Header("Movement Settings")]

    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float backwardSpeed = 2f;
    [SerializeField] private float sidewardSpeed = 3.5f;

    [Range(1f,2f)][SerializeField] private float runningMultiplaier = 1.5f;

    [Space(2)]

    // More smoothAmount intuitively means less smoothness
    [Range(1f,10f)][SerializeField] private float smoothAmount = 9f;

    private Vector2 smoothInput;
    private float currentSpeed;
    private bool isRunning;

    private Vector2 applyInput;
    private float applySpeed;
    
    [Header("Gravity & Jump Settings")]

    [SerializeField] private float gravity = -25f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float stickToGroundForce = -1f;

    [Space(5)]

    [SerializeField] private Transform groundCheckTranform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raySphereRadius = 0.3f;

    [Space(5)]

    [Range(0f,4f)][SerializeField] private float airVelocityControll = 2f;
    [Range(1f,1.2f)][SerializeField] private float airDrag = 2f;
    
    private float yVel;
    public bool isGrounded { get; private set; }

    [Header("Camera")]

    [SerializeField] private GameObject camera;


    private void Update() {

        #region Gravity

        isGrounded = Physics.CheckSphere(groundCheckTranform.position, raySphereRadius, groundLayer);

        if(isGrounded && yVel < 0){ yVel = stickToGroundForce; }
        yVel += gravity *  Time.deltaTime;

        #endregion

        #region Movement

        Vector2 rawInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        rawInput.Normalize();

        smoothInput = Vector2.Lerp(smoothInput, rawInput, Time.deltaTime * smoothAmount);

        #endregion

        #region Velocities/Running

        if(rawInput.y == 1f){ // Walking forward
            currentSpeed = forwardSpeed;
        }else if(rawInput.y == -1f){ // Walking backward
            currentSpeed = backwardSpeed;
        }else{ // Walking sidewars or diagonaly
            currentSpeed = sidewardSpeed;
        }

        if(Input.GetKey(KeyCode.LeftShift)){
            currentSpeed *= runningMultiplaier;
            isRunning = true;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift)){
            isRunning = false;
        }

        #endregion

        #region Jump

        if(Input.GetButtonDown("Jump") && isGrounded){
            yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
            applySpeed = currentSpeed;
        }

        #endregion

        #region AirVelocityControllDecay

        if(isGrounded){
            applyInput = smoothInput;
            applySpeed = currentSpeed;
        }else{
            applySpeed /= airDrag;
        }
        
        Vector3 moveDirection = transform.right * applyInput.x + transform.forward * applyInput.y;
        Vector3 finalMove = moveDirection * applySpeed * Time.deltaTime;

        if(!isGrounded && (rawInput.x != 0f || rawInput.y != 0f)){
            // Apply a friction/drag force according to 'airVelocityControll'

            Vector3 newFinal = transform.right * ((applyInput.x * applySpeed) + smoothInput.x * airVelocityControll) + transform.forward * ((applyInput.y * applySpeed) + smoothInput.y * airVelocityControll);
            finalMove = newFinal * Time.deltaTime;
        }

        #endregion

        #region ApplyMovement
            controller.Move(new Vector3(0f, yVel, 0f) * Time.deltaTime);
            controller.Move(finalMove);
        #endregion

        #region CameraEffects

        // HEAD BOB
        if( (rawInput.x != 0 || rawInput.y != 0) && isGrounded){
            camera.GetComponent<HeadBob>().ScrollHeadBob(isRunning);
        }

        //  RUNNING FOV
        camera.GetComponent<CameraEffects>().Zoom(isRunning, rawInput.y != 0f, rawInput.y == 1);

        // SIDE SWAY
        camera.GetComponent<CameraEffects>().Sway(rawInput.x);

        #endregion
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTranform.position, raySphereRadius);
    }

}