using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    private Vector2 movement;
    private Rigidbody2D rb;

    // Variables for dashing
    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField] private float dashingSpeed = 25.0f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 10.0f;

    [SerializeField]
    private TrailRenderer tr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr.emitting = false;
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    private IEnumerator OnDash()
    {
        canDash = false; // Disable dashing during the dash
        isDashing = true;

        rb.velocity = new Vector2(movement.x * dashingSpeed, movement.y * dashingSpeed);
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime); // Dash duration
        tr.emitting = false;
        isDashing = false;

        // Wait for the cooldown before allowing another dash
        // This doesn't work for some reason
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true; // Re-enable dashing after cooldown
    }

    private void FixedUpdate()
    {
        // This prevents you from moving if you are currently dashing
        if (isDashing)
        {
            return;
        }

        // Check if the dash key is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(OnDash());
        }


        // Most basic movement code
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);



        // This code uses velocity to move
        // It is slowed down by the Linear Drag variable of the Rigidbody 2D
        // The lower the Drag, the less friction there is, and the longer it
        // takes to slow down the movement after you release.

        // if (movement.x != 0 || movement.y != 0)
        // {
        //     rb.velocity = movement * speed;
        // }



        // This code uses forces to move.
        // To use it, you have to increase the speed by a lot
        // Mess around with the speed and Linear Drag variables
        // Until the movement feels good

        // rb.AddForce(movement * speed);
    }
}
