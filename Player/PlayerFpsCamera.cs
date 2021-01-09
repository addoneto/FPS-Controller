using UnityEngine;

public class PlayerFpsCamera : MonoBehaviour{

    [Header("Camera Moviment")]

    [SerializeField] private Transform PlayerTranform;

    [SerializeField] private Vector2 sensitivity = Vector2.zero;
    [SerializeField] private Vector2 YlookAngle = new Vector2(-90f, 90f);
    
    [Space(10)]

    [SerializeField] private bool smoothRotation = true;
    [SerializeField][Range(0.5f,10f)] private float smothInput = 8f;

    private Vector2 smoothMouse;
    private float cameraXrotation;

    [Header("Camera Sway")]

    [SerializeField] private AnimationCurve swayCurve;
    [SerializeField] private float swayAmount;

    [Header("Camera Zoom")]

    [SerializeField] private AnimationCurve zoomCurve;
    [SerializeField] private float forwardZoomAmount;
    [SerializeField] private float backwardZoomAmount;

    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector2 mouse = new Vector2(
            Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime, 
            Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime
        );

        smoothMouse = smoothRotation ? Vector2.Lerp(smoothMouse, mouse, Time.deltaTime * smothInput) : mouse;
        
        cameraXrotation -= smoothMouse.y;
        cameraXrotation = Mathf.Clamp(cameraXrotation, YlookAngle.x, YlookAngle.y);
        
        this.transform.localRotation = Quaternion.Euler(cameraXrotation, this.transform.localRotation.y, this.transform.localRotation.z);
        
        PlayerTranform.Rotate(Vector3.up * smoothMouse.x);

        
    }
}
