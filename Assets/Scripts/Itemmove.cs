using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemmove : MonoBehaviour
{
    bool up = true;
    float counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (up)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (0.2f * Time.deltaTime), transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x , transform.position.y - (0.2f * Time.deltaTime), transform.position.z);
        }
        counter += Time.deltaTime;
        if(counter >= 2)
        {
            up = !up;
            counter = 0;
        }
    }
}
