using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Context
{
    public abstract void FillDangerMap(KinematicTarget character, ref InterestMap dangerMap);

    public abstract void FillDesireMap(KinematicTarget character, ref InterestMap dangerMap, ref InterestMap desireMap);

#if UNITY_EDITOR
    public abstract void OnDrawGizmos();
#endif

    public static void FillFromDotVector(Vector3 vector, ref InterestMap map, float minDot = 0f)
    {
        for (int i = 0; i < map.slots.Length; i++)
        {
            float dot = Vector3.Dot(vector, map.slots[i].Vector);
            if (dot > minDot)
                map.slots[i].SetValue(dot);
        }
    }

    [System.Serializable]
    public abstract class ContextParameter
    {
        public enum Target
        {
            PLAYER
        }


        public KinematicTarget GetTarget(Target target)
        {
            switch(target)
            {
                case Target.PLAYER:
                    return TargetLibrary.Player;
                    break;
            }
            return KinematicTarget.None;
        }
    }
}
