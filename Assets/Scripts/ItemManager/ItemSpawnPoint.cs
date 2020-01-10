using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ItemSpawnPoint : NetworkBehaviour
{
    //Fields
    [System.Serializable]
    public struct ItemSpawnData
    {
        public GameObject Item;
        [Range (0f,1f)]
        public float SpawnProbability;
    }




    //Public
    public GameObject Item;

    public List<ItemSpawnData> SpawnableItems;


    //Private

    

    public void Awake()
    {
        ItemManager.RegisterStartPosition(this);
    }

    public void OnDestroy()
    {
        ItemManager.UnRegisterStartPosition(this);
    }







    public GameObject SpawnRandomItem()
    {
        if (!isServer) return null; // only server permitts this action

        float random = Random.value;

        Item = null;

        foreach(ItemSpawnData data in SpawnableItems)
        {
            random -= data.SpawnProbability;

            if (random <= 0)
            {
                Item = Instantiate(data.Item, transform);
                NetworkServer.Spawn(Item);
                return Item;
            }
        }

        return Item;
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
