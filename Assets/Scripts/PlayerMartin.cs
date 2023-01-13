using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using Avatar = Alteruna.Avatar;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMartin : MonoBehaviour
{

    [SerializeField]
    private CharacterController controller;

    [SerializeField] private GameObject projectile;
    
    [SerializeField]
    private float speed = 10f;

    [SerializeField] private Avatar avatar;

    private Camera cam;
    
    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (!avatar.IsMe)
            return;
        
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
            Shoot();
    }


    private void Shoot()
    {
        Instantiate(projectile, transform.position + transform.forward, transform.rotation);
    }
}
