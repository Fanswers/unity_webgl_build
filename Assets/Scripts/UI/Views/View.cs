using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class View : MonoBehaviour {
    public bool hidePreviousView = true;
    private bool isViewBeingDisplayed = false;
    public bool IsViewBeingDisplayed { get => isViewBeingDisplayed; }

    [Header("When view is shown")]
    [SerializeField] ViewTransition showTransition;
    [SerializeField] float showTransitionTime;

    [Header("When view is hidden")]
    [SerializeField] ViewTransition hideTransition;
    [SerializeField] float hideTransitionTime;


    public void SwapToThisView() {
        ViewManager.instance.SwapToView(this);
    }
    public async virtual Task ShowView() {
        gameObject.SetActive(true);
        await ViewTransitionService.TransitView(this, showTransition, showTransitionTime);
        isViewBeingDisplayed = true;
    }
    public async virtual Task HideView() {
        await ViewTransitionService.TransitView(this, hideTransition, hideTransitionTime);
        isViewBeingDisplayed = false;
        gameObject.SetActive(false);

    }
}