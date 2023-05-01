using UnityEngine;

public class ViewButton : MonoBehaviour {
    public void Return() {
        ViewManager.instance.Return();
    }

    public void SwapToView(View newView) {
        newView.SwapToThisView();
    }
}