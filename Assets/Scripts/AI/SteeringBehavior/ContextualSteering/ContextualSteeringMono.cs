#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ContextualSteeringMono : MonoBehaviour
{
    public int mapResolution = 8;
    public float maxSpeed = 2f;
    [SerializeReference]
    public Context context;
    [SerializeReference]
    public AvoidContext fleeContext;
    private ContextualSteering contextualSteering;
    private Rigidbody rb;

    private void Start()
    {
        contextualSteering = new ContextualSteering(mapResolution, KinematicTarget.GetFromObject(gameObject));
        contextualSteering.maxAcceleration = 25f;
        contextualSteering.contexts.Add(context);
        contextualSteering.contexts.Add(fleeContext);
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;
        Vector3 angularVel = rb.angularVelocity;
        var output = contextualSteering.GetSteering();

        velocity += output.linear * Time.fixedDeltaTime;

        rb.velocity = velocity;

        //Clamp Max speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            Vector3 vel = rb.velocity;
            vel = vel.normalized * maxSpeed;
            rb.velocity = vel;
        }
    }

    private void OnDrawGizmos()
    {
        if (contextualSteering != null)
            contextualSteering.OnDrawGizmos();
    }

}

#endif