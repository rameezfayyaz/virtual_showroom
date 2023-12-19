// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class OculusMovements : MonoBehaviour
{
    public float speed;
    private void Update() {
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero) {
            Vector3 addition = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            transform.position += 
                addition*speed*Time.fixedDeltaTime;
            Debug.Log(addition);
        }
    }
}
