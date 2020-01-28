using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyableObject : NetworkBehaviour
{
    [SyncVar] public float Health = 100.0f;









    // Messagereceiver for all Hitmasages.
    // needed to be called from server!
    // sends call to server who calls every client
    // data in list: [0] EnemyNetID, [i] Damage
    public void Msg_HIT(List<object> data)
    {
        if (isServer)
        {
            Rpc_HIT((uint)data[0], (float)data[1]);
            TakeDamage((float)data[1]);
        }
        else throw new UnityException("Call from Client, only enabled for Server/Host");
    }


    // calcs new Healthvalue and calls every client.

    [Command]
    private void Cmd_HIT(uint HitFromID, float Damage)
    {
        TakeDamage(Damage); 
    }

    // Client call when an hit msg is received
    [ClientRpc]
    private void Rpc_HIT(uint HitFromID, float Damage)
    {
        // hit Animation
    }




    // Called from bullet in impact. 
    // Calls the TakeDamage funcrion on the server.
    private void TakeDamage(float damage)
    {
        if (!isServer) return;


        float MyHealth = this.Health;

        if (MyHealth > damage)
        {
            MyHealth -= damage;
        }
        else
        {
            MyHealth = 0;
        }

        this.Health = MyHealth;

        if (MyHealth <= 0)
        {
            // Destroy Object.
            NetworkManager.Destroy(gameObject);
        }
        CmdUpdateHealth(MyHealth);
    }

    
    // calcs the new health value from player, after bullet impact.
    // retruns the rest damage, when damage is heigher then healt.
    [Command]
    private void CmdUpdateHealth(float health)
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
