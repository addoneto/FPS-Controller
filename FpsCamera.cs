using UnityEngine;

public class FpsCamera : MonoBehaviour{

    [SerializeField] private Transform PlayerTranform;
    [SerializeField] private Vector2 sensitivity = new Vector2(250f, 250f);
    [SerializeField] private Vector2 YlookAngle = new Vector2(-90f, 90f);

    [Space(10)]

    [SerializeField][Range(0.5f,10f)] private float smothInput = 8f;

    private Vector2 smoothMouse;
    private float cameraXrotation;

     private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update(){
        Vector2 mouse = new Vector2(
            Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime, 
            Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime
        );

        smoothMouse = Vector2.Lerp(smoothMouse, mouse, Time.deltaTime * smothInput);
    
        cameraXrotation -= smoothMouse.y; // Invert the rotation
        cameraXrotation = Mathf.Clamp(cameraXrotation, YlookAngle.x, YlookAngle.y);
        
        // APPLY ROTATION

        this.transform.localRotation = Quaternion.Euler(cameraXrotation, this.transform.localRotation.y, this.transform.localRotation.z);
        PlayerTranform.Rotate(Vector3.up * smoothMouse.x);
    }
}