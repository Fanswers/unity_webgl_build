using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationContext : Context
{
    [SerializeReference]
    public SeparationContextParameter parameters = new SeparationContextParameter();


    public override void FillDangerMap(KinematicTarget character, ref InterestMap dangerMap)
    {
        return;
    }

    private KinematicTarget character;
    public override void FillDesireMap(KinematicTarget character, ref InterestMap dangerMap, ref InterestMap desireMap)
    {
        this.character = character;
        KinematicTarget[] targets = TargetLibrary.EveryoneButMeTarget(character);
        Vector3 linear = new Vector3();
        foreach (var target in targets)
        {
            float distance = Vector3.Distance(character.Position, target.Position);
            if (distance < parameters.threshold)
            {
                float strength = Mathf.Min(parameters.decayCoefficient / (distance * distance), 1f);
                linear += strength * (character.Position - target.Position).normalized;
            }
        }
        if (linear.magnitude != 0f)
        {
            FillFromDotVector(linear, ref desireMap);
        }
        else
        {
            return;
        }
    }

    #if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(character.Position, parameters.threshold);
    }
    #endif

    public class SeparationContextParameter : ContextParameter
    {
        public float threshold = 10f;
        public float decayCoefficient = 1f;
    }
}
