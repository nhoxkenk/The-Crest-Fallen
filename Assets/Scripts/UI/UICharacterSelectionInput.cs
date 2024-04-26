using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterSelectionInput : MonoBehaviour
{
    private PlayerControls playerControls;

    [Header("Title Screen Input")]
    [SerializeField] private bool isDeleteCharacterSlot = false;

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.UI.Enter.performed += i => isDeleteCharacterSlot = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        if(isDeleteCharacterSlot)
        {
            isDeleteCharacterSlot = false;
            TitleScreenManager.Instance.AttempToDeleteSlot();
        }
    }
}
