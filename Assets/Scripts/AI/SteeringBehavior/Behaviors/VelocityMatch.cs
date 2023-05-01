using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatch : SteeringBehavior
{
    public KinematicTarget target;
    public KinematicTarget character;

    public float maxAcceleration = 5f;
    public float timeToTarget = 0.1f;

    public override SteeringOutput GetSteering()
    {
        var steering = new SteeringOutput();
        steering.linear = target.Velocity - character.Velocity;
        steering.linear /= timeToTarget;
        if (steering.linear.magnitude > maxAcceleration)
        {
            steering.linear = steering.linear.normalized;
            steering.linear *= maxAcceleration;
        }
        steering.angular = 0f;
        return steering;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        
    }
#endif
}
