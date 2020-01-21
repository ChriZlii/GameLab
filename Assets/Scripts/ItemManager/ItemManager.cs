using Mirror;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkIdentity))]
public class ItemManager : NetworkBehaviour
{

    //Public
    public static List<ItemSpawnPoint> ItemSpawnPoints = new List<ItemSpawnPoint>();

    public static ItemManager singleton;


    //Private


    // only the server is permitted to spawn Items!
    public void OnServerStart()
    {
        InitializeSingleton();
        
        foreach (ItemSpawnPoint spawnpoint in ItemManager.ItemSpawnPoints)
        {
            spawnpoint.SpawnRandomItem();
        }

    }

    






    internal static void RegisterStartPosition(ItemSpawnPoint spawnpoint)
    {
        ItemManager.ItemSpawnPoints.Add(spawnpoint);
    }

    internal static void UnRegisterStartPosition(ItemSpawnPoint spawnpoint)
    {
        ItemManager.ItemSpawnPoints.Remove(spawnpoint);
    }





    // maybe quit conection to networkmanager
    void InitializeSingleton()
    {
        if (singleton != null && singleton == this)
        {
            return;
        }

        // do this early
        LogFilter.Debug = NetworkManager.singleton.showDebugMessages;

        if (NetworkManager.singleton.dontDestroyOnLoad)
        {
            if (singleton != null)
            {
                Debug.LogWarning("Multiple ItemManagers detected in the scene. Only one ItemManagers can exist at a time. The duplicate ItemManagers will be destroyed.");
                Destroy(gameObject);
                return;
            }
            if (LogFilter.Debug) Debug.Log("ItemManager created singleton (DontDestroyOnLoad)");
            singleton = this;
            if (Application.isPlaying) DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (LogFilter.Debug) Debug.Log("ItemManager created singleton (ForScene)");
            singleton = this;
        }
    }



}
