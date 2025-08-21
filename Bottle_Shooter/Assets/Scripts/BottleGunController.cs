using UnityEngine;

public class BottleGunController : MonoBehaviour
{
    [Header("Gun Components")]
    public Animator gunAnimator;
    public ParticleSystem muzzleFlash;
    public AudioSource shootAudio;
    public AudioClip shootClip;
    public AudioClip reloadClip;

    [Header("Raycast Info")]
    public Transform shootOrigin;
    public Camera mainCamera;
    public float range = 100f;

    [Header("Ammo")]
    public int maxAmmo = 10;
    public int maxReserveAmmo = 40;
    private int currentAmmo;
    private int currentReserveAmmo;

    [Header("Shooting")]
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        currentReserveAmmo = maxReserveAmmo;
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && currentReserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (currentAmmo <= 0 && currentReserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        if (gunAnimator) gunAnimator.SetTrigger("Shoot");
        if (muzzleFlash) muzzleFlash.Play();
        if (shootAudio && shootClip) shootAudio.PlayOneShot(shootClip);

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Bottle"))
            {
                BottleBreak bottle = hit.collider.GetComponent<BottleBreak>();
                if (bottle != null)
                {
                    bottle.SendMessage("BreakBottle");
                }
            }
        } 
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        if (gunAnimator) gunAnimator.SetTrigger("Reload");
        if (shootAudio && reloadClip) shootAudio.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(1.5f);

        int needed = maxAmmo - currentAmmo;
        int reloadAmount = Mathf.Min(needed, currentReserveAmmo);

        currentAmmo += reloadAmount;
        currentReserveAmmo -= reloadAmount;

        isReloading = false;
    }
}
