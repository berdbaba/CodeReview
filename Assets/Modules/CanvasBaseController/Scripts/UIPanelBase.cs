using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using DG.Tweening;

public abstract class BasePanel : MonoBehaviour
{
    [Header("Core Components")]
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Animator animator;

    [Header("Transition Settings")]
    [SerializeField]
    private float fadeDuration = 0.25f;

    [Header("Buttons & Actions")]
    [SerializeField]
    private List<Button> _panelOpeningButtons = new();
    [SerializeField]
    private List<Button> _panelClosingButtons = new();
    [SerializeField]
    private List<ActionContainer> _panelOpeningActions = new();
    [SerializeField]
    private List<ActionContainer> _panelClosingActions = new();

    public event Action Shown;
    public event Action Hidden;

    private bool isVisible = false;
    private bool isAnimating = false;

    private static readonly int OPEN_HASH = Animator.StringToHash("Open");
    private static readonly int CLOSE_HASH = Animator.StringToHash("Close");

    protected virtual void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                Debug.LogWarning($"CanvasGroup is not set on '{name}'.");
        }

        _panelOpeningButtons.ForEach(b => b.onClick.AddListener(Show));
        _panelClosingButtons.ForEach(b => b.onClick.AddListener(Hide));
        _panelOpeningActions.ForEach(ac => ac.action += Show);
        _panelClosingActions.ForEach(ac => ac.action += Hide);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        if (isVisible || isAnimating)
            return;

        isVisible = true;
        isAnimating = true;
        gameObject.SetActive(true);

        if (animator != null && HasAnimatorParameter(OPEN_HASH))
        {
            ToggleCanvasInteraction(false);
            animator.SetTrigger(OPEN_HASH);
        }
        else
        {
            ToggleCanvasInteraction(false);
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, fadeDuration)
                .OnComplete(OnShowAnimationComplete);
        }
    }

    public virtual void Hide()
    {
        if (!isVisible || isAnimating)
            return;

        isVisible = false;
        isAnimating = true;

        if (animator != null && HasAnimatorParameter(CLOSE_HASH))
        {
            ToggleCanvasInteraction(false);
            animator.SetTrigger(CLOSE_HASH);
        }
        else
        {
            canvasGroup.DOFade(0f, fadeDuration)
                .OnComplete(OnHideAnimationComplete);
        }
    }

    public void OnShowAnimationComplete()
    {
        isAnimating = false;
        ToggleCanvasInteraction(true);
        Shown?.Invoke();
    }

    public void OnHideAnimationComplete()
    {
        isAnimating = false;
        ToggleCanvasInteraction(false);
        gameObject.SetActive(false);
        Hidden?.Invoke();
    }

    private void ToggleCanvasInteraction(bool enable)
    {
        if (canvasGroup == null) return;
        canvasGroup.interactable = enable;
        canvasGroup.blocksRaycasts = enable;
    }

    private bool HasAnimatorParameter(int hash)
    {
        if (animator == null) return false;
        foreach (var p in animator.parameters)
            if (p.nameHash == hash)
                return true;
        return false;
    }

    protected virtual void OnDestroy()
    {
        _panelOpeningButtons.ForEach(b => b.onClick.RemoveListener(Show));
        _panelClosingButtons.ForEach(b => b.onClick.RemoveListener(Hide));
        _panelOpeningActions.ForEach(ac => ac.action -= Show);
        _panelClosingActions.ForEach(ac => ac.action -= Hide);
    }
}