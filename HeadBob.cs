using UnityEngine;

public class HeadBob : MonoBehaviour{

    [SerializeField] private float yPosCameraOffset = 1f;

    [Space(10)]

    [SerializeField] private AnimationCurve yCurve;
    [SerializeField] private float yAmplitude = 0.1f;
    [SerializeField] private float yFrequency = 2f;

    [Space(10)]

    [SerializeField] private AnimationCurve xCurve;
    [SerializeField] private float xAmplitude = 0.1f;
    [SerializeField] private float xFrequency = 1f;

    [Space(10)]

    [Range(1f,1.1f)][SerializeField] private float runningFrequencyMultiplaier = 1.01f;
    [Range(1f,1.1f)][SerializeField] private float runningAmplitudeMultiplaier = 1.05f;

    private float yCurvePos = 0f;
    private float xCurvePos = 0f;

    public void ScrollHeadBob(bool running){
        if(yCurvePos > 1){ yCurvePos = 0; }
        if(xCurvePos > 1){ xCurvePos = 0; }

        yCurvePos += Time.deltaTime * yFrequency; 
        xCurvePos += Time.deltaTime * xFrequency;   

        yCurvePos *= running ? runningFrequencyMultiplaier : 1f;
        xCurvePos *= running ? runningFrequencyMultiplaier : 1f;

        float finalX = xCurve.Evaluate(xCurvePos) * xAmplitude;
        float finalY = yPosCameraOffset + yCurve.Evaluate(yCurvePos) * yAmplitude;

        finalX *= running ? runningAmplitudeMultiplaier : 1f;
        finalY *= running ? runningAmplitudeMultiplaier : 1f;

        this.transform.localPosition = new Vector3(
            finalX,
            finalY,
            this.transform.localPosition.z
        );

        //Debug.Log(xCurve.Evaluate(xCurvePos));
    }
}