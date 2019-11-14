using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //// movement for local player
        //if (isLocalPlayer)
        //{
        //    transform.gameObject.SetActive(false);
        //}

        if (!this.transform.parent.transform.parent.GetComponent<MovementLooking>().isLocalPlayer)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
        }
    }

}
