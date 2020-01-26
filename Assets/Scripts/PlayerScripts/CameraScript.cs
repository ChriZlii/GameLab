using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public void Start()
    {

        UpdateCamera();
        StartCoroutine(DelayedFunktion(1f));
    }


    IEnumerator DelayedFunktion(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        UpdateCamera();
    }



    private void UpdateCamera()
    {
        if (!this.transform.parent.transform.parent.GetComponent<MovementLooking>().isLocalPlayer)
        {
            //gameObject.SetActive(false);

            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Camera>().enabled = true;
            gameObject.GetComponent<AudioListener>().enabled = true;
        }
    }



}
