using UnityEngine;
using System.Threading.Tasks;
using System;
using DG.Tweening;

// For math functions used in dotween
// https://easings.net/
public static class ViewTransitionService {
    public static async Task TransitView(View view, ViewTransition transition, float time) {
        switch (transition) {
            case ViewTransition.Fade:
                await FadeTransition(view, time);
                break;
            case ViewTransition.Slide:
                await SlideTransition(view, time);
                break;
            case ViewTransition.Scale:
                await ScaleTransition(view, time);
                break;
            case ViewTransition.Custom:
                await CustomTransition(view, time);
                break;
            default:
                break;
        }
    }
    private static async Task FadeTransition(View view, float time) {
        CanvasGroup viewTransform = view.GetComponent<CanvasGroup>();
        if (view.IsViewBeingDisplayed) {
            await viewTransform.DOFade(0, time).SetEase(Ease.Linear).AsyncWaitForCompletion();
        }
        else {
            await viewTransform.DOFade(1, time).From(0).SetEase(Ease.Linear).AsyncWaitForCompletion();
        }
    }
    private static async Task SlideTransition(View view, float time) {
        RectTransform viewTransform = view.GetComponent<RectTransform>();
        if (view.IsViewBeingDisplayed) {
            await viewTransform.DOAnchorPosX(-Screen.width, time).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }
        else {
            viewTransform.anchoredPosition = new Vector2 (-Screen.width, viewTransform.anchoredPosition.y);
            await viewTransform.DOAnchorPosX(0, time).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }
    }
    private static async Task ScaleTransition(View view, float time) {
        if (view.IsViewBeingDisplayed) {
            await view.transform.DOScale(0, time).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        } else {
            await view.transform.DOScale(1, time).From(Vector3.zero).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }
    }
    private static Task CustomTransition(View view, float time) {
        throw new NotImplementedException();
    }
}

public enum ViewTransition
{
    Instant,
    Fade,
    Slide,
    Scale,
    Custom
}