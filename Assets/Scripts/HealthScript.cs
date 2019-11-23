using Mirror;
using UnityEngine;

public class HealthScript : NetworkBehaviour
{

    [SyncVar] public float Health = 100.0f;



    private void OnTriggerEnter(Collider collider)
    {
        if (!isLocalPlayer) return;

        if (collider.gameObject.CompareTag("Bullet"))
        {

            float damage = collider.gameObject.GetComponent<BulletScript>().BulletDamage;

            CmdUpdateHealth(this.Health - damage);

            if (this.Health <= 0)
            {
                RespawnPlayer();
            }

        }
    }





    public void RespawnPlayer()
    {
        NetworkConnection conn = connectionToClient;
        GameObject newPlayer = Instantiate<GameObject>(NetworkManager.singleton.playerPrefab);

        NetworkManager.Destroy(gameObject);
        //Destroy(gameObject);

        NetworkServer.ReplacePlayerForConnection(conn, newPlayer);
    }






    [Command]
    public void CmdUpdateHealth(float health)
    {
        this.Health = health;
    }



}
