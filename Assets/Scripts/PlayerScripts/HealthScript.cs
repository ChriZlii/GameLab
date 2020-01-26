using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : NetworkBehaviour
{

    [Header("Behavior")]
    public bool ShieldEnabled = true;

    [Header("Health Data")]

    [SyncVar] public float Health = 100.0f;
    [SyncVar] public float ShieldHealth = 100.0f;








    // Messagereceiver for all Hitmasages.
    // sends call to server who calls every client
    public void Msg_HIT(List<object> data)
    {
        if (isServer)
        {
            Rpc_HIT((uint)data[0], (float)data[1]);
        }
        else
        {
            Cmd_HIT((uint)data[0], (float)data[1]);
        }
    }


    // calls every client.
    [Command]
    public void Cmd_HIT(uint HitFromID, float Damage)
    {
        Rpc_HIT(HitFromID, Damage);
    }

    // Client call when an hit msg is received
    [ClientRpc]
    public void Rpc_HIT(uint HitFromID, float Damage)
    {
        // hit Animation

        if (!isLocalPlayer) return;

        TakeDamageHealth(TakeShieldDamageHealth(Damage));
    }







    //Calc the new ShieldHealth after bulletimpact.
    //returns the rest damage when damage is heigher then shieldhealth
    private float TakeShieldDamageHealth(float damage)
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
            ShieldHealth = MyShieldHealth;// if is server CmdCall will fail
            CmdUpdateShieldHealth(MyShieldHealth);
        }
        else
        {
            RestDamage = damage - this.ShieldHealth;
            ShieldHealth = 0;// if is server CmdCall will fail
            CmdUpdateShieldHealth(0);
        }

        return RestDamage;
    }

    // clacs the new health value from player, after bullet impact.
    // retruns the rest damage, when damage is heigher then healt.
    private float TakeDamageHealth(float damage)
    {
        float MyHealth = 0;
        float RestDamage = 0;
        if (this.Health > damage)
        {
            MyHealth = this.Health - damage;
            Health = MyHealth; // if is server CmdCall will fail
            CmdUpdateHealth(MyHealth);
        }
        else
        {
            RestDamage = damage - this.Health;
            Health = 0;
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





    // updates the Healt on every Client over SyncVar
    [Command]
    public void CmdUpdateHealth(float health)
    {
        this.Health = health;
    }

    // updates the Healt on every Client over SyncVar
    [Command]
    public void CmdUpdateShieldHealth(float shieldhealth)
    {
        this.ShieldHealth = shieldhealth;
    }



}
