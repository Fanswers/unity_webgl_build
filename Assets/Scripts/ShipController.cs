using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ShipController : MonoBehaviour
{
    public string mapId;
    public InputActionAsset inputActions;
    [Range(.9f, .999f)]
    public float dotMin = 0.9f;
    public float maxDistance = 200f;

    public InputActionReference move, look, thrust, primary, secondary;
    public LineRenderer lineRenderer;
    public TrailRenderer trailRenderer;
    private ShipMovement shipMovement;
    private Ship playerShip;
    private Camera mainCam;
    private Vector3 enemyAimedAtPos;
    private Enemy enemyAimedAt;

    void Start()
    {
        inputActions.FindActionMap(mapId).Enable();
        shipMovement = GetComponent<ShipMovement>();

        move.action.performed += MoveInput;
        move.action.canceled += MoveInput;

        look.action.performed += LookInput;
        look.action.canceled += LookInput;

        primary.action.performed += PrimaryInput;
        secondary.action.performed += SecondaryInput;

        thrust.action.performed += c => shipMovement.Thrust(c.ReadValue<float>());

        playerShip = this.GetComponent<Ship>();
        mainCam = Camera.main;
        lineRenderer.positionCount = 9;

        trailRenderer.transform.parent.parent.parent = null;
    }

    private void Update()
    {
        FindAimedAtEnemy();
        DrawAutoAimIndicator();
        DrawEnemyAimedIndicator();
    }

    private void FindAimedAtEnemy()
    {
        var query = from enemy in TargetLibrary.Instance.enemies
                    where Vector3.Distance(enemy.transform.position, transform.position) < maxDistance
                    where Vector3.Dot((enemy.transform.position - transform.position).normalized, transform.forward) > dotMin
                    select enemy;
        Enemy[] enemies = query.ToArray();

        this.enemyAimedAt = null;
        Enemy closestEnemy = null;
        float minDist = float.PositiveInfinity;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < minDist)
            {
                minDist = distance;
                closestEnemy = enemy;
            }
        }
        this.enemyAimedAt = closestEnemy;
    }

    private void DrawEnemyAimedIndicator()
    {
        if (!enemyAimedAt) {
            trailRenderer.gameObject.SetActive(false);
        }
        else {
        trailRenderer.gameObject.SetActive(true);
        trailRenderer.gameObject.transform.parent.parent.position = enemyAimedAt.transform.position;
        trailRenderer.gameObject.transform.parent.parent.forward = this.transform.forward * -1;
        trailRenderer.gameObject.transform.parent.Rotate(new Vector3(0f, 0f, (360f * Time.deltaTime)));
        }
    }

    private void DrawAutoAimIndicator()
    {
        Vector3[] directions = new Vector3[] {
            mainCam.transform.up,
            mainCam.transform.up + mainCam.transform.right,
            mainCam.transform.right,
            mainCam.transform.right + mainCam.transform.up * -1,
            mainCam.transform.up * -1,
            mainCam.transform.up * -1 + mainCam.transform.right * -1,
            mainCam.transform.right * -1,
            mainCam.transform.right * -1 + mainCam.transform.up,
            mainCam.transform.up
        };

        for (var i = 0; i <= 8; i++)
        {
            float dotProduct = dotMin;
            float angleInRadians = Mathf.Acos(dotProduct);
            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.AngleAxis(angleInDegrees, directions[i]); // Rotate 90 degrees around the Y axis
            Vector3 newDirection = rotation * mainCam.transform.forward;

            Ray ray = new Ray(this.transform.position, newDirection);

            // Perform the raycast and check for a hit
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                lineRenderer.SetPosition(i, hitInfo.point);
            }
            else
            {
                lineRenderer.SetPosition(i, this.transform.position + newDirection * 100);
            }
        }
    }
    #region inputBinding
    private void MoveInput(InputAction.CallbackContext context) { shipMovement.LeftJoystick(context.ReadValue<Vector2>()); }
    private void LookInput(InputAction.CallbackContext context) { shipMovement.RightJoystick(context.ReadValue<Vector2>()); }

    public void PrimaryInput(InputAction.CallbackContext context) { UseModule(1); }
    public void SecondaryInput(InputAction.CallbackContext context) { UseModule(2); }
    #endregion
    private void UseModule(int weaponId)
    {
        if (!enemyAimedAt)
        {
            Vector3 endPosition = transform.position + transform.forward * 100;
            if (weaponId == 1)
            {
                var direction = endPosition - playerShip.primaryWeapon.fireOrigin.position;
                playerShip.Primary(direction);
            }
            else
            {
                var direction = endPosition - playerShip.secondaryWeapon.fireOrigin.position;
                playerShip.Secondary(direction);
            }
        }
        else
        {
            if (weaponId == 1)
                playerShip.Primary(enemyAimedAt.transform.position - playerShip.primaryWeapon.fireOrigin.position);
            else
                playerShip.Secondary(enemyAimedAt.transform.position - playerShip.secondaryWeapon.fireOrigin.position);

        }


    }
    private void OnDestroy()
    {
        move.action.performed -= MoveInput;
        move.action.canceled -= MoveInput;
    }
}

