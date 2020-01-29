using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerWeapon : NetworkBehaviour
{
    //Public-----------------------------------------------------------------------------------

    public WeaponTypes weaponType;
    public bool destroyOnUse = true;

    //Private----------------------------------------------------------------------------------




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
                    weaponType,         // type of weapon
                    true                // true = give / false = take ... weapon
                };
                if (destroyOnUse) messageData.Add(netId); // netid of the Item

                player.SendMessage("Msg_GiveWeapon", messageData, SendMessageOptions.RequireReceiver);

            }
        }
    }











}
