using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientScript : MonoBehaviour
{
    public float illness = 0f;
    public bool isInfected = false;
    public bool inPollution = false;
    public float immunityCountDown = 0f;
    private Vector2 direction;

    public GameObject airTransmit;
    public GameObject corpsePollution;
    public GameObject life;

    public AudioClip dieClip;
    public AudioClip healClip;
    public AudioClip infectClip;

    public AudioSource audioSource;

    public bool saved;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        GetComponent<Animator>().SetBool("face_left", direction.x < 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (immunityCountDown > 0)
        {
            immunityCountDown -= Time.deltaTime;
        }
        if (isInfected)
        {
            if (GameManagerScript.airTransmit > 0)
            {
                float scale = 1f + illness * .05f + Mathf.Pow(GameManagerScript.airTransmit, 0.6f) * .2f;
                airTransmit.transform.localScale = new Vector2(scale, scale);
            }

            if (GameManagerScript.insanity > 0)
            {
                transform.Translate(
                    direction * (Mathf.Pow(GameManagerScript.insanity, 0.6f) * Mathf.Min(illness, 5f)) * Time.deltaTime / 2f);
            }
            else
            {
                transform.Translate(direction * Time.deltaTime / 2f);
            }
            illness += Time.deltaTime;
            if (WillDie())
            {
                if (GameManagerScript.corpsePollution > 0)
                {
                    CorpsePollution(illness);
                }
                FindObjectOfType<ApothecaryScript>().audioSource.clip = dieClip;
                FindObjectOfType<ApothecaryScript>().audioSource.Play();
                Destroy(gameObject);
            }
        }
        else
        {
            if (immunityCountDown > 0)
            {
                GetComponent<SpriteRenderer>().color = new Color(0f, 211f / 255f, 248f / 255f);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            transform.Translate(direction * Time.deltaTime / 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.contacts[0].point;
        Vector2 ballLocation = transform.position;
        Vector2 inNormal = (ballLocation - contactPoint).normalized;
        direction = Vector2.Reflect(direction, inNormal);
        GetComponent<Animator>().SetBool("face_left", direction.x < 0);
        if (collision.gameObject.tag == "patient")
        {
            if (collision.gameObject.GetComponent<PatientScript>().isInfected)
            {
                Infect();
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Cure();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "pollution" ||
            (collision.gameObject.tag == "transmit" && collision.gameObject != airTransmit))
        {
            inPollution = true;
            Infect();
        }
        if (collision.gameObject.tag == "aura")
        {
            Cure();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "pollution" || collision.gameObject.tag == "transmit")
        {
            inPollution = false;
        }
    }

    public void Infect()
    {
        if (immunityCountDown <= 0f)
        {
            audioSource.clip = infectClip;
            audioSource.Play();
            if (GameManagerScript.airTransmit > 0)
            {
                airTransmit.SetActive(true);
            }
            isInfected = true;
            GetComponent<SpriteRenderer>().color = new Color(182f / 255f, 210f / 255f, 96f / 255f);
        }
    }

    public void Cure()
    {
        // Either the player should possess immunization, or the patient should not be in polluted area
        if (isInfected && GameManagerScript.life > 0 &&
            (!inPollution || GameManagerScript.immune > 0))
        {
            audioSource.clip = healClip;
            audioSource.Play();
            airTransmit.SetActive(false);
            isInfected = false;
            GameManagerScript.life -= 1;
            GetComponent<SpriteRenderer>().color = Color.white;
            illness = 1f;
        }
        
        if (!isInfected && GameManagerScript.immune > 0)
        {
            immunityCountDown = GameManagerScript.immune * 0.4f + 0.6f;
        }
    }

    public void ProduceLife()
    {
        if (!saved)
        {
            saved = true;
            GameObject lifeInstance = Instantiate(life, transform.position, Quaternion.identity, transform.parent);
        }
    }

    void CorpsePollution(float illness)
    {
        float time = 1f + Mathf.Pow(GameManagerScript.corpsePollution, 0.7f) * illness * 0.05f;
        GameObject pollution = Instantiate(corpsePollution, transform.position, Quaternion.identity, transform.parent);
        pollution.GetComponent<CorpsePollutionScript>().countDown = time;
    }

    bool WillDie()
    {
        float levelProb = Mathf.Pow(0.9f, GameManagerScript.level);
        float illnessProb = 1 - Mathf.Pow(0.9f, Mathf.Floor(illness));
        return Random.value < levelProb * illnessProb * Time.deltaTime;
    }
}
