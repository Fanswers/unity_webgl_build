using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Seek
{
    public float maxPrediction = 1f;

    public override SteeringOutput GetSteering()
    {
        var steering = new SteeringOutput();
        var direction = target.Position - character.Position;
        float distance = direction.magnitude;
        float speed = character.Velocity.magnitude;
        float prediction;
        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;
        Vector3 explicitTarget = target.Position;
        explicitTarget += target.Velocity * prediction;
        steering.linear = CalculateSeek(explicitTarget);
        steering.angular = 0f;
        return steering;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        var direction = target.Position - character.Position;
        float distance = direction.magnitude;
        float speed = character.Velocity.magnitude;
        float prediction;
        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;
        Vector3 explicitTarget = target.Position;
        explicitTarget += target.Velocity * prediction;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(explicitTarget, .1f);
    }
#endif
}
