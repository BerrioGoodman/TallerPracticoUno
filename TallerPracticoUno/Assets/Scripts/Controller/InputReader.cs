using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action JumpEvent;
    public event Action JumpCancelledEvent;
    public event Action SprintEvent;
    public event Action SprintCancelledEvent;
    public event Action InteractEvent;
    public event Action InteractCancelledEvent;
    public event Action AttackEvent;
    public event Action AttackCancelledEvent;
    public event Action PauseEvent;
    public event Action ResumeEvent;
    private GameInput gameInput;
    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Gameplay.SetCallbacks(this);
            gameInput.UI.SetCallbacks(this);
            SetGameplay();
        }
    }
    private void OnDisable()
    {
        gameInput.Gameplay.Disable();
        gameInput.UI.Disable();
    }
    public void SetGameplay()
    {
        gameInput.Gameplay.Enable();
        gameInput.UI.Disable();
    }
    public void SetUI()
    {
        gameInput.UI.Enable();
        gameInput.Gameplay.Disable();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            AttackEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            AttackCancelledEvent?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            InteractCancelledEvent?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            JumpEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            JumpCancelledEvent?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
            SetUI();
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            ResumeEvent?.Invoke();
            SetGameplay();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SprintEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            SprintCancelledEvent?.Invoke();
        }
    }
}
