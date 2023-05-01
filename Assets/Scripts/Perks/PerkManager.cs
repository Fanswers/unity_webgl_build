using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Module.Singleton.Controller;
using System.Reflection;

public class PerkManager : Singleton<PerkManager>
{
    public bool verboseMode = false;

    [ShowInInspector, TableList]
    public List<PlayerStat> allowedStats;

    [InfoBox("Some perk is linked to a non-existent perk", InfoMessageType = InfoMessageType.Error, VisibleIf ="CheckConnectedPerks"), TableList, Searchable]
    public List<Perk> pool;
    private bool CheckConnectedPerks()
    {
        if (pool == null)
            return false;
        foreach (var perk in pool)
        {
            foreach (var s in perk.requiredPerksToUnlock)
            {
                if (!PerkExist(s))
                    return true;
            }
        }
        return false;
    }


    [ShowInInspector, ReadOnly]
    public List<Perk> unlockedPerks;

    private void Start()
    {
        if (!CheckPerksValidity(pool))
        {
            Debug.LogError("Invalid perks detected! Please review pool in Perk Manager.");
        }
        unlockedPerks = new List<Perk>();
    }

    private PlayerStat FindPlayerStatByVariableId(string variableId)
    {
        foreach(PlayerStat stat in allowedStats)
        {
            if(stat.variableName == variableId)
            {
                return stat;
            }
        }
        return null;
    }

    public void UnlockPerk(Perk perk)
    {
        pool.Remove(perk);
        unlockedPerks.Add(perk);
        var temp = new List<Perk>();
        temp.Add(perk);
        
        GameObject player = FindObjectOfType<ShipController>().gameObject;

        foreach(PerkEffect effect in perk.effects)
        {
            PlayerStat stat = FindPlayerStatByVariableId(effect.affectedStatId);
            Component[] component = player.GetComponentsInChildren(System.Type.GetType(stat.className));
            try
            {
                for(int i = 0; i < component.Length; i++)
                {
                    FieldInfo fieldInfo = component[i].GetType().GetField(effect.affectedStatId);
                    
                    try
                    {
                        switch(effect.GetPlayerStatType())
                        {
                            case PlayerStat.PlayerStatType.Bool : 
                                fieldInfo.SetValue(component[i], effect.bValue);
                            break;

                            case PlayerStat.PlayerStatType.Float : 
                                float fvalue = (float) fieldInfo.GetValue(component[i]);
                                switch (effect.type)
                                {
                                    case PerkEffect.ModifType.Set : 
                                        fieldInfo.SetValue(component[i], effect.fValue);
                                    break;
                                    case PerkEffect.ModifType.Add : 
                                        fieldInfo.SetValue(component[i], fvalue + effect.fValue);
                                    break;
                                    case PerkEffect.ModifType.Multiply : 
                                        fieldInfo.SetValue(component[i], fvalue * effect.fValue);
                                    break;
                                }
                            break;

                            case PlayerStat.PlayerStatType.Integer : 
                                int ivalue = (int) fieldInfo.GetValue(component[i]);
                                switch (effect.type)
                                {
                                    case PerkEffect.ModifType.Set : 
                                        fieldInfo.SetValue(component[i], effect.iValue);
                                    break;
                                    case PerkEffect.ModifType.Add : 
                                        fieldInfo.SetValue(component[i], ivalue + effect.iValue);
                                    break;
                                    case PerkEffect.ModifType.Multiply : 
                                        fieldInfo.SetValue(component[i], ivalue * effect.iValue);
                                    break;
                                }
                            break;
                            default: break;
                        }
                    }
                    catch
                    {
                        Debug.LogError("Wrong type : no match between " + effect.GetPlayerStatType().ToString() + " and " + stat.variableName + " in the class " + stat.className);
                    }   
                }
            }
            catch
            {
                Debug.LogError("No variable found corresponding to " + stat.variableName + " in the class " + stat.className);
            }
        }
        GameStateManager.instance.PerkChosen();
    }



    private bool PerkExist(string id)
    {
        foreach(var stuff in pool)
        {
            if (stuff.name == id)
                return true;
        }
        return false;
    }

    public List<Perk> GeneratePerkSelection(int amountNeeded = 3)
    {
        var perkSelection = new List<Perk>();
        var eligiblePerks = GetEligiblePerks();

        if (eligiblePerks.Count <= amountNeeded)
        {
            perkSelection = eligiblePerks;
            return perkSelection;
        }

        for(int i = 0; i < amountNeeded; ++i)
        {
            int index = Random.Range(0, eligiblePerks.Count);
            perkSelection.Add(eligiblePerks[index]);
            eligiblePerks.RemoveAt(index);
        }

        return perkSelection;
    }

    private List<Perk> GetEligiblePerks()
    {
        var result = new List<Perk>();
        foreach(var perk in pool)
        {
            if (IsPerkEligible(perk))
                result.Add(perk);
        }
        return result;
    }


    private bool IsPerkEligible(Perk perk)
    {
        if (perk.requiredPerksToUnlock.Count > 0)
        {
            foreach (var requiredPerk in perk.requiredPerksToUnlock)
            {
                bool isRequiredPerkUnlocked = false;
                foreach (var unlockedPerk in unlockedPerks)
                {
                    if (unlockedPerk.name == requiredPerk)
                    {
                        isRequiredPerkUnlocked = true;
                    }
                }
                if (!isRequiredPerkUnlocked)
                    return false;
            }
        }
        return true;
    }
    public static bool CheckPerksValidity(List<Perk> perks)
    {
        bool result = true;
        foreach(var perk in perks)
        {
            if (CheckPerkValidity(perk))
            {
                if (instance.verboseMode) Debug.Log($"PerkManager: {perk.name} valid");
            }
            else
            {
                Debug.LogError($"PerkManager: {perk.name} invalid");
            }
        }
        return result;
    }

    public static bool CheckPerkValidity(Perk perk)
    {
        bool result = true;
        foreach (var effect in perk.effects)
        {
            bool statValidated = false;
            foreach (var allowedStat in instance.allowedStats)
            {
                if (effect.affectedStatId.Equals(allowedStat.variableName))
                {
                    statValidated = true;
                    break;
                }
            }
            if (statValidated)
            {
                if (instance.verboseMode) Debug.Log($"PerkManager: {effect.ToString()} effect validated");
            }
            else
            {
                Debug.LogError($"PerkManager: {effect.ToString()} associated stat not found in Allowed Stats");
                result = false;
            }
        }
        return result;
    }

    public static PlayerStat.PlayerStatType GetStatType(string statVariableName)
    {
        if (instance.allowedStats == null)
            return 0;
        foreach(var stat in instance.allowedStats)
        {
            if (statVariableName == stat.variableName)
                return stat.statType;
        }
        Debug.LogWarning("Stat Not Found");
        return 0;
    }

    void Update()
    {
        if(Input.GetKeyDown("8"))
        {
            UnlockPerk(pool[0]);
        }
    }
}
