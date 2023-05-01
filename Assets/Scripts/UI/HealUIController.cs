using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealUIController : MonoBehaviour
{
    public TextMeshProUGUI maxHealPrice;
    public Slider healthbar;
    public Button healButton;
    public int goldPerHealth = 1;
    private Ship player;

    public Color dammageColor, repairedColor, selectedColorDiff;

    void Start()
    {
        player = FindObjectOfType<ShipController>().GetComponent<Ship>();
    }

    void Update()
    {
        healthbar.maxValue = player.maxHealth;
        maxHealPrice.text = "" + Mathf.Min(GetGold(), GetHealthMissing() * goldPerHealth);
        healthbar.value = player.Health;
        ResetColor();
    }
    
    private int GetGold()
    {
        return GameManager.instance.Gold;
    }

    private int GetHealthMissing()
    {
        return player.maxHealth - player.Health;
    }

    public void MaxHeal()
    {
        while(GameManager.instance.Gold >= goldPerHealth && GetHealthMissing() > 0)
        {
            HealOne();
        }
    }

    public void HealOne()
    {
        GameManager.instance.Gold -= goldPerHealth;
        player.Health ++;
    }

    private void ResetColor()
    {
        ColorBlock colorBlock = healButton.colors;

        if(GetHealthMissing() > 0)
        {
            colorBlock.normalColor = this.dammageColor;
            colorBlock.selectedColor = new Color(
                this.dammageColor.r * this.selectedColorDiff.r, 
                this.dammageColor.g * this.selectedColorDiff.g, 
                this.dammageColor.b * this.selectedColorDiff.b, 
                this.dammageColor.a * this.selectedColorDiff.a
            );
            colorBlock.disabledColor = repairedColor;
        }
        else
        {
            colorBlock.normalColor = repairedColor;
            colorBlock.selectedColor = new Color(
                this.repairedColor.r * this.selectedColorDiff.r, 
                this.repairedColor.g * this.selectedColorDiff.g, 
                this.repairedColor.b * this.selectedColorDiff.b, 
                this.repairedColor.a * this.selectedColorDiff.a
            );
            colorBlock.disabledColor = repairedColor;
        }
        healButton.colors = colorBlock;
    }
}
