using UnityEngine;

public class ShipModule : MonoBehaviour
{

    public ShipModuleData shipModuleData;
    private float cooldownTimer = 0f;
    public Transform fireOrigin;
    public Animator animator;
    public float cooldownTime = 0.2f;
    public void Equip() {
        
    }

    public bool CanUse() {
        return cooldownTimer + cooldownTime < Time.time;
    }

    public virtual void Use(Vector3 direction)
    {
        if (!this.CanUse()) return;
        cooldownTimer = Time.time;
        animator.SetTrigger("Use");
    }

}