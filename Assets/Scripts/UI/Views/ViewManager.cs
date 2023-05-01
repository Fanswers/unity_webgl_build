using System.Threading.Tasks;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour {
    public static ViewManager instance;
    public View startingView,
        gameView,
        shopView,
        shopIconView,
        gameOverView;
    Stack<View> viewStack = new Stack<View>();

    // Start is called before the first frame update
    void Start() {
        if (instance == null)
            instance = this;

        SwapToView(startingView);
    }

    // Affiche la page demandée
    public async void SwapToView(View newView) {
        if (viewStack.Count != 0 && newView.hidePreviousView)
            await RemoveView(viewStack.Pop());
        viewStack.Push(newView);
        await DisplayView(newView);
    }

    // Retourne sur la page précédente
    public async void Return() {
        if (viewStack.Count > 1)
            await RemoveView(viewStack.Pop());
    }

    // Used for state
    private async Task DisplayView(View view) {
        await view.ShowView();
    }
    private async Task RemoveView(View view) {
        await view.HideView();
    }
}

// GameObject.FindObjectsOfType<Unity>()