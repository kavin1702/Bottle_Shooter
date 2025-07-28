using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public GameObject explosionEffect;   // Assign explosion VFX prefab
    public float explosionDelay = 3f;    // Time before explosion
    public float explosionRadius = 5f;   // Radius of effect
    public float explosionForce = 500f;  // Physical force
    public LayerMask affectedLayers;     // What gets affected (e.g., enemies)

    void Start()
    {
        Invoke(nameof(Explode), explosionDelay);
    }

    void Explode()
    {
        // VFX
        if (explosionEffect)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);  // Clean up after 5 seconds
        }

        // Physics push
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, affectedLayers);
        foreach (Collider nearby in hits)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // Optional: damage enemies here
            // EnemyHealth eh = nearby.GetComponent<EnemyHealth>();
            // if (eh != null) eh.TakeDamage(50);
        }

        // Destroy the grenade itself
        Destroy(gameObject);
    }
}
