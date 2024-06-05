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
    public void Move(GameObject playerGunPos)
    {
        if (playerGunPos != null)
        {
            this.gameObject.transform.Translate((playerGunPos.GetComponent<LocalPlayer>()._gunPos.position - gameObject.transform.position) * _moveSpeed * Time.deltaTime);
            this.gameObject.transform.rotation = playerGunPos.GetComponent<LocalPlayer>()._gunPos.rotation;
        }
    }

}
