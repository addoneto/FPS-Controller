using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class FpsCamera : MonoBehaviour{

    [SerializeField] private Transform PlayerTranform;

    [SerializeField] private Vector2 sensitivity = Vector2.zero;
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private Vector2 smoothness = Vector2.zero;
    [SerializeField] private Vector2 YlookAngle = Vector2.zero;

    private float cameraXrotation = 0f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private Vector2 mouseInput;

    void Awake(){
        Cursor.lockState = CursorLockMode.Locked;
        PlayerTranform = this.transform.parent.gameObject.transform;
    }

    void Update()
    {

        //  PREPARE THE INPUTS

        mouseInput = new Vector2(
            Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")
        );

        float desiredRotationX = mouseInput.x * sensitivity.x * Time.deltaTime;
        float desiredRotationY = mouseInput.y * sensitivity.y * Time.deltaTime;

        //  SMOOTH INPUTS

        rotationX = smoothRotation ? Mathf.Lerp(rotationX, desiredRotationX, smoothness.x * Time.deltaTime) : desiredRotationX;
        rotationY = smoothRotation ? Mathf.Lerp(rotationY, desiredRotationY, smoothness.y * Time.deltaTime) : desiredRotationY;

        //  APPLY ROTATIONS

        cameraXrotation -= rotationY; // Invert the rotation
        cameraXrotation = Mathf.Clamp(cameraXrotation, YlookAngle.x, YlookAngle.y);
        
        this.transform.localRotation = Quaternion.Euler(cameraXrotation, this.transform.localRotation.y, this.transform.localRotation.z);
        
        PlayerTranform.Rotate(Vector3.up * rotationX);
    }

    //  Dont acces CameraEffects direcly throght the FpsController to prevent
    // Thread isues like the CameraEffect being executed before Fps Camera 
    //  (that will causes the camera to not rotate)

    public void HandleSway(float rawXInput){
        this.GetComponent<CameraEffects>().ApplySway(rawXInput);
    }

    public void HandleZoom(bool isRunning, bool forward){
        this.GetComponent<CameraEffects>().ApplyZoom(isRunning, forward);
    }
}
