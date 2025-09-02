using UnityEngine;

public class BottleBreak : MonoBehaviour
{
    [Header("Fractured Bottle Setup")]
    public GameObject fracturedBottlePrefab;

    [Header("Break Effects")]
    public AudioClip breakSound;
    public float explosionForce = 500f;
    public float explosionRadius = 2f;
    public float upwardsModifier = 0.2f;

    [Header("Cleanup")]
    public float destroyAfterSeconds = 5f;

    private bool hasBroken = false;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // Prevent multiple breaks & check impact force
    //    if (!hasBroken && collision.relativeVelocity.magnitude > 2f)
    //    {
    //        hasBroken = true;
    //        BreakBottle();
    //    }
    //}

    public  void BreakBottle()
    {
      
        GameObject fractured = Instantiate(fracturedBottlePrefab, transform.position, transform.rotation);

     
        foreach (Rigidbody rb in fractured.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
        }

       
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        }

        
        Destroy(fractured, destroyAfterSeconds);

       
        Destroy(gameObject);
    }
}
