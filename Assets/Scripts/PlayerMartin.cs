using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class PlayerMartin : MonoBehaviour
{

    [SerializeField]
    private CharacterController controller;
    
    [SerializeField]
    private float speed = 10f;
    private void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * (Time.deltaTime * speed));
    }
}
