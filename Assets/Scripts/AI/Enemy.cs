using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [ReadOnly]
    public int id;
    public float minDistToShoot = 50f;
    public float accuracy = 0.5f;
    public float maxDangerLevel = 0.8f;
    public float maxAcceleration = 5f;
    public bool debugMode = false;
    public float threshold = 25;
    [SerializeField]
    private float maxSpeed = 25f;

    public bool chaseActive = false;
    public float distanceBeforeChasing = 100f;
    public float distanceBeforeStopChasing = 200f;

    public ContextualSteering behavior;
    public ContextPack contextPack;

    private Rigidbody rb;
    private KinematicTarget player;
    private KinematicTarget character;
    private Ship ship;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        character = new KinematicTarget(rb);
        behavior = new ContextualSteering(26, character);
        var temp = contextPack.contexts.ToArray();
        behavior.contexts = temp.ToList();
        behavior.maxAcceleration = maxAcceleration;
        behavior.maxDangerLevel = maxDangerLevel;
        TargetLibrary.Register(this, out id);
        player = TargetLibrary.Player;
        ship = GetComponent<Ship>();
    }


    private void OnDestroy()
    {
        TargetLibrary.Remove(this);
    }

    private void FixedUpdate()
    {
        ChaseActiveUpdate();
        Vector3 velocity = rb.velocity;
        if (chaseActive)
        {
            var output = behavior.GetSteering();
            velocity += output.linear * Time.fixedDeltaTime;
            rb.velocity = Vector3.Lerp(rb.velocity, velocity, 0.5f);
        }

        //Clamp Max speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            Vector3 vel = rb.velocity;
            vel = vel.normalized * maxSpeed;
            rb.velocity = vel;
        }
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, rb.velocity.normalized);

        Vector3 direction = (player.Position - character.Position);
        float distance = direction.magnitude;

        if (distance< minDistToShoot)
        {
            if (Vector3.Dot(transform.forward, direction.normalized) > 0.8f)
            {
                float actualAccuracy = accuracy;
                if (player.Velocity.magnitude > threshold) {actualAccuracy *= 0.5f;}
                ship.Primary(((player.Position + (Jitter() * 1 / accuracy)) - character.Position).normalized);
                actualAccuracy = accuracy;
            }
        }

    }

    private void ChaseActiveUpdate()
    {
        KinematicTarget player = TargetLibrary.Player;
        float distance = Vector3.Distance(transform.position, player.Position);
        if (chaseActive && distance > distanceBeforeStopChasing)
        {
            chaseActive = false;
        }
        else if (!chaseActive && distance < distanceBeforeChasing)
        {
            chaseActive = true;
        }
    }

    public Vector3 Jitter()
    {
        return new Vector3(Generators.RandomBinomial, Generators.RandomBinomial, Generators.RandomBinomial);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            behavior?.OnDrawGizmos();
            Gizmos.color = Color.white;
            if (rb != null) Gizmos.DrawLine(transform.position, rb.velocity + transform.position);
        }
    }
#endif
}
