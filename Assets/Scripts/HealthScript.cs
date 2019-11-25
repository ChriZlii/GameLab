using Mirror;
using UnityEngine;

public class HealthScript : NetworkBehaviour
{
    [SyncVar] public float Health = 100.0f;



    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (!isLocalPlayer) return;

    //    if (collider.gameObject.CompareTag("Bullet"))
    //    {

    //        float damage = collider.gameObject.GetComponent<BulletScript>().BulletDamage;
    //        Cmd_Debug(damage);

    //        NetworkManager.Destroy(collider.gameObject);

    //        float MyHealth = Health - damage;
    //        CmdUpdateHealth(MyHealth);

    //        if (MyHealth <= 0)
    //        {
    //            Cmd_RespawnPlayer(netId);
    //        }

    //    }
    //}

    public void TakeDamageFromBullet(float damage)
    {
        if (!isLocalPlayer) return;

        float MyHealth = this.Health - damage;
        CmdUpdateHealth(MyHealth);

        if (MyHealth <= 0)
        {
            Cmd_RespawnPlayer(netId);
        }
    }





    //public void RespawnPlayer()
    //{
    //    NetworkConnection conn = connectionToClient;
    //    GameObject newPlayer = Instantiate<GameObject>(NetworkManager.singleton.playerPrefab);

    //    NetworkManager.Destroy(gameObject);
    //    //Destroy(gameObject);

    //    NetworkServer.ReplacePlayerForConnection(conn, newPlayer);
    //    NetworkServer.Spawn(newPlayer, conn);
    //}

    [Command]
    public void Cmd_RespawnPlayer(uint PlayerNetID)
    {
        GameObject _player = NetworkIdentity.spawned[PlayerNetID].gameObject;
        NetworkConnection _con = _player.GetComponent<NetworkIdentity>().connectionToClient;

        NetworkManager.Destroy(_player);
        //Destroy(_player);

        GameObject newPlayer = Instantiate<GameObject>(NetworkManager.singleton.playerPrefab);
        newPlayer.GetComponent<HealthScript>().Health = 100;


        NetworkServer.ReplacePlayerForConnection(_con, newPlayer);
        NetworkServer.Spawn(newPlayer, _con);

    }






    [Command]
    public void CmdUpdateHealth(float health)
    {
        this.Health = health;
    }

    [Command]
    public void Cmd_Debug(float obj) => Rpc_Debug(obj);

    [ClientRpc]
    public void Rpc_Debug(float obj)
    {
        Debug.Log(obj);
    }

}
