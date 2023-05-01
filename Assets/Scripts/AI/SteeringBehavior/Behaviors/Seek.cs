using UnityEngine;

[System.Serializable]
public class Seek : SteeringBehavior
{
    [HideInInspector]
    public KinematicTarget target;
    [HideInInspector]
    public KinematicTarget character;

    public float maxAcceleration = 5f;

    public override SteeringOutput GetSteering()
    {
        var steering = new SteeringOutput();
        steering.linear = CalculateSeek(target.Position);
        steering.angular = 0f;
        return steering;
    }

    protected Vector3 CalculateSeek(Vector3 targetPosition) => (targetPosition - character.Position).normalized * maxAcceleration;

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        if (target.Position == Vector3.zero) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(character.Position, character.Position + (target.Position - character.Position).normalized * maxAcceleration);
    }
#endif
}
