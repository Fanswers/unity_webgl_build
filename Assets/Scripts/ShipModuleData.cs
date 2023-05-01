using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu()]
public class ShipModuleData : SerializedScriptableObject {
    public float cooldown = .05f;
    public int price = 100;
    public int damages = 5;

    public ShipModuleType moduleType;
    [HideIf("IsRaycastType")]
    public GameObject bulletPrefab;

    [HideIf("IsBulletType")]
    public Color lineColor = Color.red;


    private bool IsRaycastType() { return moduleType == ShipModuleType.RAYCAST; }
    private bool IsBulletType() { return moduleType == ShipModuleType.BULLET; }
}

public enum ShipModuleType { BULLET, RAYCAST, OTHER }