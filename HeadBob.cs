using UnityEngine;

public class HeadBob : MonoBehaviour{

    [SerializeField] private float yPosCameraOffset = 1f;

    [Space(10)]

    [SerializeField] private AnimationCurve yCurve;
    [SerializeField] private float yAmplitude = 1f;
    [SerializeField] private float yFrequency = 2f;

    [Space(10)]

    [SerializeField] private AnimationCurve xCurve;
    [SerializeField] private float xAmplitude = 1f;
    [SerializeField] private float xFrequency = 1f;

    private float yCurvePos = 0f;
    private float xCurvePos = 0f;

    public void ScrollHeadBob(){
        if(yCurvePos > 1) yCurvePos = 0;
        if(xCurvePos > 1) xCurvePos = 0;

        yCurvePos += Time.deltaTime * yFrequency; 
        xCurvePos += Time.deltaTime * xFrequency;   

        this.transform.localPosition = new Vector3(
            this.transform.localPosition.x,
            yPosCameraOffset + yCurve.Evaluate(yCurvePos) * yAmplitude,
            this.transform.localPosition.z
        );

        this.transform.localPosition = new Vector3(
            xCurve.Evaluate(xCurvePos) * xAmplitude,
            this.transform.localPosition.y,
            this.transform.localPosition.z
        );

    }
}