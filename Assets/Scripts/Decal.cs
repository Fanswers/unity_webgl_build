using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Decal : MonoBehaviour
{
    DecalProjector decalProjector;
    public float timeAmountBeforeDeath = 2f;
    private float timerBeforeDeath = 0f;
    // Update is called once per frame
    private void Start() {
        decalProjector = GetComponent<DecalProjector>();
    }
    void Update() {
        timerBeforeDeath += Time.deltaTime;
        if (timerBeforeDeath >= timeAmountBeforeDeath) {
            Destroy(this.gameObject);
        }
        decalProjector.fadeFactor = 1 - (timerBeforeDeath / timeAmountBeforeDeath);
    }
}
