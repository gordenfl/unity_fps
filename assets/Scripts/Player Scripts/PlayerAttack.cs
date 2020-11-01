using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager weaponManager;
    [SerializeField]
    public float fire_rate = 1f;

    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;
    private Camera mainCam;
    private GameObject crosshair;

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;

    [SerializeField]
    private Transform arrow_bow_startPosition;
    private bool isAming;
    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CANERA).GetComponent<Animator>();
        crosshair = GameObject.Find(Tags.CROSSHAIR);
        mainCam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot()
    {
        if (weaponManager.GetCurrentSelectedWeapon().fire_type == WeaponFireType.MULTIPLE)
        {
            // Multiple fire weapon
            //Time.time 我们开游戏以来的时间
            //这里的逻辑是用nextTimeToFire 来控制是否能够Fire
            //GetMouseButtonDown  0 代表主要键，1 代表右键，2 代表中间键
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1.0f / fire_rate;
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                BulletFired();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (weaponManager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                }
                if (weaponManager.GetCurrentSelectedWeapon().bullet_type == WeaponBulletType.BULLET)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                }
                else
                {
                    //Here we have an arrow or spear
                    //weaponManager.GetCurrentSelectedWeapon()
                    if (isAming)
                    {
                        weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                        if (weaponManager.GetCurrentSelectedWeapon().bullet_type == WeaponBulletType.ARROW)
                        {
                            ThrowArrowAndSpear(true);

                        }
                        else if (weaponManager.GetCurrentSelectedWeapon().bullet_type == WeaponBulletType.SPEAR)
                        {
                            ThrowArrowAndSpear(false);
                        }
                    }
                }
            }
        }
    }

    void ZoomInAndOut()
    {
        if (weaponManager.GetCurrentSelectedWeapon().weapon_aim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOMIN_ANIM);
                crosshair.SetActive(false);
            }
            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOMOUT_ANIM);
                crosshair.SetActive(true);
            }
        }

        if (weaponManager.GetCurrentSelectedWeapon().weapon_aim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(true);
                isAming = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(false);
                isAming = false;
            }
        }
    }

    void ThrowArrowAndSpear(bool isArrow)
    {
        weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
        if (isArrow)
        {
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = this.arrow_bow_startPosition.position;
            arrow.GetComponent<ArrowAndBowScripts>().Launch(mainCam);
        }
        else
        { //throw Spear
            GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrow_bow_startPosition.position;
            spear.GetComponent<ArrowAndBowScripts>().Launch(mainCam);
        }

    }

    void BulletFired()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            print("We hit:" + hit.transform.gameObject.name);
            //shoot bullet hit something.
        }
        else
        {
            print("Physics return False");
        }
    }
}
