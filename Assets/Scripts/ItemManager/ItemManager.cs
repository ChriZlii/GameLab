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



    private void Awake()
    {
        ItemSpawnPoints = new List<ItemSpawnPoint>();
    }


    public void OnServerStart()
    {
        // only the server is permitted to spawn Items!

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
        ItemSpawnPoints.Remove(spawnpoint);
    }







}
