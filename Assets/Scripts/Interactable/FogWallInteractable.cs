using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FogWallInteractable : MonoBehaviour, IInteractable
{
    [field: SerializeField] public int ID { get; set; }

    [Header("Sub Components")]
    [SerializeField] private GameObject[] subComponents;

    [Header("Status")]
    [SerializeField] private bool isActive = true;
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
            OnIsActiveChanged?.Invoke(isActive);
        }
    }

    public event UnityAction<bool> OnIsActiveChanged = delegate { };

    private void OnEnable()
    {
        isActive = true;
        OnIsActiveChanged += HandleIsActiveChanged;
    }

    private void OnDisable()
    {
        OnIsActiveChanged -= HandleIsActiveChanged;
    }

    private void HandleIsActiveChanged(bool value)
    {
        foreach (var component in subComponents)
        {
            component.SetActive(value);
        }
    }

    private void Update()
    {
        Debug.Log(isActive);
        if(!isActive)
        {
            HandleIsActiveChanged(false);
        }
        else
        {
            HandleIsActiveChanged(true);
        }
    }
}
