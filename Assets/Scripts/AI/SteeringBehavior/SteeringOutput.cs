using UnityEngine;
public struct SteeringOutput
{
    public Vector3 linear;
    public float angular;
    public static SteeringOutput None = new SteeringOutput(Vector3.zero, 0f);

    public SteeringOutput(Vector3 linear, float angular)
    {
        this.linear = linear;
        this.angular = angular;
    }

    public static SteeringOutput operator +(SteeringOutput a, SteeringOutput b) => new SteeringOutput(a.linear + b.linear, a.angular + b.angular);
   
    public static SteeringOutput operator -(SteeringOutput a, SteeringOutput b) => new SteeringOutput(a.linear - b.linear, a.angular - b.angular);

    public static SteeringOutput operator *(SteeringOutput a, float b) => new SteeringOutput(a.linear * b, a.angular *b);

    public static SteeringOutput operator *(float a, SteeringOutput b) => new SteeringOutput(b.linear * a, b.angular * a);

}