using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekContext : Context
{
    [SerializeReference]
    public SeekContextSH parameters = new SeekContextSH();

    protected KinematicTarget target;

    public override void FillDangerMap(KinematicTarget character, ref InterestMap dangerMap)
    {
        return;
    }


    public override void FillDesireMap(KinematicTarget character, ref InterestMap dangerMap, ref InterestMap desireMap)
    {
        target = parameters.kTarget;
        
        if (Vector3.Distance(target.Position, character.Position) < parameters.closeDistance)
        {
            return;
        }
        else
        {
            Vector3 seekVector = (target.Position - character.Position).normalized;
            FillFromDotVector(seekVector, ref desireMap);
        }
    }

    #if UNITY_EDITOR
    public override void OnDrawGizmos()
    {

    }
    #endif

    public class SeekContextSH : ContextParameter
    {
        public Target target;
        public float minDot = 0f;
        public float closeDistance = 10f;
        [HideInInspector]
        public KinematicTarget kTarget => GetTarget(target);
    }
}
