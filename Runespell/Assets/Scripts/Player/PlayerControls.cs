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

    // gets data from the virtual Joystick
    [SerializeField]
    private VirtualJoystick virtualJoystick;

    private Vector2 movement;
    private Rigidbody2D rb;

    // Variables for dashing
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashingSpeed = 25.0f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 10.0f;

    [SerializeField]
    private TrailRenderer tr;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool casting;

    private Vector2 screenBounds;
    private float playerHalfWidth;
    private float playerHalfHeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr.emitting = false;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        playerHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x / 2;
        playerHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y / 2;
    }

    private void OnMovement(InputValue value)
    {
        //movement = value.Get<Vector2>();
        movement = virtualJoystick.JoystickInput;
    }

    private IEnumerator OnDash()
    {
        canDash = false;
        isDashing = true;
        tr.emitting = true;

        // Calculate the dash direction and target position
        Vector2 dashDirection = new Vector2(movement.x, movement.y).normalized;
        Vector2 startPosition = rb.position;
        Vector2 targetPosition = startPosition + dashDirection * dashingSpeed;

        // Clamps the target position to screen bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, -screenBounds.x + playerHalfWidth, screenBounds.x - playerHalfWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -screenBounds.y + playerHalfHeight, screenBounds.y - playerHalfHeight);

        // Smoothly move towards the target position
        // TODO: Switch from a Lerp to something else so that it doesn't abrubtly stop you
        float elapsedTime = 0f;
        while (elapsedTime < dashingTime)
        {
            rb.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsedTime / dashingTime));
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        rb.MovePosition(targetPosition);

        yield return new WaitForSeconds(dashingTime); 
        tr.emitting = false;
        isDashing = false;

        // Wait for the cooldown before allowing another dash
        // TODO: Make this work
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true; 
    }

    private void Animations()
    {
        if (movement.x > 0)
        {
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingUp", false);
            animator.SetBool("WalkingDown", false);
            animator.SetBool("WalkingRight", true);
        }
        else if (movement.x < 0)
        {
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingUp", false);
            animator.SetBool("WalkingDown", false);
            animator.SetBool("WalkingLeft", true);
        }
        else if (movement.y > 0)
        {
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingDown", false);
            animator.SetBool("WalkingUp", true);
        }
        else if (movement.y < 0)
        {
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingUp", false);
            animator.SetBool("WalkingDown", true);
        }
        else
        {
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingUp", false);
            animator.SetBool("WalkingDown", false);
        }
        if (casting)
        {
            animator.SetTrigger("Casting");
            casting = false;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(OnDash());
        }

        // Clamp position before applying movement
        float clampedX = Mathf.Clamp(rb.position.x, -screenBounds.x + playerHalfWidth, screenBounds.x - playerHalfWidth);
        float clampedY = Mathf.Clamp(rb.position.y, -screenBounds.y + playerHalfHeight, screenBounds.y - playerHalfHeight);
        Vector2 clampedPosition = new Vector2(clampedX, clampedY);

        // Comment out the below line to disable mobile joystick
        movement = virtualJoystick.JoystickInput;

        rb.MovePosition(clampedPosition + movement * speed * Time.fixedDeltaTime);

        Animations();
    }

    public bool Casting 
    { get { return casting; } set {  casting = value; } }
}
