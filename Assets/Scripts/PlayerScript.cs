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
            musicSource.Stop();
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
        if (collision.collider.tag == "Ground")
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
}
