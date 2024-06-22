using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [TextArea]
    public string interactableMessage;

    [SerializeField] private Collider interactableCollider;

    protected virtual void Awake()
    {
        if(interactableCollider == null)
        {
            interactableCollider = GetComponent<Collider>();
        }
    }

    protected virtual void Start()
    {
        
    }

    public virtual void Interact(PlayerManager character)
    {
        interactableCollider.enabled = false;
        character.playerInteraction.RemoveInteractionFromList(this);
        PlayerUI.Instance.playerUIPopup.ClosePopUpMessageFromInteract(); 
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        PlayerManager player;
        if(other.TryGetComponent<PlayerManager>(out player))
        {
            player.playerInteraction.AddInteractionFromList(this);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        PlayerManager player;
        if (other.TryGetComponent<PlayerManager>(out player))
        {
            player.playerInteraction.RemoveInteractionFromList(this);
            PlayerUI.Instance.playerUIPopup.ClosePopUpMessageFromInteract();
        }
    }
}
