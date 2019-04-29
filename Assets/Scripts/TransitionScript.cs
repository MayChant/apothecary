using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScript : MonoBehaviour
{
    public GameObject shopCanvas;
    public GameObject hintCanvas;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "+" + GameManagerScript.lifeGained.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameManagerScript.lifeGained = 0;
            shopCanvas.SetActive(true);
            hintCanvas.SetActive(false);
        }
    }
}
