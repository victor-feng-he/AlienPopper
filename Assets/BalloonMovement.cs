using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalloonController : MonoBehaviour
{
    public float moveSpeed = 2.0f;        // Speed of the Balloon's vertical movement
    public float maxSize = 5.0f;          // Maximum size of the Balloon before level restart
    public float sizeIncreaseRate = 0.1f; // Rate at which the Balloon size increases over time
    public AudioClip popSound;

    private Animator popAnimator;
    private float moveDirection = 1.0f;        // 1 for moving up, -1 for moving down
    private AudioSource audioSource;

    void Start()
    {
        // Assuming you have an Animator component attached to the same GameObject as this script
        popAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        MoveBalloon();
        CheckCollision();
        IncreaseSize();
        CheckSizeLimit();
    }

    void MoveBalloon()
    {
        // Move the Balloon vertically
        transform.Translate(Vector3.up * moveDirection * moveSpeed * Time.deltaTime);
    }

    void CheckCollision()
    {
        // Check if the Balloon collides with objects tagged as "Obstacle"
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale / 2, 0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Bullet"))
            {
                PopBalloon();
            }
        }
    }

    void IncreaseSize()
    {
        // Increase the size of the Balloon over time
        transform.localScale += new Vector3(sizeIncreaseRate, sizeIncreaseRate, 0) * Time.deltaTime;
    }

    void CheckSizeLimit()
    {
        // Check if the Balloon size exceeds the specified limit for level restart
        if (transform.localScale.y > maxSize)
        {
            // Add your code here for level restart
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            UnityEngine.Debug.Log("Level Restarted");
        }
    }

    void PopBalloon()
    {
        // Trigger the "Pop" animation
        if (popAnimator != null)
        {
            popAnimator.SetTrigger("Pop");

            if (audioSource != null && popSound != null)
            {
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            }

            // Destroy the GameObject after the animation is finished
            Destroy(gameObject, popAnimator.GetCurrentAnimatorClipInfo(0).Length);
        }
    }
}
