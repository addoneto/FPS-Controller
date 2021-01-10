using UnityEngine;

public class CameraEffects : MonoBehaviour{

    [Header("Zoom")]

    [SerializeField] [Range(1f,20f)] private float forwardZoom = 10f;
    [SerializeField] [Range(-1f,-20f)] private float backwardZoom = -10f;
    [SerializeField] [Range(1f,10f)] private float zoomSmooth = 7f;

    private float baseFOV;
    private float currentFOV;

    [Header("Sway")]

    // TODO: Use Animation Curves

    [SerializeField] [Range(1f,100f)] private float swayAmount = 3f;
    [SerializeField] [Range(1f,10f)] private float swayTurnSmooth = 6f;

    private float finalXrotation = 0f;

    private void Awake(){
        baseFOV = this.GetComponent<Camera>().fieldOfView;
        currentFOV = baseFOV;
    }

    public void Sway(float rawXinput){

        float desiredRotation = rawXinput * swayAmount;
        finalXrotation = Mathf.Lerp(finalXrotation, desiredRotation, Time.deltaTime *  swayTurnSmooth);

        this.transform.localRotation = Quaternion.Euler(
            this.transform.localEulerAngles.x,
            this.transform.localEulerAngles.y,
            -finalXrotation
        );

    }

    public void Zoom(bool running, bool moving, bool forward){

        float desiredFOV;

        if(running && moving) desiredFOV = forward ? baseFOV - forwardZoom : baseFOV - backwardZoom;
        else desiredFOV = baseFOV;

        currentFOV = Mathf.Lerp(currentFOV, desiredFOV, Time.deltaTime *  zoomSmooth);
        this.GetComponent<Camera>().fieldOfView = currentFOV;

    }
}