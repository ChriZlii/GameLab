using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : NetworkBehaviour
{
    public GameObject PlayerPrefab;
    public float Health = 100.0f;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            float damage = collider.gameObject.GetComponent<BulletScript>().BulletDamage;
            Health -= damage;

            if (Health <= 0)
            {
                // destroy
                if (CompareTag("Player"))
                {
                    RespawnPlayer();
                }
                else
                {
                    Destroy(gameObject);
                }
                
            }

        }
    }





    public void RespawnPlayer()
    {
        var conn = connectionToClient;
        var newPlayer = Instantiate<GameObject>(NetworkManager.singleton.playerPrefab);
        //Debug.Break();
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer);

        //Debug.Break();
        transform.Find("Head").Find("Camera").gameObject.SetActive(false);
        newPlayer.transform.Find("Head").Find("Camera").gameObject.SetActive(true);
        //Debug.Break();
        NetworkManager.Destroy(gameObject);
        Destroy(gameObject);

        //Debug.Break();
    }



}
