using Mirror;
using UnityEngine;

public class NetworkManagerExt : NetworkManager
{

    // for itemManagere
    public override void OnStartServer()
    {

        //Find all ItemManager
        ItemManager[] itemManager = Resources.FindObjectsOfTypeAll<ItemManager>();

        // if there are one or more ItemManager, start the first one.
        if (itemManager.Length > 0)
        {
            itemManager[0].OnServerStart();
            //Destroy unused Itemmanagers
            for (int i = 1; i < itemManager.Length; i++)
            {
                itemManager[i].gameObject.SetActive(false);
            }

        }
    }

}
