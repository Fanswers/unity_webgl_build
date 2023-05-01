using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkUICell : MonoBehaviour
{
    public TextMeshProUGUI nameArea;
    public TextMeshProUGUI descriptionArea;
    public TextMeshProUGUI fluffArea;
    public Image imageTarget;

    public Perk selectedPerk;

    public void Setup()
    {
        if (nameArea!=null) nameArea.text = selectedPerk.name;
        if (descriptionArea != null) descriptionArea.text = selectedPerk.description;
        if (fluffArea != null) fluffArea.text = selectedPerk.fluffDescription;
        if (imageTarget != null) imageTarget.sprite = selectedPerk.image;
    }

    public void OnClick()
    {
        PerkManager.instance.UnlockPerk(selectedPerk);
        //GameStateManager.instance.PerkChosen();
    }
}
