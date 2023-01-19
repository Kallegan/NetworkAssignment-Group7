using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField, Range(0, 10)] float MovementSmoothing;

    [SerializeField] private Animator anim;
    private CharacterController charController;

    private float SmoothVelocityMagnitude;

    // Start is called before the first frame update
    void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.SmoothDamp(SmoothVelocityMagnitude, charController.velocity.magnitude, ref SmoothVelocityMagnitude, 1);
        anim.SetFloat("VelocityMagnitude", SmoothVelocityMagnitude);

        Debug.Log(charController.velocity.magnitude);
    }
}
