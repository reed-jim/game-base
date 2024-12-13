using PrimeTween;
using UnityEngine;

public class BottomBar : MonoBehaviour
{
    [SerializeField] private RectTransform highlight;

    [SerializeField] private Vector2Variable canvasSize;

    private void Awake()
    {
        SwipeGesture.swipeGestureEvent += OnSwipe;
        SwipeGesture.stopSwipeGestureEvent += OnStopSwiping;
    }

    private void OnDestroy()
    {
        SwipeGesture.swipeGestureEvent -= OnSwipe;
        SwipeGesture.stopSwipeGestureEvent -= OnStopSwiping;
    }

    private void OnSwipe(Vector2 direction)
    {
        highlight.localPosition -= new Vector3(direction.x / 5f, 0, 0);
    }

    private void OnStopSwiping()
    {
        float unitValue = canvasSize.Value.x / 5f;

        float ratio = (highlight.localPosition.x % unitValue) / unitValue;

        int factor = (int)(highlight.localPosition.x / unitValue);

        if (Mathf.Abs(ratio) >= 0.5f)
        {
            if (ratio > 0)
            {
                factor++;
            }
            else
            {
                factor--;
            }
        }

        Tween.StopAll(highlight);
        Tween.LocalPositionX(highlight, factor * unitValue, duration: 0.3f);
    }
}
