using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[System.Serializable]
public class Perk
{
    [VerticalGroup("id"), HideLabel]
    public string name = "New Perk";
    [VerticalGroup("id"), PreviewField(ObjectFieldAlignment.Left, Height = 64), HideLabel]
    public Sprite image;
    [VerticalGroup("text"), TextArea]
    public string description = "Lorem ipsum dolor sit amet.";
    [VerticalGroup("text"), TextArea]
    public string fluffDescription = "Lorem ipsum dolor sit amet.";

    [TableList]
    public List<PerkEffect> effects;
    public List<string> requiredPerksToUnlock;
}

[System.Serializable]
public class PerkEffect
{
    [InfoBox("ID is invalid", VisibleIf = "CheckStatID", InfoMessageType = InfoMessageType.Error)]
    public string affectedStatId;
    public ModifType type;
    public enum ModifType
    {
        Set, Add, Multiply


    }

    [ShowIf("CheckFloat"), HideLabel, TabGroup("Value")]
    public float fValue;
    [ShowIf("CheckInt"), HideLabel, TabGroup("Value")]
    public int iValue;
    [ShowIf("CheckBool"), HideLabel, TabGroup("Value")]
    public bool bValue;

    private bool CheckFloat() { return PerkManager.GetStatType(affectedStatId) == PlayerStat.PlayerStatType.Float; }
    private bool CheckInt() { return PerkManager.GetStatType(affectedStatId) == PlayerStat.PlayerStatType.Integer; }
    private bool CheckBool() { return PerkManager.GetStatType(affectedStatId) == PlayerStat.PlayerStatType.Bool; }

    public PlayerStat.PlayerStatType GetPlayerStatType()
    {
        return PerkManager.GetStatType(affectedStatId);
    }

    private bool CheckStatID()
    {
        if (PerkManager.instance.allowedStats == null)
            return false;
        foreach(var stat in PerkManager.instance.allowedStats)
        {
            if (stat.variableName == affectedStatId)
                return false;
        }
        return true;
    }
}

[System.Serializable]
public class PlayerStat
{
    public string variableName;
    public string className;
    public PlayerStatType statType;
    public enum PlayerStatType
    {
        Float, Integer, Bool
    }

    public PlayerStat(string className, string variableName, PlayerStatType type)
    {
        this.className = className;
        this.variableName = variableName;
        this.statType = type;
    }
}
