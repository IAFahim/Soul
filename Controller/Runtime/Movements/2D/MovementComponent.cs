#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Soul.Controller.Runtime.Movements._2D
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 worldScale = new Vector2(1, 2 / 3f);
        [SerializeField] private float speed = 5;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private bool canMove = true;
        public Vector2 lastGroundedPosition;

        public Rigidbody2D rb;
        
        [SerializeField] private InputActionReference moveAction;
        public Vector2 direction;

        private void OnEnable()
        {
            TryEnableMoveAction();
        }

        private void OnDisable()
        {
            RemoveMoveAction();
        }
        
        public void AddMoveAction(InputActionReference inputActionReference)
        {
            TryDisableMoveAction();
            moveAction = inputActionReference;
            direction = Vector2.zero;
            TryEnableMoveAction();
        }

        public void RemoveMoveAction(bool zeroDirection = true)
        {
            TryDisableMoveAction();
            if (zeroDirection) direction = Vector2.zero;
        }
        
        public Vector2 SpeedVector => worldScale * speed;

        private void Move(Vector2 directionV2)
        {
            if (!canMove) return;
            rb.MovePosition(rb.position + directionV2 * (SpeedVector * Time.fixedDeltaTime));
        }

        private void FixedUpdate()
        {
            Move(direction);
            lastGroundedPosition = rb.position;
        }


        public void Move(InputAction.CallbackContext context) => direction = context.ReadValue<Vector2>();

        
        private void TryEnableMoveAction()
        {
            if (moveAction != null)
            {
                moveAction.action.Enable();
                moveAction.action.performed += Move;
                moveAction.action.canceled += Move;
            }
        }
        
        private void TryDisableMoveAction()
        {
            if (moveAction != null)
            {
                moveAction.action.Disable();
                moveAction.action.performed -= Move;
                moveAction.action.canceled -= Move;
                moveAction = null;
            }
        }
    }
}
#endif