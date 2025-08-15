using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // <-- Add this for the new Input System

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weapon_Manager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;
    private Animator zoomCameraAnimator;
    private bool isZoomedIn;

    private Camera mainCamera;

    private GameObject crosshair;

    private bool isAiming;

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab; // Prefab of the arrow to be instantiated

    [SerializeField]
    private Transform arrowSpawnPoint; // Point where the arrow will be spawned


    private void Awake()
    {
        weapon_Manager = GetComponent<WeaponManager>();

        zoomCameraAnimator = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();
        // mainCamera = Camera.main;
        crosshair = GameObject.FindGameObjectWithTag(Tags.CROSSHAIR);

        if (crosshair == null)
        {
            Debug.LogError("Crosshair not found in the scene!");
        }

        if (zoomCameraAnimator == null)
        {
            Debug.LogError("Zoom camera animator not found!");
        }

        mainCamera = Camera.main;

    }

    void Update()
    {
        ZoomIn(); // Call ZoomIn method to handle zooming
        WeaponShoot();
    }

    void WeaponShoot()
    {
        var weapon = weapon_Manager.GetCurrentSelectedWeapon();
        if (weapon == null)
        {
            Debug.LogWarning("No weapon selected!");
            return;
        }

        // For automatic fire
        if (weapon.fireType == WeaponFireType.MULTIPLE)
        {
            if (Mouse.current.leftButton.isPressed && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Debug.Log("Automatic fire!");
                weapon.ShootAnimation();
            }
        }
        else // For single fire
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextTimeToFire)
            {
                Debug.Log("Single fire!");
                if (weapon.tag == Tags.AXE_TAG)
                {
                    weapon.ShootAnimation();
                }

                if (weapon.bulletType == WeaponBulletType.BULLET)
                {
                    weapon.ShootAnimation();
                }
                else
                {
                    if (isAiming)
                    {
                        weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();

                        if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrowOrSpear(true);
                        }
                        else if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            ThrowArrowOrSpear(false);
                        }

                    }

                }
            }
        }
    }




    public void ZoomIn()
    {
        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1)) // Right mouse button for zooming in
            {
                zoomCameraAnimator.SetTrigger(AnimationTags.ZOOM_IN_ANIM);
                crosshair.SetActive(false);
                isZoomedIn = true;
            }

            if (Input.GetMouseButtonUp(1)) // Right mouse button for zooming in
            {
                zoomCameraAnimator.SetTrigger(AnimationTags.ZOOM_OUT_ANIM);
                crosshair.SetActive(true);
                isZoomedIn = false;
            }

        }


        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1)) // Right mouse button for zooming in
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);


                isAiming = true;
            }

            if (Input.GetMouseButtonUp(1)) // Right mouse button for zooming in
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
                isAiming = false;
            }
        }
        {

        }

    }


    void ThrowArrowOrSpear(bool ThrowArrow)
    {
        if (ThrowArrow)
        {
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = arrowSpawnPoint.position;

            arrow.GetComponent<ArrowBowScript>().ShootArrow(mainCamera);
        }
        else if (!ThrowArrow)
        {
            GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrowSpawnPoint.position;
            spear.GetComponent<ArrowBowScript>().ShootArrow(mainCamera);
        }
    }


    void BulletFired()
    {
        // This method can be used to handle any logic after a bullet is fired, if needed.
        Debug.Log("Bullet fired!");
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
            Debug.Log("Hit: " + hit.collider.name);
            // Here you can apply damage to the hit object if it has a health component
           /* var health = hit.collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }*/
        }
    }

}
