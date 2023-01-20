using System;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    private Avatar avatar;
    //[SerializeField] private CharacterController controller;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _friction = 10f;
    
    private Camera cam;
    private Rigidbody rb;
    
    private Vector3 _moveDir;
    private Vector3 _velocity;
    
    public bool stunned = false;
    private float stunTime;
    
    private void Awake()
    {
        cam = Camera.main;

        rb = GetComponent<Rigidbody>();
        transform.parent.GetComponent<Avatar>();
    }
    
    private void Update()
    {
        if (!avatar.IsMe) // ?
            return;
        /*
        _moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        _velocity += _moveDir * _moveSpeed * Time.deltaTime;
        _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, _friction * Time.deltaTime);
        controller.Move(_velocity * Time.deltaTime);
        */
        
        
        //controller.Move(_moveDir * _moveSpeed * Time.deltaTime);
        
        // LookRotation
        LookAtMouseWorldPos();
        
        // Stun
        if (!stunned) return;
        stunTime -= Time.deltaTime; 
        if (stunTime <= 0) 
            stunned = false;
    }

    public void SetAsStunned(float duration)
    {
        stunned = true;
        stunTime = duration;
    }
    
    private void FixedUpdate()
    {
        if (!avatar.IsMe) return;
        if (stunned) return;

        _moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        rb.AddForce(_moveDir * _moveSpeed);
    }
    
    private void LookAtMouseWorldPos()
    {
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength = 500.0f;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}
