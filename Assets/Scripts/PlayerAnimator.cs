using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna.Trinity;
using Alteruna;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] float MovementSmoothing;

    [SerializeField] private Animator anim;
    private CharacterController charController;

    private float smoothVelocityMagnitude;

    private Vector3 lastPosition;
    private float velocityDelta;
    private float prevDelta;
    private float smoothVelocityDelta;

    // Start is called before the first frame update
    void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positionDelta = transform.position - lastPosition;
        velocityDelta = positionDelta.magnitude;
        lastPosition = transform.position;

        float SmoothedVelocity = Mathf.SmoothDamp(prevDelta, velocityDelta, ref smoothVelocityDelta, MovementSmoothing);
        anim.SetFloat("VelocityMagnitude", SmoothedVelocity);
        Debug.Log("Smoothed Velocity" + SmoothedVelocity);

        prevDelta = velocityDelta;
        

       
    }
}
