using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMartin : Synchronizable
{
    [SerializeField] private Avatar avatar;
    
    private Camera cam;
    
    // Networking
    public Vector3 SynchronizedPosition;
    private Vector3 _oldSynchronizedPosition;
    
    // Movement
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 10f;
    
    // Attack    
    [SerializeField] private GameObject projectile;
    
    // Health
    private int health = 10;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        Multiplayer.RegisterRemoteProcedure("MyProcedureName", MyProcedureFunction);
    }
    
    // Local function, on every client
    void MyProcedureFunction(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        Debug.Log("Hello");
        Shoot();
    }
    
    void CallMyProcedure()
    {
        Multiplayer.InvokeRemoteProcedure("MyProcedureName", UserId.All);
    }
    
    private void Update()
    {
        // Networking
        SynchronizedPosition = transform.position;
        // If the value of our float has changed, sync it with the other players in our playroom.
        if (SynchronizedPosition != _oldSynchronizedPosition)
        {
            // Store the updated value
            _oldSynchronizedPosition = SynchronizedPosition;

            // Tell Alteruna that we want to commit our data.
            Commit();
        }
        // Update the Synchronizable
        base.SyncUpdate();

        if (!avatar.IsMe)
        {
            transform.position = SynchronizedPosition;
            return;
        }
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * (Time.deltaTime * speed));
        
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength = 500.0f;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);
 
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        if (Input.GetMouseButtonDown(0))
            CallMyProcedure();
    }
    
    private void Shoot()
    {
        Instantiate(projectile, transform.position + transform.forward, transform.rotation);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        // Update ui
    }

    public override void AssembleData(Writer writer, byte LOD)
    {
        // Write our data so that it can be sent to the other players in our playroom.
        writer.Write(SynchronizedPosition);
    }

    public override void DisassembleData(Reader reader, byte LOD)
    {
        // Set our data to the updated value we have recieved from another player.
        SynchronizedPosition = reader.ReadVector3();

        // Save the new data as our old data, otherwise we will immediatly think it changed again.
        _oldSynchronizedPosition = SynchronizedPosition;
    }
}
