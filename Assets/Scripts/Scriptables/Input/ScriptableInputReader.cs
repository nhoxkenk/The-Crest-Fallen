using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerControls;
[CreateAssetMenu(menuName = "Input/Input Reader")]
public class ScriptableInputReader : ScriptableObject, ICameraActions, IPlayerActions
{
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction<Vector2, bool> Look = delegate { };
    public event UnityAction<bool> Jump = delegate { };
    public event UnityAction<bool> Dodge = delegate { };
    public event UnityAction<bool> Sprint = delegate { };
    public event UnityAction<bool> LockOn = delegate { };
    public event UnityAction<bool> LightAttack = delegate { };
    public event UnityAction<bool> HeavyAttack = delegate { };
    public event UnityAction<bool> ChargeAttack = delegate { };
    public event UnityAction SwitchRightWeapon = delegate { };
    public event UnityAction SwitchLeftWeapon = delegate { };
    public event UnityAction Interact = delegate { };
    public event UnityAction DrinkEstusFlask = delegate { };
    public event UnityAction OpenInventory = delegate { };

    private PlayerControls playerControls;

    public Vector3 MoveDirection => playerControls.Player.Movement.ReadValue<Vector2>();
    public Vector3 LookDirection => playerControls.Camera.Look.ReadValue<Vector2>();
    public bool IsSprinting { get; private set; } = false;
    public bool IsChargingAttack { get; private set; } = false;

    public float MovementAmount 
    {
        get
        {
            return Mathf.Clamp01(Mathf.Abs(MoveDirection.y) + Mathf.Abs(MoveDirection.x));
        }
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.SetCallbacks(this);
            playerControls.Camera.SetCallbacks(this);
        }
        playerControls.Enable();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Dodge.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                Dodge.Invoke(false);
                break;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Jump.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                Jump.Invoke(false);
                break;
        }
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                LightAttack.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                LightAttack.Invoke(false);
                break;
        }
    }

    public void OnLockOn(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                LockOn.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                LockOn.Invoke(false);
                break;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
    }

    private bool IsDeviceMouse(InputAction.CallbackContext context)
    {
        return context.control.device.name == "Mouse";
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Move.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                IsSprinting = true;
                Sprint.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                IsSprinting = false;
                Sprint.Invoke(false);
                break;
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                HeavyAttack.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                HeavyAttack.Invoke(false);
                break;
        }
    }

    public void OnChargeAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                IsChargingAttack = true;
                ChargeAttack.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                IsChargingAttack = false;
                ChargeAttack.Invoke(false);
                break;
        }
    }

    public void OnSwitchRightWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            SwitchRightWeapon?.Invoke();
        }
    }

    public void OnSwitchLeftWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            SwitchLeftWeapon?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Interact?.Invoke();
        }
    }

    public void OnDrinkEstusFlask(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            DrinkEstusFlask?.Invoke();
        }
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OpenInventory?.Invoke();
        }
    }
}
