using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {

        if (!this.transform.parent.transform.parent.GetComponent<MovementLooking>().isLocalPlayer)
        {
            gameObject.SetActive(false);

            //gameObject.GetComponent<Camera>().enabled = false;
            //gameObject.GetComponent<AudioListener>().enabled = false;
        }

    }

    private void Update()
    {
        Debug.Log(this.transform.parent.transform.parent.GetComponent<MovementLooking>().isLocalPlayer);
    }

}
