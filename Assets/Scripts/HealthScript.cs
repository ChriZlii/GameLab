using Mirror;
using UnityEngine;

public class HealthScript : NetworkBehaviour
{

    public bool ShieldEnabled = true;

    [SyncVar] public float Health = 100.0f;
    [SyncVar] public float ShieldHealth = 100.0f;




    // Called from bullet in impact. 
    // Calls the shieldhealt und health update funkction.
    public void TakeDamageFromBullet(float damage)
    {
        if (!isLocalPlayer) return;

        TakeDamage(TakeShieldDamage(damage));
    }





    // Calc the new ShieldHealth after bulletimpact.
    //returns the rest damage when damage is heigher then shieldhealth
    private float TakeShieldDamage(float damage)
    {
        //if Shield is disabled, return damage value 
        if (!this.ShieldEnabled)
        {
            return damage;
        }


        float MyShieldHealth = 0;
        float RestDamage = 0;

        if (this.ShieldHealth > damage)
        {
            MyShieldHealth = this.ShieldHealth - damage;
            CmdUpdateShieldHealth(MyShieldHealth);
        }
        else
        {
            RestDamage = damage - this.ShieldHealth;
            CmdUpdateShieldHealth(0);
        }

        return RestDamage;
    }

    // clacs the new health value from player, after bullet impact.
    // retruns the rest damage, when damage is heigher then healt.
    private float TakeDamage(float damage)
    {
        float MyHealth = 0;
        float RestDamage = 0;
        if (this.Health > damage)
        {
            MyHealth = this.Health - damage;
            CmdUpdateHealth(MyHealth);
        }
        else
        {
            RestDamage = damage - this.Health;
            CmdUpdateHealth(0);
        }

        if (MyHealth <= 0)
        {
            Cmd_RespawnPlayer(netId);
        }

        return RestDamage;
    }



    // Respawns an new Playerobject and transfiers the Playerconection from Server.
    // old Playerobject is deletet.
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
    public void CmdUpdateShieldHealth(float shieldhealth)
    {
        this.ShieldHealth = shieldhealth;
    }







    [Command]
    public void Cmd_Debug(float obj) => Rpc_Debug(obj);

    [ClientRpc]
    public void Rpc_Debug(float obj)
    {
        Debug.Log(obj);
    }

}
