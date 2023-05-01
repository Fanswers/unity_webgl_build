using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Collider col;
    private Color gizmoColor = new Color(0,1,0,0.25f);
    public bool isNext = false, isReached = false;
    public static Target activeTarget;
    void Awake()
    {
        col = GetComponent<Collider>();
        gizmoColor = Color.red;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetActive()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        gizmoColor = Color.green;
        isNext = true;
        activeTarget = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isNext && other.gameObject.GetComponentInParent<ShipController>() != null)
        {
            TargetReached();
        }
    }

    private void TargetReached()
    {
        isReached = true;
        isNext = false;
        gizmoColor = Color.yellow;
        transform.GetChild(0).gameObject.SetActive(false);
        GameManager.instance.TargetReached(this);
    }

    public bool IsReached()
    {
        return isReached;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
#endif

}
