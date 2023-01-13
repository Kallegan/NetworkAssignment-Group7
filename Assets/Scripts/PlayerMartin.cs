using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public class PlayerMartin : MonoBehaviour
{

    [SerializeField]
    private CharacterController controller;
    
    [SerializeField]
    private float speed = 10f;

    [SerializeField] private Avatar avatar;
    private void Update()
    {
        if (!avatar.IsMe)
            return;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * (Time.deltaTime * speed));
    }
}
