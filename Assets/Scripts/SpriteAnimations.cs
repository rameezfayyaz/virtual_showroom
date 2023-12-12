using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
// using UnityEditor;

public class SpriteAnimations : MonoBehaviour
{
    public bool AutoStart;
    public float Distance;
    public float Duration;
    // public float TimeStep;
    public float DelayInStart;
    public SpriteRenderer spriteRendererForAlphaChange;
    public enum Direction {Up, Down, Left, Right}
    public enum MovmentType {DitanceAddedAfter, DistanceSubtractedBefore, None}
    public enum FadeType {Fade_In, Fade_Out, No_Fading}
    public enum ZoomType {Zoom_In, Zoom_Out, None}
    public Direction movement_Direction;
    public MovmentType Movement_Type;
    public FadeType fading_type;
    public ZoomType zoom_type;
    public bool OverrideDefaultOpacity = true;
    public float InitialOpacity = 0;
    public float FinalOpacity = 1;
    private RectTransform ObjTransform;
    private Vector2 DirectionVector;
    private Color spriteColor;
    private bool NullColor = true;
    private Image img;
    Vector2 AlphaSquence;
    public UnityEvent OnMovementCompletion;
    public UnityEvent OnFadeCompletion;
    public UnityEvent BeforeMovementStart;
    public UnityEvent OnZoomCompletion;
    private Vector2 OriginalPos;
    private Vector3 OriginalScale;
    private float OriginalOpacity;
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
            AlphaSquence = SetFading();
            if (Movement_Type == MovmentType.DitanceAddedAfter || Movement_Type == MovmentType.DistanceSubtractedBefore) StartCoroutine(Movement());
            if (fading_type != FadeType.No_Fading) StartCoroutine(Fading());
            if (zoom_type != ZoomType.None) StartCoroutine(Zooming());
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
        }
    }
    private Vector2 SetFading() {
        switch (fading_type) {
            case FadeType.Fade_In: return new Vector2(InitialOpacity, FinalOpacity);
            case FadeType.Fade_Out: return new Vector2(FinalOpacity, InitialOpacity);
            case FadeType.No_Fading: return Vector2.one;
            default: return Vector2.one;
        }
    }
    private Vector2 SetZooming() {
        switch (zoom_type) {
            case ZoomType.Zoom_In: return Vector2.up;
            case ZoomType.Zoom_Out: return Vector2.right;
            case ZoomType.None: return Vector2.one;
            default: return Vector2.one;
        }
    }
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
    private IEnumerator Fading() {
        if (spriteRendererForAlphaChange != null) {
            OriginalOpacity = spriteRendererForAlphaChange.color.a;
            spriteColor = spriteRendererForAlphaChange.color;
            spriteColor.a = AlphaSquence.x;
            NullColor = false;
        }
        else {
            img = gameObject.GetComponent<Image>();
            if (img != null) {
                OriginalOpacity = img.color.a;
                spriteColor = img.color;
                spriteColor.a = AlphaSquence.x;
                NullColor = false;
                if (fading_type == FadeType.Fade_In) if (OverrideDefaultOpacity) img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
            }
            else yield break;
        }
        if (DelayInStart > 0) yield return new WaitForSeconds(DelayInStart);
        // WaitForSeconds wait = new WaitForSeconds(TimeStep);
        float elapsedTime = 0;
        // Vector2 AlphaSquence = SetFading();

        while (elapsedTime < Duration) {
            float ratio = elapsedTime/Duration;
            if (!NullColor) {
                spriteColor.a = Mathf.Lerp(AlphaSquence.x, AlphaSquence.y, ratio);
                if (spriteRendererForAlphaChange != null) spriteRendererForAlphaChange.color = spriteColor;
                else img.color = spriteColor;
            }
            // elapsedTime += TimeStep;
            elapsedTime += Time.fixedDeltaTime;
            // yield return wait;
            yield return new WaitForFixedUpdate();
        }

        if (!NullColor) {
            spriteColor.a = AlphaSquence.y;
            if (spriteRendererForAlphaChange != null) spriteRendererForAlphaChange.color = spriteColor;
            else img.color = spriteColor;
        }
        if (OnFadeCompletion != null) OnFadeCompletion.Invoke();
    }
    private IEnumerator Zooming() {
        if (DelayInStart > 0) yield return new WaitForSeconds(DelayInStart);
        // WaitForSeconds wait = new WaitForSeconds(TimeStep);
        Vector2 sequence = SetZooming();
        float elapsedTime = 0;
        RectTransform trans = gameObject.GetComponent<RectTransform>();
        OriginalScale = trans.localScale;
        trans.localScale = sequence.x * Vector3.one;
        Vector3 finalScale = sequence.y * Vector3.one;
        while (elapsedTime < Duration) {
            float ratio = elapsedTime/Duration;
            trans.localScale = Vector3.Lerp(trans.localScale, finalScale, ratio);
            // elapsedTime += TimeStep;
            elapsedTime += Time.fixedDeltaTime;
            // yield return wait;
            yield return new WaitForFixedUpdate();
        }
        trans.localScale = finalScale;
        if (OnZoomCompletion != null) OnZoomCompletion.Invoke();
    }
    public void ResetObject(float delay) {Invoke("ResetObj", delay);}
    private void ResetObj() {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = OriginalPos;
        rect.localScale = OriginalScale;
        if (spriteRendererForAlphaChange != null) {
            Color clr1 = spriteRendererForAlphaChange.color;
            clr1.a = OriginalOpacity;
            spriteRendererForAlphaChange.color = clr1;
        }
        img = gameObject.GetComponent<Image>();
        if (img != null) {
            Color clr = img.color;
            clr.a = OriginalOpacity;
            img.color = clr;
        }
    }
}
