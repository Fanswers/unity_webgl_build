using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[System.Serializable]
public class PrioritySteering : SteeringBehavior
{
    [OdinSerialize]
    public BlendedSteering[] groups = new BlendedSteering[0];
    public float epsilon = 0.1f;

    private BlendedSteering selectedGroup;

    public override SteeringOutput GetSteering()
    {
        var steering = new SteeringOutput();
        foreach(var group in groups)
        {
            steering = group.GetSteering();
            if (steering.linear.magnitude > epsilon || Mathf.Abs(steering.angular) > epsilon)
            {
                selectedGroup = group;
                return steering;
            }
        }
        if (groups.Length > 0) selectedGroup = groups[groups.Length - 1];
        return steering;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        foreach(var group in groups)
        {
            group.OnDrawGizmos();
        }
    }
#endif
}
