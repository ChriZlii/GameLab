using Mirror;
using UnityEngine;

[AddComponentMenu("")]
public class NetworkRoomManagerExt : NetworkRoomManager
{

    public override void Start()
    {
        base.Start();
        Application.targetFrameRate = 45; //For a target fps of 45.
    }


    // for itemManagere
    public override void OnRoomServerSceneChanged(string sceneName)
    {

        if (sceneName.StartsWith("GameScene"))
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




}
