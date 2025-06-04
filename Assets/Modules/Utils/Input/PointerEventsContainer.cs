using System;
using UnityEngine;

public class PointerEventsContainer : MonoBehaviour
{
    public event Action OnPointerDown2D;
    public event Action OnPointerUp2D;
    public event Action OnPointerDrag2D;
    public event Action OnPointerEnter2D;
    public event Action OnPointerExit2D;

    private void OnMouseDown()
    {
        OnPointerDown2D?.Invoke();
    }

    private void OnMouseUp()
    {
        OnPointerUp2D?.Invoke();
    }

    private void OnMouseDrag()
    {
        OnPointerDrag2D?.Invoke();
    }

    private void OnMouseEnter()
    {
        OnPointerEnter2D?.Invoke();
    }

    private void OnMouseExit()
    {
        OnPointerExit2D?.Invoke();
    }
}