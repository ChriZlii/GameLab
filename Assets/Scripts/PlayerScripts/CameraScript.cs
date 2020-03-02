using System.Collections;
using UnityEngine;
public class CameraScript : MonoBehaviour
{
    //[SerializeField]
    public GameObject[] CameraDeaktivationList;

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
        foreach (GameObject cam in CameraDeaktivationList)
        {
            SetupCam(cam);
        }
    }

    private void SetupCam(GameObject CameraObject)
    {
        Camera cam = CameraObject.GetComponent<Camera>();
        AudioListener audio = CameraObject.GetComponent<AudioListener>();

        if (!this.transform.parent.transform.parent.GetComponent<MovementLooking>().isLocalPlayer)
        {
            if (cam != null) cam.enabled = false;
            if (audio != null) audio.enabled = false;
        }
        else
        {
            if (cam != null) cam.enabled = true;
            if (audio != null) audio.enabled = true;
        }

    }
}
