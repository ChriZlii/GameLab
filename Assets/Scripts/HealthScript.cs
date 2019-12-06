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




    [Command]
    public void Cmd_RespawnPlayer(uint PlayerNetID)
    {
        GameObject _player = NetworkIdentity.spawned[PlayerNetID].gameObject;
        NetworkConnection _con = _player.GetComponent<NetworkIdentity>().connectionToClient;

        NetworkManager.Destroy(_player);
        //Destroy(_player);

        Transform _spawnpoint = NetworkManager.singleton.GetStartPosition();

        GameObject _newPlayer = Instantiate<GameObject>(NetworkManager.singleton.playerPrefab, _spawnpoint.position, _spawnpoint.rotation);
        _newPlayer.GetComponent<HealthScript>().Health = 100;


        NetworkServer.ReplacePlayerForConnection(_con, _newPlayer);
        NetworkServer.Spawn(_newPlayer, _con);

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
