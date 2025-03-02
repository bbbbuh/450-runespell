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
     

    private Vector2 touchStartPosition;
    private Vector2 touchEndPosition;

    [SerializeField]
    private Vector2 positionDifference;
    //  This is the distance needed before the code recognizes a swipe as a dash
    private float swipeThreshold = 50.0f;

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

    private IEnumerator OnDash(Vector2 dashDirection)
    {
        canDash = false;
        isDashing = true;
        tr.emitting = true;

        SoundManager.instance.PlaySoundEffect(SoundEffectNames.PlayerDash);

        // Calculate the dash direction and target position
        //Vector2 dashDirection = new Vector2(movement.x, movement.y).normalized;
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
        if (Mathf.Abs(positionDifference.x) > Mathf.Abs(positionDifference.y))
        {
            if (positionDifference.x < 0)
            {
                animator.SetBool("WalkingLeft", false);
                animator.SetBool("WalkingUp", false);
                animator.SetBool("WalkingDown", false);
                animator.SetBool("WalkingRight", true);
            }
            else if (positionDifference.x > 0)
            {
                animator.SetBool("WalkingRight", false);
                animator.SetBool("WalkingUp", false);
                animator.SetBool("WalkingDown", false);
                animator.SetBool("WalkingLeft", true);
            }
        }
        else
        {
            if (positionDifference.y < 0)
            {
                animator.SetBool("WalkingLeft", false);
                animator.SetBool("WalkingRight", false);
                animator.SetBool("WalkingDown", false);
                animator.SetBool("WalkingUp", true);
            }
            else if (positionDifference.y > 0)
            {
                animator.SetBool("WalkingLeft", false);
                animator.SetBool("WalkingRight", false);
                animator.SetBool("WalkingUp", false);
                animator.SetBool("WalkingDown", true);
            }
        }
    }

    private void DetectSwipe()
    {
        // Use the mouse's right button and position to simulate touch if no touchscreen is available
        bool isTouchScreenAvailable = Touchscreen.current != null;

        // Simulate touch using the mouse
        Vector2 touchPosition = isTouchScreenAvailable ? Touchscreen.current.primaryTouch.position.ReadValue() : Mouse.current.position.ReadValue();

        if (!isTouchScreenAvailable)
        {
            if (Mouse.current.rightButton.isPressed)
            {
                // Detect start of swipe (mouse button down)
                if (touchStartPosition == Vector2.zero)  
                {
                    touchStartPosition = touchPosition;
                }
            }

            // Detect end of swipe (mouse button up)
            if (Mouse.current.rightButton.wasReleasedThisFrame && touchStartPosition != Vector2.zero)
            {
                touchEndPosition = touchPosition;
                Vector2 swipeDelta = touchEndPosition - touchStartPosition;

                // Check if the swipe distance exceeds the threshold
                if (swipeDelta.magnitude > swipeThreshold)
                {
                    Vector2 dashDirection = swipeDelta.normalized;
                    if (canDash)
                    {
                        StartCoroutine(OnDash(dashDirection));
                    }
                }

                touchStartPosition = Vector2.zero; // Reset after swipe
            }
        }
        else
        {
            // Normal swipe handling if touchscreen is available
            if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                touchStartPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }

            if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                touchEndPosition = Touchscreen.current.primaryTouch.position.ReadValue();

                Vector2 swipeDelta = touchEndPosition - touchStartPosition;

                if (swipeDelta.magnitude > swipeThreshold)
                {
                    Vector2 dashDirection = swipeDelta.normalized;
                    if (canDash)
                    {
                        StartCoroutine(OnDash(dashDirection));
                    }
                }
            }
        }
    }



    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        //if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        //{
        //    StartCoroutine(OnDash());
        //}

        DetectSwipe();

        // Clamp position before applying movement
        float clampedX = Mathf.Clamp(rb.position.x, -2.5f + playerHalfWidth, 2.5f - playerHalfWidth);
        float clampedY = Mathf.Clamp(rb.position.y, -4.5f + playerHalfHeight, 4.5f - playerHalfHeight);
        Vector2 clampedPosition = new Vector2(clampedX, clampedY);

        // Comment out the below line to disable mobile joystick
        movement = virtualJoystick.JoystickInput;

        Vector2 newPosition = clampedPosition + movement * speed * Time.fixedDeltaTime;

        if (newPosition != rb.position)
        {
            SoundManager.instance.PlayWalk();
        }
        else
        {
            SoundManager.instance.StopWalk();
        }

        positionDifference = rb.position - newPosition;
        rb.MovePosition(newPosition);

        Animations();
    }

    public bool Casting 
    { get { return casting; } set {  casting = value; } }
}
