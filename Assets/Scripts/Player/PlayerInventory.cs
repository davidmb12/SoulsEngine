using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

    public int currentRightWeaponIndex = 0;
    public int currentLefttWeaponIndex = 0;

    public List<WeaponItem> weaponsInventory;
    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

    }

    private void Start()
    {
        rightWeapon = weaponsInRightHandSlots[0];
        leftWeapon = weaponsInLeftHandSlots[0];
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }
    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;
        if (currentRightWeaponIndex <= weaponsInRightHandSlots.Length - 1)
        {

            if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (weaponsInRightHandSlots[currentRightWeaponIndex] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

        }
        else
        {
            currentRightWeaponIndex = -1;
            rightWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        }



    }
    public void ChangeLeftWeapon()
    {
        currentLefttWeaponIndex = currentLefttWeaponIndex + 1;
        if (currentLefttWeaponIndex <= weaponsInLeftHandSlots.Length - 1)
        {
            if (weaponsInLeftHandSlots[currentLefttWeaponIndex] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLefttWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLefttWeaponIndex], true);
            }
            else if (weaponsInLeftHandSlots[currentLefttWeaponIndex] == null)
            {
                currentLefttWeaponIndex = currentLefttWeaponIndex + 1;
            }
        }
        else
        {
            currentLefttWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
        }



    }
}
