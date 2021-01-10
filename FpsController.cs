using UnityEngine;

public class Controller : MonoBehaviour{

    [Header("Controller")]

    [SerializeField] private CharacterController controller;

    [Header("Movement Settings")]

    [SerializeField] private float speed = 5f;
    [Range(1f,10f)][SerializeField] private float smoothAmount = 5f;

    private Vector2 smoothInput;
    
    [Header("Gravity & Jump Settings")]

    [SerializeField] private float gravity = -9f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float stickToGroundForce = -1f;

    [Space(5)]

    [SerializeField] private Transform groundCheckTranform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raySphereRadius = 0.2f;
    
    private float yVel;
    private bool isGrounded;

    [Header("Camera")]

    [SerializeField] private GameObject camera;

    // TODO: Diferent Velocities to each direction (And smooth transition between them)
    // TODO: Velocity decay on air

    private void Update() {

        #region Gravity

        isGrounded = Physics.CheckSphere(groundCheckTranform.position, raySphereRadius, groundLayer);

        if(isGrounded && yVel < 0) yVel = stickToGroundForce;
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

        #region Jump

        if(Input.GetButtonDown("Jump") && isGrounded){
            yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        #endregion

        #region ApplyMovement
            controller.Move(new Vector3(0f, yVel, 0f) * Time.deltaTime);
            controller.Move(moveDirection * speed * Time.deltaTime);
        #endregion

    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTranform.position, raySphereRadius);
    }

}