using PrimeTween;
using UnityEngine;

public class SwipingScreen : MonoBehaviour
{
    [SerializeField] private RectTransform container;

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
        container.localPosition += new Vector3(direction.x, 0, 0);
    }

    private void OnStopSwiping()
    {
        float ratio = (container.localPosition.x % canvasSize.Value.x) / canvasSize.Value.x;

        int factor = (int)(container.localPosition.x / canvasSize.Value.x);

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

        Tween.StopAll(container);
        Tween.LocalPositionX(container, factor * canvasSize.Value.x, duration: 0.3f);
    }
}
