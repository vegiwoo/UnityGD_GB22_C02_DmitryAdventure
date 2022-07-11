using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeroMovement : MonoBehaviour
{
    [SerializeField] private float power;
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = 5;
        power = 5;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
    private void FixedUpdate()
    {
        // Moving 
        var sideSpeed = Input.GetAxis("Horizontal") * power * Time.fixedDeltaTime;
        var forwardSpeed = Input.GetAxis("Vertical") * power * Time.fixedDeltaTime;
        var movement = new Vector3(sideSpeed, 0, forwardSpeed).normalized;
        
        if (movement == Vector3.zero) return;
        rigidbody.MovePosition(movement);

        // Rotating
        if (Input.GetKey(KeyCode.E))
        {
            rigidbody.MoveRotation(transform.rotation * Quaternion.AngleAxis(15,Vector3.up));
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            rigidbody.MoveRotation(transform.rotation * Quaternion.AngleAxis(-15,Vector3.up));
        }
    }
}