using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaycastGunManager : MonoBehaviour
{
    [System.Serializable]
    public class GunData
    {
        public string gunName;

        [Header("Gun Components")]
        public Animator gunAnimator;
        public ParticleSystem muzzleFlash;
        public AudioSource shootAudio;
        public AudioClip shootClip;
        public AudioClip reloadClip;

        [Header("Raycast Info")]
        public Transform shootOrigin;

        [Header("References")]
        public GameObject gunObject;

        [Header("Ammo")]
        public int maxAmmo = 10;
        [HideInInspector] public int currentAmmo;

        public int maxReserveAmmo = 40;
        [HideInInspector] public int currentReserveAmmo;
    }

    [Header("Gun Settings")]
    public List<GunData> allGuns = new List<GunData>();
    private GunData currentGun;
    private int currentGunIndex = 0;

    [Header("Camera")]
    public Camera mainCamera;

    [Header("Shooting")]
    public float fireRate = 0.2f;
    public float range = 100f;
    public GameObject hitEffectPrefab;
    public GameObject bloodEffectPrefab;
    private float nextFireTime = 0f;
    private bool isReloading = false;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    void Start()
    {
        foreach (var gun in allGuns)
        {
            gun.currentAmmo = gun.maxAmmo;
            gun.currentReserveAmmo = gun.maxReserveAmmo;
        }
        EquipGun(0);
    }

    void Update()
    {
        // Gun switching
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipGun(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipGun(1);

        // Prevent input during reload
        if (isReloading) return;

        // Manual reload
        if (Input.GetKeyDown(KeyCode.R) &&
            currentGun.currentAmmo < currentGun.maxAmmo &&
            currentGun.currentReserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Auto-reload if empty
        if (currentGun.currentAmmo <= 0 &&
            currentGun.currentReserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Shooting
        if (Input.GetMouseButtonDown(0) &&
            Time.time >= nextFireTime &&
            currentGun.currentAmmo > 0)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    public void EquipGun(int index)
    {
        if (index < 0 || index >= allGuns.Count) return;

        // Deactivate all guns
        foreach (var gun in allGuns)
        {
            gun.gunObject.SetActive(false);

            if (gun.muzzleFlash != null)
            {
                gun.muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                gun.muzzleFlash.Clear();
            }
        }

        // Activate selected gun
        currentGunIndex = index;
        currentGun = allGuns[currentGunIndex];
        currentGun.gunObject.SetActive(true);

        // Initialize ammo if needed
        if (currentGun.currentAmmo <= 0)
            currentGun.currentAmmo = currentGun.maxAmmo;

        if (currentGun.currentReserveAmmo <= 0)
            currentGun.currentReserveAmmo = currentGun.maxReserveAmmo;

        UpdateAmmoUI();
    }

    void Shoot()
    {
        // Extra safeguard
        if (isReloading) return;

        currentGun.currentAmmo--;

        // Play animation
        if (currentGun.gunAnimator)
            currentGun.gunAnimator.SetTrigger("Shoot");

        // Muzzle flash
        if (currentGun.muzzleFlash)
            currentGun.muzzleFlash.Play();

        // Shoot sound
        if (currentGun.shootAudio && currentGun.shootClip)
            currentGun.shootAudio.PlayOneShot(currentGun.shootClip);

        // Raycast shooting
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            if (hit.collider.CompareTag("Zombie"))
            {
                if (bloodEffectPrefab)
                {
                    GameObject blood = Instantiate(bloodEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(blood, 5f);
                }

                //ZombieAI zombie = hit.collider.GetComponent<ZombieAI>();
                //if (zombie != null)
                //{
                //    zombie.TakeDamage(25f);
                //}
            }
            else
            {
                if (hitEffectPrefab)
                {
                    GameObject impact = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 5f);
                }
            }
        }

        UpdateAmmoUI();
    }

    IEnumerator Reload()
    {
        if (isReloading) yield break; // Prevent double reload

        isReloading = true;

        if (currentGun.gunAnimator)
            currentGun.gunAnimator.SetTrigger("Reload");

        if (currentGun.shootAudio && currentGun.reloadClip)
            currentGun.shootAudio.PlayOneShot(currentGun.reloadClip);

        yield return new WaitForSeconds(1.5f);

        int needed = currentGun.maxAmmo - currentGun.currentAmmo;
        int loadAmount = Mathf.Min(needed, currentGun.currentReserveAmmo);

        currentGun.currentAmmo += loadAmount;
        currentGun.currentReserveAmmo -= loadAmount;

        isReloading = false;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText && currentGun != null)
        {
            ammoText.text = $"{currentGun.gunName}: {currentGun.currentAmmo} / {currentGun.currentReserveAmmo}";
        }
    }

    public void AddAmmo(string weaponName, int amount)
    {
        foreach (var gun in allGuns)
        {
            if (gun.gunName == weaponName)
            {
                gun.currentReserveAmmo = Mathf.Min(gun.currentReserveAmmo + amount, gun.maxReserveAmmo);
                UpdateAmmoUI();
                return;
            }
        }
    }
}
