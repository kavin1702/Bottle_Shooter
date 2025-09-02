


using UnityEngine;

public class BottleGunController : MonoBehaviour
{
    [Header("Gun Components")]
    public Animator gunAnimator;
    public ParticleSystem muzzleFlash;
    public AudioSource shootAudio;
    public AudioClip shootClip;

    [Header("Raycast Info")]
    public Camera mainCamera;
    public float range = 100f;
    public LayerMask bottleLayer;   // 👈 Assign the "Bottle" layer in the Inspector

    [Header("Ammo")]
    public int currentAmmo = 6;   // start with 6 bullets
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        gameManager.OnBulletFired(); // 👈 Notify GameManager (reduce ammo UI etc.)

        // Gun FX
        if (gunAnimator) gunAnimator.SetTrigger("Shoot");
        if (muzzleFlash) muzzleFlash.Play();
        if (shootAudio && shootClip) shootAudio.PlayOneShot(shootClip);

        // Raycast ONLY on Bottle layer
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range, bottleLayer))
        {
            BottleBreak bottle = hit.collider.GetComponent<BottleBreak>();
            if (bottle != null)
            {
                bottle.BreakBottle();      // call public method directly
                gameManager.OnBottleShot(); // increase score
            }
        }
    }
}
