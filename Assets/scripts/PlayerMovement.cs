using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

        if(transform.position.y < -10)
        {
            //Mario should die
            MarioDie();
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
        if(collision.collider.tag == "EnemyHead")
        {
            //Kill the enemy
            Destroy(collision.gameObject);
        }

        else if(collision.collider.tag == "EnemyBody")
        {
            //Kill the player
            gameObject.SetActive(false);
            MarioDie();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            //Increase the coin count
            GetComponent<CoinWallet>().CoinsCount += 1;
            UIHandler.Instance.CoinsCount_Text.text = GetComponent<CoinWallet>().CoinsCount.ToString();
            collision.gameObject.SetActive(false);
        }

        if(collision.gameObject.tag == "castle")
        {
            //Move to next level
            UIHandler.Instance.WinPanel.gameObject.SetActive(true);
            this.enabled = false;
        }
    }

    void MarioDie()
    {
        //reload the current scence using scenemanager

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
