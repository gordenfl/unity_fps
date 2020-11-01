using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private WeaponHandler[] weapons;

    private int curWeaponIdx;
    // Start is called before the first frame update
    void Start()
    {
        curWeaponIdx = 0;
        weapons[curWeaponIdx].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOnSelectedWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOnSelectedWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TurnOnSelectedWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TurnOnSelectedWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TurnOnSelectedWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TurnOnSelectedWeapon(5);
        }
    } // update

    void TurnOnSelectedWeapon(int weaponIdx)
    {
        if (curWeaponIdx == weaponIdx)
            return;
            
        weapons[curWeaponIdx].gameObject.SetActive(false);
        weapons[weaponIdx].gameObject.SetActive(true);
        curWeaponIdx = weaponIdx;
    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
        return weapons[curWeaponIdx];
    }
} // class
