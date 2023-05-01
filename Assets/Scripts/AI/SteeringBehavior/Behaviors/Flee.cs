using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehavior
{
    public KinematicTarget target;
    public KinematicTarget character;

    public float maxAcceleration = 5f;

    public override SteeringOutput GetSteering()
    {
        var steering = new SteeringOutput();
        steering.linear = (character.Position - target.Position).normalized * maxAcceleration;
        steering.angular = 0f;
        return steering;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(character.Position, character.Position + (character.Position - target.Position).normalized * maxAcceleration);
    }
#endif
}
