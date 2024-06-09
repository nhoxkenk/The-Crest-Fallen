using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is suppose to be attached to the left and right hand of the model, so we can instantiate the weapon model to it
public class WeaponModelInstantiationSlot : MonoBehaviour
{
    public Transform parentOverride;
    public WeaponModelSlot weaponSlot;
    public GameObject currentWeaponModel;

    public void UnloadAndDestroyWeapon()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void UnloadWeapon()
    {
        if(currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    public void LoadWeaponModel(GameObject weaponModel)
    {
        //unload and destroy the previous weapon
        UnloadAndDestroyWeapon();

        if (weaponModel == null)
        {
            //unload the weapon
            UnloadWeapon();
            return;
        }

        currentWeaponModel = weaponModel;

        if (weaponModel != null)
        {
            if(parentOverride != null)
            {
                weaponModel.transform.SetParent(parentOverride, false);
            }
            else
            {
                weaponModel.transform.SetParent(transform, false);
            }

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
        
    }
}
