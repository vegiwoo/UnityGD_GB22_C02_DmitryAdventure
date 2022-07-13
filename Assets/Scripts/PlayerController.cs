using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Refs:
 * - https://docs.unity3d.com/ru/2019.4/Manual/class-CharacterController.html
 * - https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
 * - https://youtu.be/SeBEvM2zMpY
 */

namespace DmitryAdventure
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Tooltip("Игровая камера")]
        private Camera gameCamera;
        
        [SerializeField, Tooltip("Скорость персонажа")]
        private float playerSpeed;
        
        [SerializeField, Tooltip("Скорость вращения"), Range(1f,5f)]
        private float rotationSpeed;
        
        [SerializeField, Tooltip("Высота прыжка персонажа")]
        private float jumpHeight;

        private const float GravityValue = -9.81f;

        private CharacterController controller;
        private PlayerInput playerInput;
        private Vector3 playerVelocity;
        private bool groundedPlayer;

        private InputAction moveAction;
        private InputAction jumpAction;
        
        private void Start()
        {
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];

            playerSpeed = 2f;
            jumpHeight = 0.7f;
            rotationSpeed = 4f;
        }

        private void Update()
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            var moveInput = moveAction.ReadValue<Vector2>();
            var move = new Vector3(moveInput.x, 0, moveInput.y);
            var camTransform = gameCamera.transform;
            move = move.x * camTransform.right.normalized + move.z * camTransform.forward.normalized;
            move.y = 0f;
            controller.Move(move * Time.deltaTime * playerSpeed);
            
            if (jumpAction.triggered && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * GravityValue);
            }

            playerVelocity.y += GravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
            
            // Rotate towards camera direction 
            var rotation = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}

