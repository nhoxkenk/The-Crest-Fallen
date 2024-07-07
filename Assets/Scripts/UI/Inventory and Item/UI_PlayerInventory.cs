using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerInventory : MonoBehaviour
{
    public GameObject inventoryUI;

    public InventoryView inventoryView;

    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private ScriptableInputReader inputReader;
    [SerializeField] private Camera modelCamera;

    private void Awake()
    {
        inventory = PlayerManager.Instance.playerInventory;
        modelCamera.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        inputReader.OpenInventory += HandleOpenInventory;
        inventory.OnItemChangedCallback += UpdateUI;
    }

    private void OnDisable()
    {
        inputReader.OpenInventory -= HandleOpenInventory;
    }

    private void HandleOpenInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        modelCamera.gameObject.SetActive(!modelCamera.gameObject.activeSelf);
        PlayerManager.Instance.playerCombat.CanAttack = !inventoryUI.activeSelf;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < inventoryView.slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                inventoryView.slots[i].AddItemToSlot(inventory.items[i]);
            }
            else
            {
                inventoryView.slots[i].ClearSlot();
            }
        }
    }

}
