using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShoutGun : NetworkBehaviour
{
    private float _moveSpeed = 3.0f;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    [ClientRpc]
    public void MoveToPlayer(GameObject player)
    {
        if (player != null)
        {
            this.gameObject.transform.Translate((player.GetComponent<LocalPlayer>()._gunPos.position - this.gameObject.transform.position) * _moveSpeed * Time.deltaTime);
            this.gameObject.transform.rotation = player.GetComponent<LocalPlayer>()._gunPos.rotation;

        }
    }
    [ClientRpc]
    public void ResetMove(GameObject gunPos)
    {
        if (gunPos != null)
        {
            this.gameObject.transform.Translate((gunPos.transform.position - gameObject.transform.position) * _moveSpeed * Time.deltaTime);
            this.gameObject.transform.rotation = gunPos.transform.rotation;

        }
    }
}
