using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private float timeStep;
    public GameObject patient;
    private Text areaCodeText;
    private Text lifeText;
    public GameObject gameOverCanvas;

    public static int level = 1;
    public static int life = 10;
    public static int lifeGained = 0;
    public static GameManagerScript instance = null;
    /* Upgrades */
    public static int immune = 0; // Give all patients in contact immunity for a certain amount of time
    public static int speed = 0; // Extra moving speed
    public static int aura = 0; // Larger curing aura (collider)
    public static int worth = 1; // Life's worth
    /* Virus Mutations */
    public static int airTransmit = 0;
    public static int insanity = 0;
    public static int corpsePollution = 0;
    public static bool[] mutated;

    // Start is called before the first frame update
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        else if (instance != this)
        {
            //If instance already exists and it's not this:
            Destroy(gameObject);
        }
        if (FindObjectOfType<ApothecaryScript>())
        {
            DontDestroyOnLoad(gameObject);
            Mutation();
            Initialize();
            areaCodeText = GameObject.FindGameObjectWithTag("area_number").GetComponent<Text>();
            lifeText = GameObject.FindGameObjectWithTag("life_display").GetComponent<Text>();
            areaCodeText.text = "AREA-" + level;
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        if (FindObjectOfType<ApothecaryScript>())
        {
            if (gameOverCanvas.activeSelf)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Restart();
                    SceneManager.LoadScene(0);
                }
                return;
            }
            if (LevelEnded())
            {
                LifeSaved();
                if (FindObjectsOfType<LifeScript>().Length == 0)
                {
                    if (life > 0)
                    {
                        new WaitForSeconds(5f);
                        level += 1;
                        SceneManager.LoadScene(2);
                    }
                    else
                    {
                        gameOverCanvas.SetActive(true);
                    }
                }
            }
            lifeText = GameObject.FindGameObjectWithTag("life_display").GetComponent<Text>();
            lifeText.text = life.ToString();
        } 
    }

    public void Restart()
    {
        level = 1;
        life = 10;
        lifeGained = 0;
        /* Upgrades */
        immune = 0; // Give all patients in contact immunity for a certain amount of time
        speed = 0; // Extra moving speed
        aura = 0; // Larger curing aura (collider)
        worth = 1; // Life's worth
        /* Virus Mutations */
        airTransmit = 0;
        insanity = 0;
        corpsePollution = 0;
        mutated = new bool[] { false, false, false };
    }

    bool LevelEnded()
    {
        PatientScript[] patients = FindObjectsOfType<PatientScript>();
        foreach(PatientScript patient in patients)
        {
            if (patient.isInfected)
            {
                return false;
            }
        }
        return FindObjectsOfType<CorpsePollutionScript>().Length == 0;
    }

    int LifeSaved()
    {
        PatientScript[] lives = FindObjectsOfType<PatientScript>();
        foreach(PatientScript life in lives)
        {
            life.ProduceLife();
        }
        return lives.Length;
    }

    void Initialize()
    {
        for (int i = 0; i < Random.Range(Mathf.Pow(level, 0.6f), Mathf.Pow(level, 0.8f)); i++)
        {
            Instantiate(
                patient,
                new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)),
                Quaternion.identity,
                transform.parent
                );
        }
        for (int i = 0; i < Mathf.Pow(level, 0.8f); i++)
        {
            GameObject illPatient = Instantiate(
                patient,
                new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)),
                Quaternion.identity,
                transform.parent
                );
            illPatient.GetComponent<PatientScript>().Infect();
        }
    }

    void Mutation()
    {
        mutated = new bool[] { false, false, false };
        if (level == 1)
        {
            return;
        }
        float prob = 1 - Mathf.Pow(.99f, level);
        float airTransmitProb = Mathf.Pow(0.8f, airTransmit);
        if (Random.value < airTransmitProb * prob)
        {
            airTransmit++;
            mutated[0] = true;
        }
        float insanityProb = Mathf.Pow(0.9f, insanity);
        if (Random.value < insanityProb * prob)
        {
            insanity++;
            mutated[1] = true;
        }
        float corpsePollutionProb = Mathf.Pow(0.7f, corpsePollution);
        if (Random.value < corpsePollutionProb * prob)
        {
            corpsePollution++;
            mutated[2] = true;
        }
    }
}
