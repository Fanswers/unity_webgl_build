using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Seek
{
    public LayerMask layerMask;
    public float lookAhead = 5f;
    public float lookAheadWhiskers = 2f;
    public float whiskersAngle = 0.5f;
    public float avoidDistance = 1.5f;
    public bool useOrientationInsteadOfVelocity = false;

    private Vector3 frontDir, lWhiskerDir, rWhiskerDir;

    public override SteeringOutput GetSteering()
    {
        frontDir = useOrientationInsteadOfVelocity ? KinematicTarget.GetOrientationAsVector(character.Orientation) : character.Velocity.normalized;
        lWhiskerDir = new Vector3(Mathf.Cos(whiskersAngle) * frontDir.x - Mathf.Sin(whiskersAngle) * frontDir.y, Mathf.Sin(whiskersAngle) * frontDir.x + Mathf.Cos(whiskersAngle) * frontDir.y);
        rWhiskerDir = new Vector3(Mathf.Cos(-whiskersAngle) * frontDir.x - Mathf.Sin(-whiskersAngle) * frontDir.y, Mathf.Sin(-whiskersAngle) * frontDir.x + Mathf.Cos(-whiskersAngle) * frontDir.y);
        Ray frontRay = new Ray(character.Position, frontDir);
        Ray lRay = new Ray(character.Position, lWhiskerDir);
        Ray rRay = new Ray(character.Position, rWhiskerDir);
        RaycastHit infos;
        if (Physics.Raycast(frontRay, out infos, lookAhead, layerMask.value))
        {
            target = KinematicTarget.CreateVirtualTarget(infos.point + infos.normal * avoidDistance);
            return base.GetSteering();
        }
        else if (Physics.Raycast(lRay, out infos, lookAheadWhiskers, layerMask.value))
        {
            target = KinematicTarget.CreateVirtualTarget(infos.point + infos.normal * avoidDistance);
            return base.GetSteering();
        }
        else if (Physics.Raycast(rRay, out infos, lookAheadWhiskers, layerMask.value))
        {
            target = KinematicTarget.CreateVirtualTarget(infos.point + infos.normal * avoidDistance);
            return base.GetSteering();
        }
        else
        {
            return SteeringOutput.None;
        }
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(character.Position, character.Position + frontDir * lookAhead);
        Gizmos.DrawLine(character.Position, character.Position + lWhiskerDir * lookAheadWhiskers);
        Gizmos.DrawLine(character.Position, character.Position + rWhiskerDir * lookAheadWhiskers);
        base.OnDrawGizmos();
    }
#endif
}
