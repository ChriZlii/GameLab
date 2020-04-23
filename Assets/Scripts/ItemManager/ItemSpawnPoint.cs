using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;




public class ItemSpawnPoint : NetworkBehaviour
{
    //Fields--------------------------------------------------------------------------
    [System.Serializable]
    public struct ItemSpawnData
    {
        public GameObject Item;
        [Range(0f, 1f)]
        public float SpawnProbability;
    }



    //Public--------------------------------------------------------------------------
    [HideInInspector]
    public GameObject Item;


    public List<ItemSpawnData> SpawnableItems;


    //Private-------------------------------------------------------------------------




    public void Awake()
    {
        if (SpawnableItems.Count > 0)
        {
            ItemManager.RegisterStartPosition(this);
        }
    }

    public void OnDestroy()
    {
        if (SpawnableItems.Count > 0)
        {
            ItemManager.UnRegisterStartPosition(this);
        }
    }








    public GameObject SpawnRandomItem()
    {
        if (SpawnableItems.Count <= 0)
        {
            return null;
        }

        float random = UnityEngine.Random.value;

        // Destroy old gameobject 
        NetworkServer.Destroy(Item);

        foreach (ItemSpawnData data in SpawnableItems)
        {
            random -= data.SpawnProbability;

            if (random <= 0)
            {
                //NetworkServer.Destroy(Item);
                Item = Instantiate(original: data.Item, position: transform.position, rotation: transform.rotation);
                NetworkServer.Spawn(Item);
                return Item;
            }
        }
        return Item;
    }


    public GameObject SpawnItem(int ID)
    {
        if (SpawnableItems.Count <= 0)
        {
            return null;
        }

        if (!isServer) return null; // only server permitts this action

        // Destroy old gameobject 
        NetworkServer.Destroy(Item);

        // Try to spawn an new Object
        try
        {
            Item = Instantiate(SpawnableItems[ID].Item, transform);
            NetworkServer.Spawn(Item);
        }
        catch (ArgumentOutOfRangeException) { }


        return Item;
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(255, 0, 0, 196);
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(255, 0, 0, 196);
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

























    //internal void NomamalizeProbabilatys()
    //{
    //    float prob = 0;
    //    foreach (ItemSpawnData data in SpawnableItems)
    //    {
    //        prob += data.SpawnProbability;
    //    }

    //    //Debug.Log(prob);

    //    for (int i = 0; i<SpawnableItems.Count;i++)
    //    {
    //        ItemSpawnData data = SpawnableItems[i];
    //        data.SpawnProbability /= prob;
    //        //Debug.Log(data.SpawnProbability);
    //    }

    //}




}
