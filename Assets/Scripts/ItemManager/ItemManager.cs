using Mirror;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkIdentity))]
public class ItemManager : NetworkBehaviour
{

    //Public
    public static List<ItemSpawnPoint> ItemSpawnPoints = new List<ItemSpawnPoint>();


    //Private


    // only the server is permitted to spawn Items!
    public void OnServerStart()
    {        
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





}
