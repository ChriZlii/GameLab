using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyableObject : NetworkBehaviour
{
    [SyncVar] public float Health = 100.0f;




    // Called from bullet in impact. 
    // Calls the TakeDamage funcrion on the server.
    public void TakeDamage(float damage)
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
