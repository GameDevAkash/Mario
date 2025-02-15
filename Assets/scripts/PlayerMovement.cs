using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    //public float runSpeed = 40f; // Speed multiplier for horizontal movement
    //private CharacterController2D controller;
    //private float horizontalMove = 0f;
    //private bool jump = false;
    //private bool crouch = false;

    //private void Awake()
    //{
    //    controller = GetComponent<CharacterController2D>();
    //}

    //void Update()
    //{
    //    // Get horizontal movement input (-1 to 1 for left/right movement)
    //    horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

    //    // Check for jump input
    //    if (Input.GetButtonDown("Jump"))
    //    {
    //        jump = true;
    //    }

    //    // Check for crouch input
    //    if (Input.GetButtonDown("Crouch"))
    //    {
    //        crouch = true;
    //    }
    //    else if (Input.GetButtonUp("Crouch"))
    //    {
    //        crouch = false;
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    // Move the player using CharacterController2D's Move method
    //    controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
    //    jump = false; // Reset jump after applying it
    //}

    [SerializeField] CharacterController2D controller2D;
    float horinzontalValue;
    public float horinzontalSpeed;
    public bool jump;

    private void Awake()
    {
        controller2D = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        horinzontalValue = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W)) { jump = true; }

    }

    private void FixedUpdate()
    {
        if (controller2D != null)
        {
            controller2D.Move(horinzontalValue * horinzontalSpeed * Time.fixedDeltaTime, false, jump);
            jump = false;
        }
    }
}
