using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "new context pack", menuName = "AI")]
public class ContextPack : SerializedScriptableObject,  System.ICloneable
{
    [ListDrawerSettings(DefaultExpandedState = true)]
    public List<Context> contexts = new List<Context>();

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
