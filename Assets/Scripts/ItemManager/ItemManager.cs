using Mirror;
using System.Collections.Generic;

public class ItemManager : NetworkBehaviour
{

    //Public
    public static List<ItemSpawnPoint> ItemSpawnPoints = new List<ItemSpawnPoint>();

    //Private


    // diese funtion muss beim starten des hostes augerugfen werden    <-------
    public void OnStartHost()
    {
        // only the server is permitted to spawn Items!
        if (!isServer) return;

        foreach (ItemSpawnPoint spawnpoint in ItemSpawnPoints)
        {
            spawnpoint.SpawnRandomItem();
        }

    }

    






    internal static void RegisterStartPosition(ItemSpawnPoint spawnpoint)
    {
        ItemSpawnPoints.Add(spawnpoint);
    }

    internal static void UnRegisterStartPosition(ItemSpawnPoint spawnpoint)
    {
        ItemSpawnPoints.Remove(spawnpoint);
    }







}
