using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseContext : Context
{
    public new ChaseSH parameters = new ChaseSH();
    private new KinematicTarget target;

    public override void FillDangerMap(KinematicTarget character, ref InterestMap dangerMap)
    {
        return;
    }


    public override void FillDesireMap(KinematicTarget character, ref InterestMap dangerMap, ref InterestMap desireMap)
    {
        target = parameters.GetTarget(parameters.target);
        var direction = target.Position - character.Position;
        float distance = direction.magnitude;
        float speed = character.Velocity.magnitude;
        float prediction;
        if (speed <= distance / parameters.maxPrediction)
            prediction = parameters.maxPrediction;
        else
            prediction = distance / speed;
        Vector3 explicitTarget = target.Position;
        explicitTarget += target.Velocity * prediction;
        var vTarget = KinematicTarget.CreateVirtualTarget(explicitTarget);

        //seek copy paste
        if (Vector3.Distance(target.Position, character.Position) < parameters.maxTargetDistance)
        {
            return;
        }
        else if (Vector3.Distance(target.Position, character.Position) < parameters.minTargetDistance)
        {
            Vector3 seekVector = (vTarget.Position - character.Position).normalized;
            FillFromDotVector(-seekVector, ref desireMap);
        }
        else
        {
            Vector3 seekVector = (vTarget.Position - character.Position).normalized;
            FillFromDotVector(seekVector, ref desireMap);
        }
    }

    #if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        
    }
    #endif

    public class ChaseSH : ContextParameter
    {
        public Target target;
        public float minDot = 0f;
        [HideInInspector]
        public KinematicTarget kTarget => GetTarget(target);
        public float maxPrediction = 1f;
        public float maxTargetDistance = 15f;
        public float minTargetDistance = 10f;
    }
}
