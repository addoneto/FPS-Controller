using UnityEngine;

public class FpsControler2 : MonoBehaviour{
    
    [Header("Controller")]

    [SerializeField] private CharacterController controller;

    [Header("Moviment Settings")]

    [SerializeField] private float speed = 5f;
    [Range(1f,10f)][SerializeField] private float smoothAmount = 5f;

    private Vector2 smoothInput;

    [Header("Gravity & Jump Settings")]

    [SerializeField] private float gravity = -9f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private Transform groundCheckTranform;
    [SerializeField] private float raySphereRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Space(5)]

    [SerializeField] private float airVelocityDecay = 0.9f;
    
    private float yVel;
    private bool isGrounded;
    private Vector3 freezeDirection;

    [Header("Camera")]

    [SerializeField] private GameObject camera;

    private void Update()
    {
        //  Gravity

        isGrounded = Physics.CheckSphere(groundCheckTranform.position, raySphereRadius, groundLayer);

        if(isGrounded && yVel < 0){
            yVel = -1f;
        }

        yVel += gravity * Time.deltaTime;

        controller.Move(new Vector3(0f, yVel, 0f) * Time.deltaTime);


        //  Moviment

        Vector2 rawMoveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        rawMoveInput.Normalize();

        smoothInput = Vector2.Lerp(smoothInput, rawMoveInput, Time.deltaTime * smoothAmount);

        Vector3 moveDirection = transform.right * smoothInput.x + transform.forward * smoothInput.y;
        
        if(isGrounded){
            controller.Move(moveDirection * speed * Time.deltaTime);
        }else{
            controller.Move(freezeDirection * speed * Time.deltaTime);
            freezeDirection = new Vector3(freezeDirection.x * airVelocityDecay, freezeDirection.y * airVelocityDecay, freezeDirection.z * airVelocityDecay);
        }   
        

        //  Jump

        if(Input.GetButtonDown("Jump") && isGrounded){
            yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);

            freezeDirection = moveDirection;
        }

        //camera.GetComponent<HeadBob>().ScrollHeadBob();
        
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTranform.position, raySphereRadius);
    }
}
