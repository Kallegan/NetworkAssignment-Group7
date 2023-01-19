using System;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Avatar avatar;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed = 10f;
    
    private Camera cam;
    private bool canMove = true;
    
    private void Awake()
    {
        cam = Camera.main;
        
    }
    
    private void Update()
    {
        if (!avatar.IsMe)
            return;
        
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (canMove)
            controller.Move(moveDir * Time.deltaTime * moveSpeed);
        
        LookAtMouseWorldPos();
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
