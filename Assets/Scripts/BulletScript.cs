using Mirror;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    // Public variables
    public GameObject Bulletinpact = null;

    [Header("Bullet Data")]
    public float BulletSpeed = 1f;
    public float MaxFlightDistance = 50f;

    [HideInInspector] public uint PLAYERID = 0;
    /*[HideInInspector]*/
    public float BulletDamage = 0f;


    // Private variables
    private float travelDistance = 0;
    private bool isActive = true;


    void Update()
    {
        // dont keep traveling while hiting something   
        if (!isActive) return;

        float distance = BulletSpeed * Time.deltaTime;
        travelDistance += distance;
        transform.position += transform.forward * distance;

        if (travelDistance >= MaxFlightDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject inpact = Instantiate(Bulletinpact, transform.position, Quaternion.LookRotation(transform.forward));

        isActive = false;

        NetworkManager.Destroy(gameObject, 0.1f);
        Destroy(inpact, 2f);

        //Debug.Break();
    }

}
