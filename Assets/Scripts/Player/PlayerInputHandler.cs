using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerInputHandler : MonoBehaviour
    {   
        public event Action OnJumped;
        public event Action OnJumpHolded;
        public event Action OnJumpReleased; 
        public event Action OnDashPressed;     
        public event Action<StateColor> OnColorChange;  
       
        public float MoveAxis { get; private set; }
        public float VerticalAxis { get; private set; } 

        public bool IsInteract { get; private set; }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveAxis = context.ReadValue<float>();
        }
        public void OnVertical(InputAction.CallbackContext context)
        {
            VerticalAxis = context.ReadValue<float>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnJumped?.Invoke();
            }
            else if (context.performed)
            {
                OnJumpHolded?.Invoke();
            }
            else if (context.canceled)
            {
                OnJumpReleased?.Invoke();
            }
        }

        public void HandleColorInput(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            string controlName = context.control.name;

            switch (controlName)
            {
                case "r": OnColorChange?.Invoke(StateColor.red); break;
                case "g": OnColorChange?.Invoke(StateColor.green); break;
                case "b": OnColorChange?.Invoke(StateColor.blue); break;
            }
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                IsInteract = true;
            }

            if (context.performed)
            {
                IsInteract = true;
            }

            if (context.canceled)
            {
                IsInteract = false;
            }
        }

        public void Dash(InputAction.CallbackContext context)
        {
            if (context.started)
                OnDashPressed?.Invoke();
        }
    }
}

