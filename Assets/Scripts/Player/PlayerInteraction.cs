using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> currentInteractableActions = new List<Interactable>();
    [SerializeField] private ScriptableInputReader inputReader;
    private Interactable currentInteractableIsDisplay;

    private void OnEnable()
    {
        inputReader.Interact += HandleInteract;
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

        for (int i = 0; i < currentInteractableActions.Count; i++) 
        {
            if (currentInteractableActions[i] == null)
            {
                currentInteractableActions.RemoveAt(0);
            }
            else
            {
                PlayerUI.Instance.playerUIPopup.SendMessageFromInteractToPlayer(currentInteractableActions[i].interactableMessage);
                currentInteractableIsDisplay = currentInteractableActions[i];
            }
            return;
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
        if(currentInteractableIsDisplay != null)
        {
            currentInteractableIsDisplay.Interact(this.GetComponent<PlayerManager>());
        }
        Debug.Log("Press");
    }
}
