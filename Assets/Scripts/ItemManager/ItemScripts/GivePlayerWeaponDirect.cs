using Mirror;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class GivePlayerWeaponDirect : ItemBehaviour
{
    //Public-----------------------------------------------------------------------------------

    public WeaponTypes weaponType;

    //Private----------------------------------------------------------------------------------




    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.gameObject.CompareTag("Player"))
        {
            GameObject player = playerCollider.gameObject;
            NetworkIdentity identity = player.GetComponent<NetworkIdentity>();

            if (identity.isLocalPlayer)
            {
                // If player is in activation radius
                Activate(player);
            }
        }
    }


    private void Activate(GameObject player)
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
