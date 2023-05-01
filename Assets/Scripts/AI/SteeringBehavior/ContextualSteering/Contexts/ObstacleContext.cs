using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleContext : Context
{
    public ObstacleHelper parameter = new ObstacleHelper();

    private InterestMap.Slot selected = null;
    private RaycastHit infos;
    private KinematicTarget target;

    public override void FillDangerMap(KinematicTarget character, ref InterestMap dangerMap)
    {
        selected = null;
        float closest = float.PositiveInfinity;
        foreach (var s in dangerMap.slots)
        {
            var ray = new Ray(character.Position, s.Vector);
            var tempInfo = new RaycastHit();
            if (Physics.Raycast(ray, out tempInfo, parameter.lookAhead, parameter.layerMask.value))
            {
                if (infos.distance < closest)
                {
                    infos = tempInfo;
                    selected = s;
                    closest = infos.distance;
                }
            }
        }

        if (selected != null)
            FillFromDotVector(selected.Vector, ref dangerMap);
        else
            return;
    }

    public override void FillDesireMap(KinematicTarget character, ref InterestMap dangerMap, ref InterestMap desireMap)
    {
        if (selected == null) return;
        target = KinematicTarget.CreateVirtualTarget(infos.normal * parameter.avoidDistance);
        Vector3 seekVector = (target.Position - character.Position).normalized;
        FillFromDotVector(seekVector, ref desireMap);
    }

    #if UNITY_EDITOR
    public override void OnDrawGizmos()
    {

    }
    #endif

    public class ObstacleHelper : ContextParameter
    {
        public LayerMask layerMask;
        public float lookAhead = 20f;
        public float avoidDistance = 5f;
    }
}
