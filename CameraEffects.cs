using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour{

    [SerializeField] [Range(1f,100f)] private float swayAmount = 40f;
    [SerializeField] [Range(1f,10f)] private float swayTurnSmooth = 7f;
    //[SerializeField] [Range(1f,20f)] private float returnSwaySpeed = 7f;

    [SerializeField] [Range(1f,20f)] private float forwardZoomAmout = 10f;
    [SerializeField] [Range(-1f,-20f)] private float backwardZoomAmout = -10f;
    [SerializeField] [Range(1f,10f)] private float zoomSmooth = 7f;

    private float normalFOV;
    private float currentFOV;

    private float finalXrotation = 0f;
    //private float previusXInput = null;

    private void Awake(){
        normalFOV = this.GetComponent<Camera>().fieldOfView;
        currentFOV = normalFOV;
    }

    public void ApplySway(float rawXInput){
        float desiredRotation = rawXInput * swayAmount;
        finalXrotation = Mathf.Lerp(finalXrotation, desiredRotation, Time.deltaTime *  swayTurnSmooth);

        // this.transform.localEulerAngles = new Vector3(
        //     this.transform.localRotation.x,
        //     this.transform.localRotation.y,
        //     -finalXrotation
        // );

        this.transform.localRotation = Quaternion.Euler(
            this.transform.localEulerAngles.x,
            this.transform.localEulerAngles.y,
            -finalXrotation
        );
    }

    public void ApplyZoom(bool isRunning, bool forward){
        float desiredFOV;

        if(isRunning){
            if(forward)
                desiredFOV = normalFOV - forwardZoomAmout;
            else    
                desiredFOV = normalFOV - backwardZoomAmout;
        }else{
            desiredFOV = normalFOV;
        }

        currentFOV = Mathf.Lerp(currentFOV, desiredFOV, Time.deltaTime *  zoomSmooth);
        this.GetComponent<Camera>().fieldOfView = currentFOV;
    }

}
