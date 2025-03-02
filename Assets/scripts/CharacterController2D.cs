using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;
    [SerializeField] private float m_FallMultiplier = 2.5f;                     // Faster fall speed
    [SerializeField] private float m_LowJumpMultiplier = 2f;                    // Short tap jump control
    [Range(0, 1)][SerializeField] private float m_AirControlFactor = 0.5f;     // Limited air movement
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;

    private const float k_GroundedRadius = .2f;
    private bool m_Grounded;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private bool m_FacingRight = true;  // Tracks facing direction

    [Header("Events")]
    public UnityEvent OnLandEvent;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        // Apply better gravity
        if (m_Rigidbody2D.velocity.y < 0)
        {
            m_Rigidbody2D.gravityScale = m_FallMultiplier;
        }
        else if (m_Rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            m_Rigidbody2D.gravityScale = m_LowJumpMultiplier;
        }
        else
        {
            m_Rigidbody2D.gravityScale = 1f; // Normal gravity when grounded or ascending
        }
    }

    public void Move(float move, bool jump)
    {
        if (m_Grounded || move != 0)
        {
            float controlFactor = m_Grounded ? 1f : m_AirControlFactor;
            Vector3 targetVelocity = new Vector2(move * 10f * controlFactor, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, 0.05f);
        }

        if (m_Grounded && jump)
        {
            m_Grounded = false;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce * Time.fixedDeltaTime);
        }

        // Handle player facing direction
        if (move > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (move < 0 && m_FacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
