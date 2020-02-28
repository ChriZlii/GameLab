using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkRoomManagerExt : NetworkRoomManager
    {

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
}
