using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float yawTorque = 1000;
    public float pitchTorque = 1000;
    public float rollTorque = 1000;

    public float maxForwardSpeed = 30f, maxStrafSpeed = 30f;
    public float timeToMaxSpeed = 2f;

    public float speedBoostFlat = 40f;
    private float maxForwardSpeedOnBoost() { return (maxForwardSpeed + speedBoostFlat); }
    public float speedBoostDuration = 2f;
    private bool isBoosted;
    private float speedBoostDurationTimer;
    public ParticleSystem HighSpeedParticles;

    private EngineAudio engineAudio;
    private float yawInput, pitchInput, rollInput, thrustInput, horizontalMoveInput, verticalMoveInput;
    private float oldForwardSpeedFactor = 0f, oldStrafSpeedFactor = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        engineAudio = GetComponentInChildren<EngineAudio>();
    }
    private float currentThrustSpeed, currentStrafeSpeed, currentVStrafeSpeed;
    private void FixedUpdate()
    {
        HandleTorque();
        SetVelocity();
    }

    private void HandleTorque()
    {
        // Pitch
        this.rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchInput, -1, 1) * pitchTorque);
        // Yaw
        this.rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(yawInput, -1, 1) * yawTorque);
        // Roll
        this.rb.AddRelativeTorque(Vector3.forward * Mathf.Clamp(-horizontalMoveInput, -1, 1) * rollTorque);
    }

    private void SetVelocity()
    {
        //
        // Forward speed calculation
        Vector3 newForwardVelocity = Vector3.zero;
        speedBoostDurationTimer -= Time.fixedDeltaTime;
        float factorSign = 1f;
        // Calculation of forward
        if (speedBoostDurationTimer <= 0 && isBoosted)
        {
            BoostOver();

        }
        if (!isBoosted)
        {
            if (this.rb.velocity.magnitude <= .1f && Mathf.Abs(oldForwardSpeedFactor) > .5f)
            {
                oldForwardSpeedFactor = 0;
            }
            factorSign = Mathf.Sign(thrustInput - oldForwardSpeedFactor);
            float newForwardSpeedFactor = oldForwardSpeedFactor + ((Time.fixedDeltaTime / timeToMaxSpeed) * factorSign);
            float newForwardSpeed = InveretSquared(newForwardSpeedFactor) * maxForwardSpeed;

            newForwardVelocity = newForwardSpeed * this.transform.forward;

            oldForwardSpeedFactor = newForwardSpeedFactor;
        }

        // Calculation of forward if boosted
        else
        {
            factorSign = Mathf.Sign(thrustInput - oldForwardSpeedFactor);
            float newForwardSpeedFactor = oldForwardSpeedFactor + ((Time.fixedDeltaTime / timeToMaxSpeed) * factorSign);
            float newForwardSpeed = InveretSquared(newForwardSpeedFactor) * maxForwardSpeedOnBoost() ;

            newForwardVelocity = newForwardSpeed * this.transform.forward;

            oldForwardSpeedFactor = newForwardSpeedFactor;
        }



        // 
        // Straf speed calculation

        factorSign = Mathf.Sign(verticalMoveInput - oldStrafSpeedFactor);
        float newStrafSpeedFactor = oldStrafSpeedFactor + ((Time.fixedDeltaTime / timeToMaxSpeed) * factorSign);
        float newStrafSpeed = newStrafSpeedFactor * maxStrafSpeed;

        Vector3 newStrafVelocity = newStrafSpeed * this.transform.up;

        oldStrafSpeedFactor = newStrafSpeedFactor;

        // Setting velocity
        Vector3 newVelocity = newForwardVelocity + newStrafVelocity;

        // Playing particles based on forward speed
        if (oldForwardSpeedFactor >= .8f && !HighSpeedParticles.isPlaying) { HighSpeedParticles.Play(); }
        if (oldForwardSpeedFactor < .8f && HighSpeedParticles.isPlaying) { HighSpeedParticles.Stop(); }

        this.rb.velocity = newVelocity;
    }

    private void BoostOver()
    {
        isBoosted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("boost"))
            BoostStart();
    }

    private void BoostStart()
    {
        engineAudio.BoostGateSound();
        isBoosted = true;
        speedBoostDurationTimer = 2f;
        RecalculateOldForwardSpeedFactorOnBoost();
    }

    private void RecalculateOldForwardSpeedFactorOnBoost()
    {
        float speedWithOldFactor = InveretSquared(oldForwardSpeedFactor) * maxForwardSpeed;
        float newLinearFactor = (speedWithOldFactor / maxForwardSpeedOnBoost());
        float newFactor = (newLinearFactor * newLinearFactor) * Mathf.Sign(oldForwardSpeedFactor);
        // float newForwardSpeedFactor = oldForwardSpeedFactor + ((Time.fixedDeltaTime / timeToMaxSpeed) * factorSign);
        oldForwardSpeedFactor = (oldForwardSpeedFactor * oldForwardSpeedFactor) * Mathf.Sign(oldForwardSpeedFactor);
    }

    private float InveretSquared(float newForwardSpeedFactor)
    {
        return MathF.Sqrt(Mathf.Abs(newForwardSpeedFactor)) * Mathf.Sign(newForwardSpeedFactor);
    }

    #region input
    public void Thrust(float f) { thrustInput = f; }
    public void RightJoystick(Vector2 input) { yawInput = input.x; pitchInput = input.y; }
    public void LeftJoystick(Vector2 input) { horizontalMoveInput = input.x; verticalMoveInput = input.y; }
    #endregion
}
