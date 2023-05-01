using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Separation : SteeringBehavior
{
    [HideInInspector]
    public KinematicTarget character;
    [HideInInspector]
    public KinematicTarget[] targets;

    public float threshold = 2f;
    public float decayCoefficient = 1f;
    public float maxAcceleration = 5f;

    public override SteeringOutput GetSteering()
    {
        var steering = new SteeringOutput();
        foreach(var target in targets)
        {
            float distance = Vector3.Distance(character.Position, target.Position);
            if (distance < threshold)
            {
                float strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);
                steering.linear += strength * (character.Position - target.Position).normalized;
            }
        }
        return steering;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(character.Position, threshold);
    }
#endif

}
