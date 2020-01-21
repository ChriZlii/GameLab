using Mirror;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    // Public variables
    public GameObject Bulletinpact = null;

    [Header("Bullet Data")]
    public float BulletSpeed = 1f;
    public float MaxFlightDistance = 50f;

    public uint PLAYERID = 0;
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

        // if is Player, tage damage from bullet
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SendMessage("TakeDamageFromBullet", BulletDamage);
        }
        else if (other.gameObject.CompareTag("DestroyableItem"))
        {
            if (!isServer) return;
            other.gameObject.SendMessage("TakeDamage", BulletDamage);
        }
            

        Destroy(inpact, 2f);

        //Debug.Break();
    }

    private void OnTriggerExit(Collider other)
    {
        NetworkManager.Destroy(gameObject);
    }

}
