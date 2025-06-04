using System;
using UnityEngine;

public class MouseEventsContainer : MonoBehaviour
{
    public static Action OnMouseDown;
    public static Action OnMouse;
    public static Action OnMouseUp;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseDown?.Invoke();
        if (Input.GetMouseButton(0))
            OnMouse?.Invoke();
        if (Input.GetMouseButtonUp(0))
            OnMouseUp?.Invoke();
    }

    private void OnDestroy()
    {
        OnMouseDown = () => { };
        OnMouse = () => { };
        OnMouseUp = () => { };
    }
}