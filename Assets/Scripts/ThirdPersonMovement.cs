using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private Transform cam;
    private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    private float maxMoveSpeed;
    
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    
    private void Awake()
    {
        moveSpeed = 25;
        maxMoveSpeed = 4f;

        rb = GetComponent<Rigidbody>();
        rb.mass = 2;
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            if (rb.velocity.magnitude < maxMoveSpeed)
            {
                var moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
                rb.AddForce(moveDir.normalized * (moveSpeed * Time.fixedDeltaTime), ForceMode.VelocityChange); 
            }
        }
    }
}
