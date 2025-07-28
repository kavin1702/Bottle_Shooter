
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float runSpeed = 9f;

    [Header("Running")]
    public bool canRun = true;
    public KeyCode runningKey = KeyCode.LeftShift;
    public bool IsRunning { get; private set; }

    [Header("Animator")]
    public Animator weaponAnimator; // Assign your FPS weapon (FpsAnims) animator here

    [Header("Optional")]
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    private Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Check if running
        IsRunning = canRun && Input.GetKey(runningKey);

        // Determine speed
        float targetSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 input = new Vector2(horizontal, vertical);
        Vector2 moveDirection = input.normalized * targetSpeed;

        // Apply movement
        Vector3 velocity = transform.rotation * new Vector3(moveDirection.x, rigidbody.linearVelocity.y, moveDirection.y);
        rigidbody.linearVelocity = velocity;

        // Update Animator (Speed and Running)
        if (weaponAnimator != null)
        {
            weaponAnimator.SetFloat("Speed", input.magnitude); // For blend tree
            weaponAnimator.SetBool("IsRunning", IsRunning);
        }
    }

    void Update()
    {
        // Handle reload trigger
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (weaponAnimator != null)
            {
                weaponAnimator.SetTrigger("Reload");
            }
        }
    }
}
