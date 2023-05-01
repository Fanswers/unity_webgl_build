using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Type type = Type.Timer;

    public enum Type
    {
        Gold, Timer, Health
    }

    public static List<Chest> instances = new List<Chest>();

    private void Start()
    {
        Color c;
        Material m = GetComponentInChildren<Renderer>().materials[1];
        switch (type)
        {
            case Type.Gold:
                c = Color.yellow;
                m.SetColor("_BaseColor", c);
                m.SetColor("_EmissionColor" ,c);
                break;

            case Type.Timer:
                c = Color.blue;
                m.SetColor("_BaseColor", c);
                m.SetColor("_EmissionColor", c);
                break;

            case Type.Health:
                c = Color.blue;
                m.SetColor("_BaseColor", c);
                m.SetColor("_EmissionColor", c);
                break;
        }

        instances.Add(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<ShipController>();
        if (player != null)
        {
            switch (type)
            {
                case Type.Gold:
                    GameManager.instance.Gold += 50;
                    break;
                case Type.Timer:
                    GameManager.instance.Timer -= 60f;
                    break;
                case Type.Health:
                    player.GetComponent<Ship>().Health += 10;
                    break;
            }
            instances.Remove(this);
            Destroy(gameObject);
        }
        
    }
}
