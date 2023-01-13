using System;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Vector3 = UnityEngine.Vector3;

public class PlayerMartin : MonoBehaviour
{
    [SerializeField] private Avatar avatar;
    
    private Camera cam;
    
    // Networking
    public Vector3 SynchronizedPosition;
    private Vector3 _oldSynchronizedPosition;
    
    // Movement
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 10f;
    private bool stunned = false;
    private float stunTime = 0.5f;
    
    // Attack    
    [SerializeField] private GameObject projectile;
    
    // Health
    private int health = 10;

    private void Awake()
    {
        cam = Camera.main;
    }
    
    private void Update()
    {
        if (!avatar.IsMe)
            return;
        
        if (!stunned)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * (Time.deltaTime * speed));
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + -transform.forward * 10, 0.5f);
            stunTime -= Time.deltaTime;
            if (stunTime <= 0)
            {
                stunned = false;
                stunTime = 2f;
            }
        }
        
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength = 500.0f;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);
 
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
        
        
    }
    
    private void Shoot()
    {
        Instantiate(projectile, transform.position + transform.forward, transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
            stunned = true;
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

    private void KnockBack(Vector3 direction)
    {
        
    }

    /*
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
    */
}
