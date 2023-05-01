using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestMap
{
    public Slot[] slots;

    private Vector3[] directions;
    public Vector3[] Directions
    {
        get => directions;
        set
        {
            directions = value;
            InitializeSlots(directions);
        }
    }

    public InterestMap(Vector3[] directions)
    {
        this.directions = directions;
        InitializeSlots(directions);
    }

    public Slot GetHighestValueSlot()
    {
        float highestValue = float.NegativeInfinity;
        Slot result = null;
        foreach (var slot in slots)
        {
            if (slot.Value > highestValue)
            {
                highestValue = slot.Value;
                result = slot;
            }
        }
        return result;
    }

    public Vector3 GetWeightedAverage()
    {
        float weightedTotal = 0f;
        Vector3 total = Vector3.zero;
        foreach(var s in slots)
        {
            total += s.Vector * s.Value;
            weightedTotal += s.Value;
        }
        return total / weightedTotal;
    }

    public static InterestMap Combine(InterestMap danger, InterestMap desire, float maxDangerLevel = 0.8f)
    {
        var result = new InterestMap(danger.Directions);
        for (int i = 0; i < danger.Directions.Length; ++i)
        {
            float dangerLevel = danger.slots[i].Value;
            float desireLevel = desire.slots[i].Value;
            result.slots[i].Value = (dangerLevel < maxDangerLevel) ? desireLevel - dangerLevel : 0f;
        }
        return result;
    }

    public void ResetValues()
    {
        foreach (var s in slots)
        {
            s.Value = 0f;
        }
    }

    private void InitializeSlots(Vector3[] directions)
    {
        slots = new Slot[directions.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new Slot(directions[i]);
        }
    }

    /*
    private void GenerateSlotList(int resolution)
    {
        slots = new Slot[resolution];
        float increment = (360 / resolution) * Mathf.Deg2Rad * 2;
        float theta = 0f, phi = 0f;
        for (int i = 0; i < resolution; ++i)
        {
            float x = Mathf.Cos(theta) * Mathf.Sin(phi);
            float y = Mathf.Sin(theta) * Mathf.Sin(phi);
            float z = Mathf.Cos(phi);
            slots[i] = new Slot(new Vector3(x, y, z));
            if (i % 2 == 1)
            {
                theta += increment;
            }
            else
            {
                phi += increment;
            }
        }
    }
    */

    public override string ToString()
    {
        string result = $"Interest Map {directions.Length} :";
        foreach (var s in slots)
        {
            result += $"_({s.Vector} | {s.Value})";
        }
        return result;
    }

    public static readonly Vector3[] sixDir = new Vector3[6]
    {
            new Vector3(0,1,0),
            new Vector3(0,-1,0),
            new Vector3(1,0,0),
            new Vector3(-1,0,0),
            new Vector3(0,0,1),
            new Vector3(0,0,-1)
     };


    public static readonly Vector3[] twentySixDir = new Vector3[26]
    {
            new Vector3(0,0,1),
            new Vector3(0, 0, -1),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1).normalized,
            new Vector3(0, 1, -1).normalized,
            new Vector3(0, -1, 0),
            new Vector3(0, -1, 1).normalized,
            new Vector3(0, -1, -1).normalized,
            new Vector3(1, 0, 0),
            new Vector3(1,0,1),
            new Vector3(1, 0, -1),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1).normalized,
            new Vector3(1, 1, -1).normalized,
            new Vector3(-1, -1, 0),
            new Vector3(-1, -1, 1).normalized,
            new Vector3(-1, -1, -1).normalized,
            new Vector3(-1, 0, 0),
            new Vector3(-1,0,1),
            new Vector3(-1, 0, -1),
            new Vector3(-1, 1, 0),
            new Vector3(-1, 1, 1).normalized,
            new Vector3(-1, 1, -1).normalized,
            new Vector3(-1, -1, 0),
            new Vector3(-1, -1, 1).normalized,
            new Vector3(-1, -1, -1).normalized,
    };

    public class Slot
    {
        private float value;
        private Vector3 vector;
        public float Value 
        { 
            get => value;
            set
            {
                this.value = Mathf.Clamp01(value);
            }
        }

        public void SetValue(float newValue)
        {
            if (newValue > Value)
                Value = newValue;
        }

        public Vector3 Vector { get => vector;}

        public Slot(Vector3 vector)
        {
            value = 0;
            this.vector = vector;
        }

    }
}
