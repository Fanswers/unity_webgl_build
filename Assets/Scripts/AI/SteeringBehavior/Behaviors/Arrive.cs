using UnityEngine;

public class Arrive : SteeringBehavior
{
    public KinematicTarget character;
    public KinematicTarget target;

    public float maxAcceleration = 10f;
    public float maxSpeed = 25f;

    public float targetRadius = 1f;
    public float slowRadius = 5f;

    public float timeToTarget = 0.1f;

    private float targetSpeed;

    public override SteeringOutput GetSteering()
    {
        var steering = new SteeringOutput();
        Vector3 direction = target.Position - character.Position;
        float distance = direction.magnitude;

        if (distance < targetRadius)
            return SteeringOutput.None;

        if (distance > slowRadius)
            targetSpeed = maxSpeed;
        else
            targetSpeed = maxSpeed * distance / slowRadius;
        
        Vector3 targetVelocity = direction;
        targetVelocity = targetVelocity.normalized * targetSpeed;
        steering.linear = targetVelocity - character.Velocity;
        steering.linear /= timeToTarget;

        if (steering.linear.magnitude > maxAcceleration)
            steering.linear = steering.linear.normalized * maxAcceleration;
        
        steering.angular = 0f;
        return steering;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.Position, targetRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.Position, slowRadius);
        Gizmos.color = Color.blue;
        var st = GetSteering();
        Gizmos.DrawLine(character.Position, character.Position + st.linear);
    }
#endif
}
