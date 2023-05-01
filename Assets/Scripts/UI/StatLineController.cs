using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatLineController : MonoBehaviour
{
    public int maxLevel, currentLevel;
    public RectTransform levelHolder;
    public GameObject levelPrefab;
    public GameObject levelMaxSign, price, goldIcon, button;
    public List<Perk> perks;
    public List<int> prices;
    public List<Color> colors;
    public Color selectedColorDiff;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = currentLevel; i < 0; i--)
        {
            Instantiate(levelHolder, levelHolder);
        }
        levelMaxSign.SetActive(false);
        ResetColor();
        ResetPrice();
    }
    
    public void Buy()
    {
        if(currentLevel < maxLevel)
        {
            PerkManager.instance.UnlockPerk(perks[currentLevel]);
            GameManager.instance.Gold -= prices[currentLevel];
            currentLevel ++;

            ResetColor();

            if(currentLevel == maxLevel)
            {
                levelMaxSign.SetActive(true);
                levelHolder.gameObject.SetActive(false);
                price.SetActive(false);
                goldIcon.SetActive(false);
            }
            else
            {
                ResetPrice();                
            }
        }
    }

    private void ResetColor()
    {
        ColorBlock colorBlock = button.GetComponent<Button>().colors;
        colorBlock.normalColor = this.colors[currentLevel];
        colorBlock.selectedColor = new Color(
            this.colors[currentLevel].r * this.selectedColorDiff.r, 
            this.colors[currentLevel].g * this.selectedColorDiff.g, 
            this.colors[currentLevel].b * this.selectedColorDiff.b, 
            this.colors[currentLevel].a * this.selectedColorDiff.a
        );
        colorBlock.disabledColor = this.colors[colors.Count -1];
        button.GetComponent<Button>().colors = colorBlock;
    }

    public void ResetPrice()
    {
        price.GetComponentInChildren<TextMeshProUGUI>().text = "" + prices[currentLevel];
    }
}
