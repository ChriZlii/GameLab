using Mirror;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    // Public variables
    public GameObject Bulletinpact = null;

    [Header("Bullet Data")]
    public float BulletDamage = 15f;
    public float BulletSpeed = 1f;
    public float MaxFlightDistance = 50f;

    [HideInInspector] public uint PLAYERID = 0;


    // Private variables
    private float travelDistance = 0;


    void Update()
    {
        float distace = BulletSpeed * Time.deltaTime;
        travelDistance += distace;
        transform.position += transform.forward * distace;  
        
        if (travelDistance >= MaxFlightDistance)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject inpact = Instantiate(Bulletinpact, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
        Debug.Log("Inpact Bullet ID: " + netId);
        //Destroy(inpact, 2f);
        Destroy(this);
    }

}
