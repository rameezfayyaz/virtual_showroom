// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchHaptics : MonoBehaviour
{
    public GameObject menuObject;
    public UnityEvent OnTouchRecorded;
    public bool disable = false;
    private bool running = false;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "hand") {
            if (running) return;
            running = true;
            Debug.Log("Hand collided");
            if (menuObject != null) {
                SpriteAnimations[] anims = menuObject.GetComponents<SpriteAnimations>();
                anims[0].ForceRestart();
            }
            if (OnTouchRecorded != null) OnTouchRecorded.Invoke();
            if (disable) gameObject.SetActive(false);   

            Invoke("enableFunctions", 1.5f);         
        }
    }

    private void enableFunctions() {
        running = false;
    }
}
