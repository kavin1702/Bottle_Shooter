using UnityEngine;

public class BottleBreak : MonoBehaviour
{
    public GameObject fracturedBottlePrefab;
    public AudioClip breakSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2f) // Adjust threshold
        {
            BreakBottle();
        }
    }

    void BreakBottle()
    {
        // Spawn fractured bottle
        GameObject fractured = Instantiate(fracturedBottlePrefab, transform.position, transform.rotation);

        // Add explosion force to pieces
        foreach (Rigidbody rb in fractured.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(500f, transform.position, 3f);
        }

        // Play breaking sound
        if (breakSound!=null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        }

        // Destroy fractured pieces after 5 seconds
        Destroy(fractured, 5f);

        // Destroy original bottle
        Destroy(gameObject);
    }
}
