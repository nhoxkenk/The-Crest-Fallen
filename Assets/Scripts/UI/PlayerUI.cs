using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private static PlayerUI instance;
    public static PlayerUI Instance { get { return instance; } }

    [HideInInspector] public PlayerUIHud playerUIHud;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);    
        }

        playerUIHud = GetComponentInChildren<PlayerUIHud>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
