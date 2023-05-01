using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Ship : MonoBehaviour
{
    public bool debugMode = false;
    public int maxHealth = 3;

    public float minimumSpeedToTakeDamageFromCollision = 20f;
    public int collisionDamage = 3;

    public GameObject damageParticlesPrefab;
    public ParticleSystem HighSpeedParticles;


    private ParticleSystem damageParticleSystem;
    private Rigidbody rb;
    [ReadOnly, ShowInInspector]
    private int health;
    public int Health 
    { 
        get => health;
        set
        {
            health = value;
            if (health > maxHealth)
                health = maxHealth;
            else if (health <= 0)
                Death();
            if (damageParticleSystem != null)
            {
                var module = damageParticleSystem.emission;
                float t = -((float)health / (float)maxHealth)+1f;
                print (health/maxHealth);
                module.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Lerp(0f, 100, t));
            }
        }
    }

    [Button()]
    private void ChangeActualHealth(int amount) => Health += amount;

    public ShipModule primaryWeapon;
    public ShipModule secondaryWeapon;

    private void Start() {
        this.rb = GetComponent<Rigidbody>();
        var go = GameObject.Instantiate(damageParticlesPrefab, this.transform);
        go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        damageParticleSystem = go.GetComponent<ParticleSystem>();
        Health = maxHealth;
    }

    public void Primary(Vector3 direction) {
        if (primaryWeapon.CanUse())
            primaryWeapon.Use(direction);
    }

    public void Secondary(Vector3 direction) {
        if (secondaryWeapon.CanUse())
            secondaryWeapon.Use(direction);
    }

    public void Hit(ShipModule weapon) 
    {
        Health -= weapon.shipModuleData.damages;
        //if (!damageParticleSystem.isPlaying) damageParticleSystem.Play();
    }

    public virtual void Death()
    {
        GameManager.instance.Gold += 5;
        if (this.GetComponent<ShipController>() != null)
        {
            FindObjectOfType<GameOver>().GameLost(true);
        }
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        var otherRb = collision.gameObject.GetComponentInParent<Rigidbody>();
        if (otherRb != null)
        {
            if ((rb.velocity - otherRb.velocity).magnitude >= minimumSpeedToTakeDamageFromCollision)
            {
                Health -= collisionDamage;
            }
        }
        else if (rb.velocity.magnitude > minimumSpeedToTakeDamageFromCollision)
        {
            Health -= collisionDamage;
        }

    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!debugMode) return;
        Handles.Label(transform.position, health.ToString());
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(secondaryWeapon.fireOrigin.position, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(primaryWeapon.fireOrigin.position, 1f);
    }

#endif

}
