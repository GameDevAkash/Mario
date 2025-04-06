using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController2D controller2D;
    float horinzontalValue;
    public float horinzontalSpeed;
    public bool jump;
    [SerializeField] Animator m_animator, small_Animator, big_Animator;
    public bool CanMove;
    public SpriteRenderer Big_Renderer;
    public SpriteRenderer Small_Renderer;
    public CapsuleCollider2D m_collider;

    public bool Big => Big_Renderer.enabled;
    public bool Small => Small_Renderer.enabled;

    public AudioSource m_audioSource;
    public MarioSoundManager MarioSoundManager;

    bool starpower;

    private void Awake()
    {
        controller2D = GetComponent<CharacterController2D>();
        //m_animator = GetComponent<Animator>();
        Small_Renderer.enabled = true;
        Big_Renderer.enabled = false;
        m_collider = GetComponent<CapsuleCollider2D>();
        m_audioSource = GetComponent<AudioSource>();
        MarioSoundManager = GetComponent<MarioSoundManager>();
    }

    private void Update()
    {
        if (!CanMove) { return; }
        horinzontalValue = Input.GetAxis("Horizontal");
        m_animator.SetFloat("Speed",Mathf.Abs(horinzontalValue));

        if (Input.GetKeyDown(KeyCode.W))
        {           
            jump = true; 
            m_animator.SetBool("Jump",jump);
            PlaySound(MarioSoundManager.MarioJump);
        }

        if(transform.position.y < -10)
        {
            //Mario should die
            MarioDie();
        }

    }

    private void FixedUpdate()
    {
        if (!CanMove) { return; }
        if (controller2D != null)
        {
            controller2D.Move(horinzontalValue * horinzontalSpeed * Time.fixedDeltaTime, jump);
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
            PlaySound(MarioSoundManager.Mario_Kill);
            Destroy(collision.gameObject);
        }

        else if(collision.collider.tag == "EnemyBody" && !starpower)
        {
            //Kill the player
            MarioDie();
        }
        else if(collision.collider.tag == "EnemyBody" && starpower)
        {
            PlaySound(MarioSoundManager.Mario_Kill);
            Destroy(collision.gameObject);
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
            SoundManager.instance.MarioCollectionSound.clip = SoundManager.instance._coin;
            SoundManager.instance.MarioCollectionSound.Play();
        }

        if(collision.gameObject.tag == "castle")
        {
            //Move to next level
            PlayfabManager.instance.SendLeaderboard(GetComponent<CoinWallet>().CoinsCount);
            UIHandler.Instance.WinPanel.gameObject.SetActive(true);
            BackgroundSoundManager.instance.MarioCollectionSound.clip = BackgroundSoundManager.instance.Mario_Win;
            BackgroundSoundManager.instance.MarioCollectionSound.Play();
            this.enabled = false;
        }
    }

    void MarioDie()
    {
        //reload the current scence using scenemanager

        PlayfabManager.instance.SendLeaderboard(GetComponent<CoinWallet>().CoinsCount);
        //PlaySound(MarioSoundManager.Mario_Die);

        BackgroundSoundManager.instance.MarioCollectionSound.clip = BackgroundSoundManager.instance.Mario_Die;
        BackgroundSoundManager.instance.MarioCollectionSound.Play();

        Big_Renderer.enabled = false;
        Small_Renderer.enabled = false;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        float elapsed = 0f;
        float duration = 3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Grow()
    {
        PlaySound(MarioSoundManager.Mario_PowerUp);
        Big_Renderer.enabled = true;
        Small_Renderer.enabled = false;
        m_animator = big_Animator;
        m_collider.offset = new Vector2(0f, 0.5f);
        m_collider.size = new Vector2(0.875f, 2f);
    }
    public void Shrink()
    {
        Big_Renderer.enabled = false;
        Small_Renderer.enabled = true;
        m_animator = small_Animator;
        m_collider.offset = new Vector2(0f, 0f);
        m_collider.size = new Vector2(0.875f, 1f);
    }

    public void Starpower(float duration = 10f)
    {

        PlaySound(MarioSoundManager.Mario_PowerUp);
        StartCoroutine(StarpowerAnimation(duration));
    }

    private IEnumerator StarpowerAnimation(float duration)
    {
        starpower = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                Big_Renderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
                Small_Renderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        Big_Renderer.color = Color.white;
        Small_Renderer.color = Color.white;
        starpower = false;
    }

    public void PlaySound(AudioClip clip)
    {
        m_audioSource.clip = clip;
        m_audioSource.Play();
    }
}
