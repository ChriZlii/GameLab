using Mirror;
using System.Collections.Generic;
using UnityEngine;

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
            Rpc_HIT(/*(uint)data[0], (float)data[1]*/);
            TakeDamage((float)data[1]);
        }
        else throw new UnityException("Call from Client, only enabled for Server/Host");
    }



    // Client call when an hit msg is received
    [ClientRpc]
    private void Rpc_HIT(/*uint HitFromID, float Damage*/)
    {
        // hit Animation
    }




    // Called from bullet in impact. 
    // Calls the TakeDamage funcrion on the server.
    private void TakeDamage(float damage)
    {
        if (isServer)
        {
            if (this.Health > damage)   { this.Health -= damage; }
            else                        { this.Health = 0; }

            if (this.Health <= 0)
            {
                // Destroy Object.
                NetworkManager.Destroy(gameObject);
            }
        }
    }













    [Command]
    public void Cmd_Debug(float obj) => Rpc_Debug(obj);

    [ClientRpc]
    public void Rpc_Debug(float obj)
    {
        Debug.Log(obj);
    }
}
