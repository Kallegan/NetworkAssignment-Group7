using System;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Avatar avatar;
    [SerializeField] private CharacterController controller;

    private Rigidbody rb;
    
    [SerializeField] private float _moveSpeed = 10f;
    private float _curSpeed;
    
    [SerializeField] private float _friction = 10f;
    
    private Vector3 _moveDir;
    private Vector3 _velocity;
    
    private Camera cam;
    public bool canMove = true;
    
    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        _curSpeed = 0;
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
        
        _moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        controller.Move(_moveDir * _moveSpeed * Time.deltaTime);
        
        LookAtMouseWorldPos();
        /*
        _velocity += _moveDir * _moveSpeed * Time.deltaTime;
        _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, _friction * Time.deltaTime);
         controller.Move(_velocity * Time.deltaTime);
        */
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
