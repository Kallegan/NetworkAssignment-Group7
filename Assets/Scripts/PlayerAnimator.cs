using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna.Trinity;
using Alteruna;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] float MovementSmoothing;

    [SerializeField] private Animator anim;
    private RigidbodySynchronizable rbSync;
    private Rigidbody rb;


    private Vector3 lastPosition;
    private float velocityDelta;
    private float prevDelta;
    private float smoothVelocityDelta;

    private float prevAngle;
    private float turnAngle;
    private float smoothedTurnAngle;

    [SerializeField] float Coefficent = 1;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //rbSync = GetComponent<RigidbodySynchronizable>();
    }

    private void Start()
    {
        //Bind event to play spell cast animation here
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 positionDelta = transform.position - lastPosition;
        velocityDelta = positionDelta.magnitude;
        lastPosition = transform.position;

        

        //prevDelta = velocityDelta;
        prevDelta = prevDelta * 0.2f + velocityDelta * 0.8f;

        
        prevAngle = turnAngle;

        turnAngle = CalculateDirection(positionDelta);
        Debug.Log("Strafe Direction" + turnAngle);
        Debug.Log("Velocity Magnitude: " + velocityDelta);

        anim.SetFloat("VelocityMagnitude", velocityDelta * Coefficent);

        anim.SetFloat("StrafeX", turnAngle);

    }

    float CalculateDirection(Vector3 velocity)
    {
        if (velocity.magnitude > 0.01)
        {
            float ForwardCosAngle = Vector3.Dot(transform.forward, velocity.normalized);
            float ForwardDeltaDegree = Mathf.Rad2Deg * Mathf.Acos(ForwardCosAngle);

            float RightCosangle = Vector3.Dot(transform.right, velocity.normalized);
            if (RightCosangle < 0)
            {
                ForwardDeltaDegree *= -1;
            }

            return ForwardDeltaDegree;
        }
        return 0.0f;
    }

    private void LateUpdate()
    {
        

        //This is jittery
        float SmoothedVelocity = Mathf.SmoothDamp(prevDelta, velocityDelta, ref smoothVelocityDelta, MovementSmoothing, 0.01f, Time.deltaTime);

        float SmoothedTurn = Mathf.SmoothDampAngle(prevAngle, turnAngle, ref smoothedTurnAngle, 0.01f);
        
        
    }

    //Plays animation once on action layer. Like an Unreal Engine montage
    private void PlayAction(string ActionName) 
    {
        anim.Play(ActionName, 1, -1);
    }
}
