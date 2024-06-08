using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private static PlayerUI instance;
    public static PlayerUI Instance { get { return instance; } }

    [HideInInspector] public PlayerUIHud playerUIHud;
    [HideInInspector] public PlayerUIPopup playerUIPopup;

    [Header("UI Flags")]
    public bool menuWindowIsOpen;       //Inventory, Equipment, BlackSmith, ...
    public bool popUpWindowIsOpen;      //Item pick up, Dialogue pop up, ...

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);    
        }

        playerUIHud = GetComponentInChildren<PlayerUIHud>();
        playerUIPopup = GetComponentInChildren<PlayerUIPopup>();
    }
}
