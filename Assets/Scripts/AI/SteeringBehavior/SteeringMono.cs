using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class SteeringMono : MonoBehaviour
{
    public bool debugMode = false;
    public bool autoFaceDir = true;

    [SerializeField]
    private float maxSpeed = 25f;

    [SerializeReference]
    public SteeringBehavior behavior;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;

        var output = behavior.GetSteering();

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            behavior.OnDrawGizmos();
            Gizmos.color = Color.white;
            if (rb != null) Gizmos.DrawLine(transform.position, rb.velocity + transform.position);
        }
    }
#endif
}
