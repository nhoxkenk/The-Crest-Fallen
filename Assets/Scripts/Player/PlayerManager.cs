using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerManager>();
            }
            return instance;
        }
    }

    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerLocomotion playerLocomotion;
    [HideInInspector] public PlayerAnimator playerAnimator;
    [HideInInspector] public PlayerInventory playerInventory;
    [HideInInspector] public PlayerStat playerStat;

   protected override void Awake()
    {
        base.Awake();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerInput = GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStat = GetComponent<PlayerStat>();

        LoadGameDataToCurrentCharacterData(ref WorldSaveManager.Instance.currentCharacterData);
    }

    protected override void Update()
    {
        base.Update();
        playerLocomotion.HandleAllMovement();

        playerStat.RegenerateStamina();
    }

    private void LateUpdate()
    {
        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterSaveData)
    {
        currentCharacterSaveData.characterXPosition = transform.position.x;
        currentCharacterSaveData.characterYPosition = transform.position.y;
        currentCharacterSaveData.characterZPosition = transform.position.z;
    }

    public void LoadGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterSaveData)
    {
        Vector3 currentPosition = new Vector3(currentCharacterSaveData.characterXPosition, currentCharacterSaveData.characterYPosition, currentCharacterSaveData.characterZPosition);
        transform.position = currentPosition;
    }
}
