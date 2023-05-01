using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class BlendedSteering : SteeringBehavior
{
    [System.Serializable]
    public struct BehaviorAndWeight
    {
        public float weight;
        [SerializeReference]
        public SteeringBehavior steeringBehavior;

        public BehaviorAndWeight(float weight, SteeringBehavior steeringBehavior) : this()
        {
            this.weight = weight;
            this.steeringBehavior = steeringBehavior;
        }
    }

    public BehaviorAndWeight[] behaviors;
    public float maxAcceleration = 25f;
    public float maxRotation = 5f;

    public override SteeringOutput GetSteering()
    {
        var steering = new SteeringOutput();
        foreach(var behavior in behaviors)
        {
            steering += behavior.weight * behavior.steeringBehavior.GetSteering();
        }
        steering.linear = Vector3.ClampMagnitude(steering.linear, maxAcceleration);
        steering.angular = Mathf.Clamp(steering.angular, -maxRotation, maxRotation);
        return steering;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        if (behaviors == null || behaviors.Length == 0) return;
        foreach(var behavior in behaviors)
        {
            behavior.steeringBehavior.OnDrawGizmos();
        }
    }
#endif
}
