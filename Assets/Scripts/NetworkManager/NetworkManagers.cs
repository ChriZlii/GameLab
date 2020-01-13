using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagers : NetworkManager
{
    // Wrapper klasse für den vorhandenen Networkmanager.
    // Wird irgendwann zum eigenen Networkmanager gemacht!!!


    public override void OnStartServer()
    {
        //Find all Manager
        ItemManager[] itemManager = Resources.FindObjectsOfTypeAll<ItemManager>();

        // if there are one or more ItemManager, start the first one.
        if (itemManager.Length > 0)
        {
            itemManager[0].OnServerStart();
        }
    }












}
