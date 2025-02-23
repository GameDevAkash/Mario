using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController2D controller2D;
    float horinzontalValue;
    public float horinzontalSpeed;
    public bool jump;
    [SerializeField] Animator m_animator;

    private void Awake()
    {
        controller2D = GetComponent<CharacterController2D>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horinzontalValue = Input.GetAxis("Horizontal");
        m_animator.SetFloat("Speed",Mathf.Abs(horinzontalValue));

        if (Input.GetKeyDown(KeyCode.W))
        {           
            jump = true; 
            m_animator.SetBool("Jump",jump);
        }

    }

    private void FixedUpdate()
    {
        if (controller2D != null)
        {
            controller2D.Move(horinzontalValue * horinzontalSpeed * Time.fixedDeltaTime, false, jump);
            jump = false;
        }
    }

    public void OnLand()
    {
        m_animator.SetBool("Jump",false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit "+ collision.gameObject.name);

        if (collision.collider.tag == "EnemyHead")
        {
            //Kill the enemy
            Debug.Log("Hit enemy in head");
            Destroy(collision.gameObject);
        }

        else if(collision.collider.tag == "EnemyBody")
        {
            //Kill the player/ourself
            gameObject.SetActive(false); Debug.Log("Hit enemy in body");
        }
    }
}
