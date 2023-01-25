using System;
using Alteruna;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : Synchronizable
{
    private Avatar avatar;
    //[SerializeField] private CharacterController controller;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _friction = 10f;
    
    private Camera cam;
    private Rigidbody rb;
    
    private Vector3 _moveDir;
    private Vector3 _velocity;
    private float _velMagnitude;
    private float _oldVelMagnitude;
    
    public bool stunned = false;
    private float stunTime;
    
    private void Awake()
    {
        cam = Camera.main;

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _oldVelMagnitude = _velMagnitude;
        avatar = gameObject.GetComponentInParent(typeof(Alteruna.Avatar)) as Alteruna.Avatar;
    }
    
    
    private void Update()
    {
        if (!avatar.IsMe) // ?
            return;

        
        if (_oldVelMagnitude != _velMagnitude)
        {
            _oldVelMagnitude = _velMagnitude;
            Commit();
        }
        
        // LookRotation
        LookAtMouseWorldPos();
        
        SyncUpdate();
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

        _velMagnitude = rb.velocity.magnitude;
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
    
    public override void AssembleData(Writer writer, byte LOD = 100)
    {
      writer.Write(_velMagnitude);
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
        _velMagnitude = reader.ReadFloat();
        _oldVelMagnitude = _velMagnitude;
    }
}
