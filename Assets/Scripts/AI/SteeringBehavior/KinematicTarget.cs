using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable, InlineProperty]
public struct KinematicTarget
{
    [HideLabel, OnValueChanged("TargetChange")]
    public GameObject target;

    private void TargetChange()
    {
        if (target == null) return;
        rb = target.GetComponent<Rigidbody>();
        tr = target.transform;
    }

    //public static Vector3 UpDirection = Vector3.up;
    private Rigidbody rb;
    private Transform tr;

    public Vector3 Position
    {
        get
        {
            if (tr == null && target != null) TargetChange();
            if (tr == null) return position;
            return tr.position;
        }
    }
    private Vector3 position;
    public Vector3 Velocity
    {
        get
        {
            if (rb != null)
                return rb.velocity;
            else return velocity;
        }
    }
    private Vector3 velocity;

    public float Orientation
    {
        get
        {
            if (tr == null && target != null) TargetChange();
            if (tr == null) return orientation;
            return Mathf.Deg2Rad * tr.rotation.eulerAngles.z;
        }
    }
    private float orientation;

    public float Rotation
    {
        get
        {
            if (rb == null) return rotation;
            return rb.angularVelocity.z;
        }
    }
    private float rotation;

    public KinematicTarget(Rigidbody rb) : this()
    {
        this.rb = rb;
        this.tr = rb.transform;
    }

    public static bool operator ==(KinematicTarget a, KinematicTarget b) => a.Position == b.position;

    public static bool operator !=(KinematicTarget a, KinematicTarget b) => a.Position != b.position;


    public KinematicTarget(Transform tr) : this()
    {
        this.tr = tr;
    }

    public KinematicTarget(Transform tr, Rigidbody rb) : this()
    {
        this.tr = tr;
        this.rb = rb;
    }

    public static KinematicTarget GetFromObject (GameObject source)
    {
        var rb = source.GetComponent<Rigidbody>();
        var tr = source.transform;
        return new KinematicTarget(tr, rb);
    }

    public static KinematicTarget None => CreateVirtualTarget(Vector3.zero, Vector3.zero, 0f, 0f);

    public static Vector3 GetOrientationAsVector(float orientation)
    {
        return new Vector3(-Mathf.Sin(orientation), Mathf.Cos(orientation), 0f).normalized;
    }
    public static KinematicTarget CreateVirtualTarget(Vector3 position = default, Vector3 velocity = default, float orientation = default, float rotation = default)
    {
        var kt = new KinematicTarget();
        kt.position = position;
        kt.velocity = velocity;
        kt.orientation = orientation;
        kt.rotation = rotation;
        return kt;
    }
}
