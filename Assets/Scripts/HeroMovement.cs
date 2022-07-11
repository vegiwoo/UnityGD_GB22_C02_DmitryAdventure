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
        power = 10;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
    private void FixedUpdate()
    {
        // Moving 
        var sideSpeed = Input.GetAxis("Horizontal") * power * Time.fixedDeltaTime;
        var forwardSpeed = Input.GetAxis("Vertical") * power * Time.fixedDeltaTime;
        var movingDirection = new Vector3(sideSpeed, 0, forwardSpeed);
        rigidbody.AddForce(movingDirection, ForceMode.VelocityChange);
        
        
        // Rotating
        if (Input.GetKey(KeyCode.E))
        {
            //rigidbody.constraints = RigidbodyConstraints.None;
            //rigidbody.MoveRotation(rigidbody.rotation * Quaternion.AngleAxis(10,Vector3.up));
            //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            transform.Rotate(transform.up, 15.0f, Space.Self);
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            //rigidbody.constraints = RigidbodyConstraints.None;
            //rigidbody.MoveRotation(rigidbody.rotation * Quaternion.AngleAxis(10,-Vector3.up));
            //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            //transform.rotation = transform.GetChild(0).transform.rotation;
            transform.Rotate(transform.up, -15.0f, Space.Self);
        }
    }
}