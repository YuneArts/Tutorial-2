using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;

    public float speed;

    public Text score;

    public Text winText;

    public Text lives;

    private int scoreValue = 0;

    private int lifeCount;

    public AudioSource musicSource;

    public AudioClip musicClip1;

    public AudioClip musicClip2;

    Animator anim;

    private bool facingRight = true;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        winText.text = "";
        lifeCount = 3;
        GetLifeCount();
        musicSource.clip = musicClip1;
        musicSource.Play();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (scoreValue >= 8)
        {
            winText.text = "You Win! Game created by Mike Rodriguez.";
        }

        if (hozMovement != 0)
        {
            anim.SetInteger("State", 1);
        }
        if (hozMovement == 0)
        {
            anim.SetInteger("State", 0);
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if (scoreValue == 4)
            {
                transform.position = new Vector2(50.0f, 0.0f);
                lifeCount = 3;
                GetLifeCount();
            }

            if (scoreValue == 8)
            {
                musicSource.Stop();
                musicSource.clip = musicClip2;
                musicSource.Play();
            }
        }

        if (collision.collider.tag == "Enemy")
        {

            lifeCount -= 1;
            lives.text = lifeCount.ToString();
            Destroy(collision.collider.gameObject);
            GetLifeCount();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void GetLifeCount()
    {
        lives.text = "Lives: " + lifeCount.ToString();
        if (lifeCount <= 0)
        {
            winText.text = "You Lose!";
            speed = 0;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
