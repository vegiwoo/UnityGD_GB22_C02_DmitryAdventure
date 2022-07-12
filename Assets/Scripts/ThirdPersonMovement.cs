using System;
using DmitryAdventure;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private Transform cam;
    
    [Tooltip("Скорость персонажа")]
    [SerializeField] private float moveSpeed;
 
    [Tooltip("Максимально допустимое ускорение персонажа")]
    [SerializeField, Range(4f,6f)] private float maxVelocity;
    
    [Tooltip("Коэффициент сглаживания угла поворота")]
    [SerializeField, Range(0.1f,1.0f)] private float turnSmoothTime;
    private float turnSmoothVelocity;
    
    private Rigidbody playerRigidbody;
    
    private void Awake()
    {
        moveSpeed = 25;
        turnSmoothTime = 0.65f;
        maxVelocity = 4f;

        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.mass = 2;

    }

    private void FixedUpdate()
    {
        // TODO: Вынести в InputController
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            if (playerRigidbody.velocity.magnitude < maxVelocity)
            {
                var moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                playerRigidbody.AddForce(moveDir.normalized * (moveSpeed * Time.fixedDeltaTime), ForceMode.VelocityChange); 
            }
        }
    }
}
