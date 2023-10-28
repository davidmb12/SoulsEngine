using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HandEquipmentSlotUI : MonoBehaviour
{
    UIManager uiManager;
    public Image icon;
    WeaponItem weapon;

    [SerializeField]
    public bool rightHandSlot01;
    [SerializeField]
    public bool rightHandSlot02;
    [SerializeField]
    public bool leftHandSlot01;
    [SerializeField]
    public bool leftHandSlot02;

    Color transparentColor = new Color(1, 1, 1, 0);
    Color whiteColor = new Color(1, 1, 1, 1);
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    public void AddItem(WeaponItem n_weapon)
    {
        gameObject.SetActive(true);
        icon.color = whiteColor;
        weapon = n_weapon;
        icon.sprite = weapon.itemIcon;
    }

    public void ClearItem()
    {
        weapon = null;
        icon.sprite = null;
        icon.color = transparentColor;
        gameObject.SetActive(false);
    }

    public void SelectThisSlot()
    {
        if (rightHandSlot01)
        {
            uiManager.rightHandSlot01Selected = true;
        }
        else if (rightHandSlot02)
        {
            uiManager.rightHandSlot02Selected = true;

        }
        else if (leftHandSlot01)
        {
            uiManager.leftHandSlot01Selected = true;
        }
        else
        {
            uiManager.leftHandSlot02Selected = true;

        }
    }
}
