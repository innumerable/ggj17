using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour {

    [SerializeField]
    float leftEyeRotationSpeed = 0.1f;
    [SerializeField]
    float rightEyeRotationSpeed = 0.12f;

    Quaternion myRotation;
    Transform parentTransform;
    public Transform lOuterEyeRotation;
    public Transform rOuterEyeRotation;

	// Use this for initialization
	void Start ()
    {
        parentTransform = GetComponentInParent<Transform>();

	}
	
	// Update is called once per frame
	void Update () {
        //transform.localPosition = this.myPosition;
        transform.rotation = Quaternion.identity;// = Quaternion.Euler(0,0,-1 * parentTransform.localRotation.eulerAngles.z);
        lOuterEyeRotation.rotation = Quaternion.Euler(0, 0, -parentTransform.localRotation.eulerAngles.z * leftEyeRotationSpeed) * lOuterEyeRotation.rotation;
        rOuterEyeRotation.rotation = Quaternion.Euler(0, 0, parentTransform.localRotation.eulerAngles.z * rightEyeRotationSpeed) * rOuterEyeRotation.rotation;
    }
}
