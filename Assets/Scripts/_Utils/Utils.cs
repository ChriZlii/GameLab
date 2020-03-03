using UnityEngine;

public class Utils
{

    public static GameObject ChangeLayerWithChilds( ref GameObject gm, LayerMask layer)
    {
        gm.layer = layer;
        foreach (Transform t1 in gm.transform)
        {
            t1.gameObject.layer = layer;
            foreach (Transform t2 in t1)
            {
                t2.gameObject.layer = layer;
                foreach (Transform t3 in t2)
                {
                    t3.gameObject.layer = layer;
                }
            }
        }
        return gm;
    }
}