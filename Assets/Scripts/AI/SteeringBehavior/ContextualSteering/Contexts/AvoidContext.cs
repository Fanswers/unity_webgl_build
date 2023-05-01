using UnityEngine;

public class AvoidContext : Context
{
    public KinematicTarget[] targets;
    public float maxPerceptionRadius = 7f;
    public float extractionDistance = 0.5f;
    
    private Vector3 fleeVector;
    private KinematicTarget closest;
    private bool hasTarget;

    public override void FillDangerMap(KinematicTarget character, ref InterestMap dangerMap)
    {
        hasTarget = false;
        float minDist = float.PositiveInfinity;
        foreach(var t in targets)
        {
            float dist = Vector3.Distance(character.Position, t.Position);
            if (dist < maxPerceptionRadius) continue;
            if (Vector3.Distance(character.Position, t.Position) < minDist)
            {
                hasTarget = true;
                closest = t;
                minDist = Vector3.Distance(character.Position, t.Position);
            }
        }
        if (!hasTarget) return;
        fleeVector = (character.Position - closest.Position).normalized;
        FillFromDotVector(-fleeVector, ref dangerMap, 0.5f);
    }

    public override void FillDesireMap(KinematicTarget character, ref InterestMap dangerMap, ref InterestMap desireMap)
    {
        if (!hasTarget) return;
        if (Vector3.Distance(character.Position, closest.Position) < extractionDistance)
        {
            FillFromDotVector(fleeVector, ref desireMap);
        }
        else
        {
            return;
        }
    }

    #if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(closest.Position, extractionDistance);
    }
    #endif
}
