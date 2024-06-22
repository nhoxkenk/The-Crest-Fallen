using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> currentInteractableActions;
    [SerializeField] private ScriptableInputReader inputReader;

    private void Awake()
    {
        currentInteractableActions = new List<Interactable>();
    }

    private void OnEnable()
    {
        inputReader.Interact += HandleInteract;
    }

    private void OnDisable()
    {
        inputReader.Interact -= HandleInteract;
    }

    private void FixedUpdate()
    {
        if(!PlayerUI.Instance.menuWindowIsOpen && !PlayerUI.Instance.popUpWindowIsOpen)
        {
            CheckForInteraction();
        }
    }

    private void CheckForInteraction()
    {
        if(currentInteractableActions.Count == 0)
        {
            return;
        }

        if (currentInteractableActions[0] == null)
        {
            currentInteractableActions.RemoveAt(0);
            return;
        }

        if(currentInteractableActions[0] != null)
        {
            PlayerUI.Instance.playerUIPopup.SendMessageFromInteractToPlayer(currentInteractableActions[0].interactableMessage);
        }
    }

    private void RefreshInteractionList()
    {
        for(int i = currentInteractableActions.Count - 1; i >= 0 ; i--)
        {
            if(currentInteractableActions[i] == null)
            {
                currentInteractableActions.RemoveAt(i);
            }
        }
    }

    public void AddInteractionFromList(Interactable interactableObject)
    {
        RefreshInteractionList();
        if (!currentInteractableActions.Contains(interactableObject))
        {
            currentInteractableActions.Add(interactableObject);
        }
    }

    public void RemoveInteractionFromList(Interactable interactableObject)
    {
        if (currentInteractableActions.Contains(interactableObject))
        {
            currentInteractableActions.Remove(interactableObject);
        }
        RefreshInteractionList();
    }

    public void HandleInteract()
    {
        if(currentInteractableActions.Count == 0)
        {
            return;
        }

        if(currentInteractableActions[0] != null)
        {
            currentInteractableActions[0].Interact(this.GetComponent<PlayerManager>());
            RefreshInteractionList();
        }
    }
}
