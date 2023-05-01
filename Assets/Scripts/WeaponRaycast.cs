using UnityEngine;

public class WeaponRaycast : ShipModule {
    private float maxTimeBeforeDissapear = .2f, timerBeforeDissapear;
    public LineRenderer lineRenderer;
    public GameObject decalPrefab;
    public ParticleSystem arrivalParticles, startingParticles;
    public float decalSize;
    private Vector3 pointDistance;

    public AudioClip[] clips;
    public AudioSource source;

    void Start() {
        arrivalParticles.transform.parent = null;
    }

    private void PlaySfx()
    {
        if (source.isPlaying)
            source.Stop();
        var clip = Select();
        source.pitch = Generators.RandomBinomial * 0.5f + 1f;
        source.PlayOneShot(clip);
    }

    private int previouslySelected = -1;
    private AudioClip Select()
    {
        if (previouslySelected == -1)
        {
            int roll = Random.Range(0, clips.Length);
            previouslySelected = roll;
            return clips[roll];
        }
        else
        {
            int roll = Random.Range(0, clips.Length);
            while (roll == previouslySelected)
            {
                roll = Random.Range(0, clips.Length);
            }
            previouslySelected = roll;
            return clips[roll];
        }
    }

    public override void Use(Vector3 direction) {
        base.Use(direction);
        PlaySfx();
        if (startingParticles) {
            startingParticles.Play();
        }
        lineRenderer.positionCount = 7;

        lineRenderer.SetPosition(0, this.fireOrigin.position);


        // Create a ray in the forward direction of the object
        Ray ray = new Ray(this.fireOrigin.position, direction);

        Vector3 endPosition = this.fireOrigin.position + direction * 100;
        Ship hitShip = null;
        bool hasHit = false;
        // Perform the raycast and check for a hit
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            hasHit = true;
            endPosition = hitInfo.point;
            hitShip = hitInfo.collider.transform.GetComponentInParent<Ship>();
            if (hitShip != null) hitShip.Hit(this);
        }

        pointDistance = (endPosition - this.fireOrigin.position) / 6;

        for (int i = 1; i < 6; i++)
        {
            lineRenderer.SetPosition(i, this.fireOrigin.position + (direction + pointDistance * i));
        }
        
        lineRenderer.SetPosition(6, endPosition);

        GameObject decalInstance = GameObject.Instantiate(decalPrefab, endPosition - (direction.normalized * decalSize) , Quaternion.LookRotation(direction, this.transform.up));
        decalInstance.transform.localScale *= decalSize;

        if (arrivalParticles && hasHit) {
            arrivalParticles.transform.position = endPosition;
            // TODO Set rotation
            arrivalParticles.transform.up = hitInfo.normal;
            arrivalParticles.Stop();
            arrivalParticles.Play();
        }

    }
}