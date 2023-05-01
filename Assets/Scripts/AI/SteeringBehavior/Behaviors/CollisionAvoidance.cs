using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : SteeringBehavior
{
    public KinematicTarget character;
    public KinematicTarget[] targets;

    public float maxAcceleration = 5f;
    public float radius = 1.5f;

    public override SteeringOutput GetSteering()
    {
        var steering = new SteeringOutput();

        float shortestTime = float.PositiveInfinity;
        bool hasTarget = false;
        KinematicTarget firstTarget = KinematicTarget.None;
        float firstMinSeparation = 0f, firstDistance = 0f;
        Vector3 firstRelativePos = Vector3.zero, firstRelativeVel = Vector3.zero;

        foreach(var target in targets)
        {
            Vector3 relativePos = target.Position - character.Position;
            Vector3 relativeVel = target.Velocity - character.Velocity;
            float relativeSpeed = relativeVel.magnitude;
            float timeToCollision = (Vector3.Dot(relativePos, relativeVel)) / (relativeSpeed * relativeSpeed);
            float distance = relativePos.magnitude;
            float minSeparation = distance - relativeSpeed * timeToCollision;
            if (minSeparation > 2 * radius) continue;

            if (timeToCollision > 0f && timeToCollision < shortestTime)
            {
                if (!hasTarget) hasTarget = true;
                shortestTime = timeToCollision;
                firstTarget = target;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }

        if (!hasTarget) return SteeringOutput.None;
        Vector3 finalRelativePos;
        if (firstMinSeparation <= 0f || firstDistance < 2*radius)
        {
            finalRelativePos = firstTarget.Position - character.Position;
        }
        else
        {
            finalRelativePos = firstRelativePos + firstRelativeVel * shortestTime;
        }
        steering.linear = -finalRelativePos.normalized * maxAcceleration;
        return steering;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(character.Position, radius);
        Gizmos.color = Color.black;
        foreach (var target in targets)
        {
            Gizmos.DrawWireSphere(target.Position, radius);
        }
    }
#endif
}
