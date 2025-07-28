using UnityEngine;

public class GunSwitcher : MonoBehaviour
{
    [Header("Gun References")]
    public GameObject[] guns;

    [Header("Gun Manager")]
    public RaycastGunManager gunManager;

    private int currentGunIndex = 0;
  //  private GrenadeThrower currentGrenadeThrower;

    void Start()
    {
        ActivateGun(currentGunIndex);
    }

    void Update()
    {
        // Number keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SwitchTo(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SwitchTo(1); }
        //if (Input.GetKeyDown(KeyCode.Alpha3)) { SwitchTo(2); } // grenade slot

        // Mouse Scroll
        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0f) SwitchTo((currentGunIndex + 1) % guns.Length);
        else if (scroll < 0f) SwitchTo((currentGunIndex - 1 + guns.Length) % guns.Length);

        // Only allow throwing when grenade is selected
        //if (currentGrenadeThrower != null && Input.GetButtonDown("Fire1"))
        //{
        //    currentGrenadeThrower.ManualThrow();
        //}
    }

    void SwitchTo(int index)
    {
        currentGunIndex = index;
        ActivateGun(currentGunIndex);
    }

    void ActivateGun(int index)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(i == index);
        }

        Debug.Log("Switched to weapon: " + guns[index].name);

        if (gunManager != null)
            gunManager.EquipGun(index);

        // Check if this weapon has a grenade thrower
        //currentGrenadeThrower = guns[index].GetComponent<GrenadeThrower>();
    }
}
