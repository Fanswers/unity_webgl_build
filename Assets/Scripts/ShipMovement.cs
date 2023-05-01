using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float thrustSpeed = 90;
    public float thrustAccelerationFactor = 1.5f;
    public float strafeAccelerationFactor = 2;
    public float strafeSpeed = 30;
    public float yawTorque = 1000;
    public float pitchTorque = 1000;
    public float rollTorque = 1000;

    public ParticleSystem HighSpeedParticles;

   
    private float yawInput, pitchInput, rollInput, thrustInput, horizontalMoveInput, verticalMoveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
private float currentThrustSpeed, currentStrafeSpeed, currentVStrafeSpeed;
    private void FixedUpdate()
    {
        // float pitch = pitchInput * pitchTorque * Time.fixedDeltaTime;

        // Pitch
        this.rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchInput, -1, 1) * pitchTorque);
        // Yaw
        this.rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(yawInput, -1, 1) * yawTorque);
        this.rb.AddRelativeTorque(Vector3.forward * Mathf.Clamp(-horizontalMoveInput, -1, 1) * rollTorque);


        // Handles movement in vertical and horizontal axis, based on player forward
        // TODO : Move player with a bit of lerping

        currentThrustSpeed += thrustInput * thrustSpeed * thrustAccelerationFactor * Time.fixedDeltaTime;
        currentThrustSpeed = Mathf.Clamp(currentThrustSpeed, -thrustSpeed, thrustSpeed);
        if (thrustInput == 0f)
        {
            if (currentThrustSpeed != 0f) currentThrustSpeed = Mathf.MoveTowards(currentThrustSpeed , 0f, thrustAccelerationFactor * thrustSpeed * Time.fixedDeltaTime);
        }
        //currentStrafeSpeed = Mathf.MoveTowards(currentStrafeSpeed, strafeSpeed * horizontalMoveInput * Time.fixedDeltaTime, strafeAccelerationFactor * strafeSpeed * Time.fixedDeltaTime);
        currentVStrafeSpeed = Mathf.MoveTowards(currentVStrafeSpeed, strafeSpeed * verticalMoveInput, strafeAccelerationFactor * strafeSpeed * Time.fixedDeltaTime * verticalMoveInput);

        Vector3 newVelocity = (this.transform.forward * currentThrustSpeed) + this.transform.up * currentVStrafeSpeed * verticalMoveInput;

        if (currentThrustSpeed >= (thrustSpeed/100) * 80 && !HighSpeedParticles.isPlaying) {HighSpeedParticles.Play();}
        if (currentThrustSpeed <= (thrustSpeed/100) * 80 && HighSpeedParticles.isPlaying) {HighSpeedParticles.Stop();}

        this.rb.velocity = newVelocity;
    }


    public void Thrust(float f) {
        thrustInput = f;
    }
    public void RightJoystick(Vector2 input)
    {
        yawInput = input.x;
        pitchInput = input.y;
    }

    public void LeftJoystick(Vector2 input)
    {
        horizontalMoveInput = input.x;
        verticalMoveInput = input.y;
    }
}
