using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualSteering : SteeringBehavior
{
    public float maxDangerLevel = 0.8f;
    public float maxAcceleration = 5f;
    [HideInInspector]
    public InterestMap dangerMap = new InterestMap(InterestMap.sixDir);
    [HideInInspector]
    public InterestMap desireMap = new InterestMap(InterestMap.sixDir);
    public List<Context> contexts = new List<Context>();
    private KinematicTarget character;


    public ContextualSteering(int definition, KinematicTarget character)
    {
        dangerMap = new InterestMap(InterestMap.twentySixDir);
        desireMap = new InterestMap(InterestMap.twentySixDir);
        this.character = character;
    }

    private InterestMap finalMap;

    public override SteeringOutput GetSteering()
    {
        dangerMap.ResetValues();
        desireMap.ResetValues();

        //Calculate danger map
        foreach(var c in contexts)
        {
            c.FillDangerMap(character, ref dangerMap);
        }

        //Calculate desire map
        foreach (var c in contexts)
        {
            c.FillDesireMap(character, ref dangerMap, ref desireMap);
        }

        //Combine both maps
        finalMap = InterestMap.Combine(dangerMap, desireMap, maxDangerLevel);

        Vector3 chosenDir = finalMap.GetWeightedAverage();
        if (chosenDir == Vector3.zero || float.IsNaN(chosenDir.x) || float.IsNaN(chosenDir.y)
            || float.IsNaN(chosenDir.z)) return SteeringOutput.None;
        
        return new SteeringOutput(chosenDir * maxAcceleration, 0f);
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()    
    {
        foreach(var slot in desireMap.slots)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(character.Position, character.Position + slot.Vector * 3f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(character.Position, character.Position + slot.Vector * 3f * slot.Value);

        }
        Gizmos.color = Color.red;
        foreach (var slot in dangerMap.slots)
        {
            Gizmos.DrawLine(character.Position, character.Position + slot.Vector * 3f * slot.Value);
        }
        /*
        Gizmos.color = Color.magenta;
        foreach (var slot in finalMap.slots)
        {
            Gizmos.DrawLine(character.Position, character.Position + slot.Vector * 3f * slot.Value);
        }
        */
        foreach(var c in contexts)
        {
            c.OnDrawGizmos();
        }
    }
#endif
}
