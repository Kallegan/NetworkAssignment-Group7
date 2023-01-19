using System;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Avatar avatar;
    
    private Camera cam;
    
    // Movement
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float stunTime = 0.75f;
    private float curStunTime;
    private bool canMove = true;
    
    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        curStunTime = stunTime;
    }

    private void Update()
    {
        if (!avatar.IsMe)
            return;
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        
        if (canMove)
            controller.Move(move * (Time.deltaTime * speed));
        else
        {
            curStunTime -= Time.deltaTime;
            if (curStunTime <= 0)
                canMove = true;
        }
        
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength = 500.0f;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    public void Stun(float time)
    {
        stunTime = time;
        canMove = false;
    }
}
