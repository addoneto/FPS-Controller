using UnityEngine;

public class LookCamera : MonoBehaviour
{

    [SerializeField]private Transform camera;
    //[SerializeField]private float smoothAmount;

    //private float currentRotation = 0f;

    void Update()
    {
        // currentRotation = Mathf.Lerp(currentRotation, camera.rotation.x, Time.deltaTime * smoothAmount);

        // this.transform.localRotation = Quaternion.Euler(
        //     currentRotation,
        //     this.transform.localEulerAngles.y,
        //     this.transform.localEulerAngles.z
        // );

        this.transform.eulerAngles = new Vector3(
            camera.eulerAngles.x,
            transform.eulerAngles.y,
            camera.eulerAngles.z * 10
        );

        // this.transform.position = new Vector3(
        //     transform.position.x + (camera.localEulerAngles.z / 1000),
        //     transform.position.y,
        //     transform.position.z
        // );
    }
}
