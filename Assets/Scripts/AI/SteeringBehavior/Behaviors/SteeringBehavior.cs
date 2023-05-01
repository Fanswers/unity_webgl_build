[System.Serializable]
public abstract class SteeringBehavior
{
    public abstract SteeringOutput GetSteering();
#if UNITY_EDITOR
    public abstract void OnDrawGizmos();
#endif
}