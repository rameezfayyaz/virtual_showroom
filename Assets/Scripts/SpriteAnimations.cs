using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.Events;
// using UnityEditor;

public class SpriteAnimations : MonoBehaviour
{
    public bool AutoStart;
    public float Distance;
    public float Duration;
    // public float TimeStep;
    public float DelayInStart;
    // public SpriteRenderer spriteRendererForAlphaChange;
    public enum Direction {Up, Down, Left, Right}
    public enum MovmentType {DitanceAddedAfter, DistanceSubtractedBefore, None}
    // public enum FadeType {Fade_In, Fade_Out, No_Fading}
    // public enum ZoomType {Zoom_In, Zoom_Out, None}
    public Direction movement_Direction;
    public MovmentType Movement_Type;
    // public FadeType fading_type;
    // public ZoomType zoom_type;
    // public bool OverrideDefaultOpacity = true;
    // public float InitialOpacity = 0;
    // public float FinalOpacity = 1;
    private RectTransform ObjTransform;
    private Vector2 DirectionVector;
    // private Color spriteColor;
    // private bool NullColor = true;
    // private Image img;
    // Vector2 AlphaSquence;
    public UnityEvent OnMovementCompletion;
    // public UnityEvent OnFadeCompletion;
    public UnityEvent BeforeMovementStart;
    // public UnityEvent OnZoomCompletion;
    private Vector2 OriginalPos;
    // private Vector3 OriginalScale;
    // private float OriginalOpacity;
    private void Start() {
        if (AutoStart) Initialize();
    }
    public void ForceRestart() {
        Initialize();
    }
    private void Initialize() {
        if (gameObject.GetComponent<RectTransform>() != null) {
            ObjTransform = gameObject.GetComponent<RectTransform>();
            SetDirection();
            if (Movement_Type == MovmentType.DitanceAddedAfter || 
                    Movement_Type == MovmentType.DistanceSubtractedBefore)
                        StartCoroutine(Movement());
        } else {
            Debug.LogError("No Rect Transform found on object");
            Debug.LogWarning("This script is only coded to handle sprites at the moment");
        }
    }
    private void SetDirection() {
        switch (movement_Direction) {
            case Direction.Up: DirectionVector = Vector2.up; break;
            case Direction.Down: DirectionVector = Vector2.down; break;
            case Direction.Left: DirectionVector = Vector2.left; break;
            case Direction.Right: DirectionVector = Vector2.right; break;
        }    }
    private IEnumerator Movement() {
        if (BeforeMovementStart != null) BeforeMovementStart.Invoke();
        if (DelayInStart > 0) yield return new WaitForSeconds(DelayInStart);
        // WaitForSeconds wait = new WaitForSeconds(TimeStep);
        float elapsedTime = 0;
        Vector2 finalPos = ObjTransform.anchoredPosition;
        OriginalPos = finalPos;     //// used for resetting
        // Debug.Log(finalPos);
        if (Movement_Type == MovmentType.DistanceSubtractedBefore) {
            ObjTransform.anchoredPosition -= Distance*DirectionVector;
        } else {
            finalPos += Distance*DirectionVector;
        }
        
        while (elapsedTime < Duration) {
            // float ratio = elapsedTime/Duration;
            ObjTransform.anchoredPosition = Vector2.Lerp(ObjTransform.anchoredPosition, finalPos, elapsedTime/Duration);
            // elapsedTime += TimeStep;
            elapsedTime += Time.fixedDeltaTime;
            // yield return wait;
            yield return new WaitForFixedUpdate();
        }
        ObjTransform.anchoredPosition = finalPos;
        if (OnMovementCompletion != null) OnMovementCompletion.Invoke();
    }
    public void ResetObject(float delay) {Invoke("ResetObj", delay);}
}
