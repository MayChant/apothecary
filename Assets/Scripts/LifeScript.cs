using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = FindObjectOfType<ApothecaryScript>().transform.position;
        float step = Time.deltaTime * 4f;
        transform.position = Vector2.MoveTowards(transform.position, playerPos, step);
    }
}
