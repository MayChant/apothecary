using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpsePollutionScript : MonoBehaviour
{
    public float countDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(countDown, countDown);
        countDown -= Time.deltaTime;
        if (countDown < 0)
        {
            Destroy(gameObject);
        }
    }
}
