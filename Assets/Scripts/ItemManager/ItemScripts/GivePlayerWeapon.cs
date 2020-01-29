using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerWeapon : NetworkBehaviour
{
    //Public-----------------------------------------------------------------------------------

    public WeaponTypes weaponType;

    //Private----------------------------------------------------------------------------------




    [Command]
    public void CmdDespawn(uint playerNetID)
    {
        GameObject obj = NetworkIdentity.spawned[playerNetID].gameObject;

        NetworkServer.Destroy(obj);
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            NetworkIdentity identity = player.GetComponent<NetworkIdentity>();

            if (identity.isLocalPlayer)
            {
                List<object> messageData = new List<object>
                {
                    weaponType,
                    true
                };

                player.SendMessage("Msg_GiveWeapon", messageData, SendMessageOptions.RequireReceiver);

                CmdDespawn(netId);
            }
        }
    }











}
