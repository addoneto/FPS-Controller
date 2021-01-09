using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour{

    [SerializeField] private AnimationCurve yCurve;
    [SerializeField] private float yAmplitude = 0.5f;
    [SerializeField] private float yFrequency = 0.1f;

    private float yCurvePos = 0f;

    public void ScrollHeadBob(){
        //Reset Curve when Stop

        if(yCurvePos > 1){
            yCurvePos = 0;
        }

        yCurvePos += Time.deltaTime * yFrequency;   

        //Debug.Log(yCurvePos);

        this.transform.localPosition = new Vector3(
            this.transform.localPosition.x,
            1f + yCurve.Evaluate(yCurvePos) * yAmplitude,
            this.transform.localPosition.z
        );

    }
}
