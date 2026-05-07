using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private bool dead;

    public float jumpForce = 7f;
    public AudioClip[] auClip;
    public GameObject fire;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    void Start()
    {
        dead = false;

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (fire != null)
        {
            fire.SetActive(false);
        }
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !dead)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (fire != null)
        {
            fire.SetActive(true);
        }

        if (audioSource != null && auClip.Length > 0)
        {
            audioSource.clip = auClip[0];
            audioSource.Play();
        }

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (dead) return;

        if (col.CompareTag("Score"))
        {
            GameManager manager = FindObjectOfType<GameManager>();

            if (manager != null)
            {
                manager.Score++;
            }

            Destroy(col.gameObject);
        }
        else if (col.CompareTag("Finish"))
        {
            dead = true;

            if (audioSource != null && auClip.Length > 1)
            {
                audioSource.clip = auClip[1];
                audioSource.Play();
            }

            Invoke(nameof(BackToMain), 1.5f);
        }
    }

    void BackToMain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}