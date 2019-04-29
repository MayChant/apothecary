using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApothecaryScript : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction;
    public GameObject aura;

    public AudioClip collectClip;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        if (GameManagerScript.aura > 0)
        {
            aura.SetActive(true);
            float auraScale = GameManagerScript.aura * 0.1f + 1.1f;
            aura.transform.localScale = new Vector2(auraScale, auraScale);
        }
        else
        {
            aura.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float walkSpeed = (speed + GameManagerScript.speed * 0.2f) * Time.deltaTime;
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Animator animator = GetComponent<Animator>();
        if (direction.magnitude <= 1e-2)
        {
            animator.SetBool("walking", false);
            return;
        }
        animator.SetBool("walking", true);
        if (Mathf.Abs(direction.x) > 1e-1)
        {
            animator.SetBool("face_left", direction.x < 0);
        }
        transform.Translate(direction.normalized * walkSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "life")
        {
            audioSource.clip = collectClip;
            audioSource.Play();
            GameManagerScript.life += GameManagerScript.worth;
            GameManagerScript.lifeGained += GameManagerScript.worth;
            Destroy(collision.gameObject);
        }
    }
}
