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

    [Range(1f,10f)][SerializeField] private float smoothAmount = 9f;

    private Vector2 smoothInput;
    private float currentSpeed;
    private bool isRunning;
    
    [Header("Gravity & Jump Settings")]

    [SerializeField] private float gravity = -25f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float stickToGroundForce = -1f;

    [Space(5)]

    [SerializeField] private Transform groundCheckTranform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raySphereRadius = 0.3f;
    
    private float yVel;
    private bool isGrounded;

    [Header("Camera")]

    [SerializeField] private GameObject camera;
    
    // TODO: Velocity decay on air
    // TODO: Step sounds

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

        Vector3 moveDirection = transform.right * smoothInput.x + transform.forward * smoothInput.y;

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
        }

        #endregion

        #region ApplyMovement
            controller.Move(new Vector3(0f, yVel, 0f) * Time.deltaTime);
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        #endregion

        #region CameraEffects

        // HEAD BOB
        if(rawInput.x != 0 ||rawInput.y != 0){
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