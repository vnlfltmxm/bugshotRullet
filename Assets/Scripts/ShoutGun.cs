using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShoutGun : NetworkBehaviour
{
    private Queue<int> _bullet=new Queue<int>();
    //[SyncVar]
    //public int[] _randomBullet=new int[8];
    public SyncList<int> _randomBullet = new SyncList<int>();
    private float _moveSpeed = 3.0f;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    [Server]
    public void ReloadShoutGun()
    {
        int randomIndex = Random.Range(0, 8);

        for (int i = 0; i < 8; i++)
        {
            if (randomIndex != i)
            {
                _randomBullet.Add(0);
            }
            else
            {
                _randomBullet.Add(1);
            }
        }
        //Test();
    }
    [Server]
    private void HookRelad()
    {

    }
    //[ClientRpc]
    public void Test()
    {
        for (int i = 0; i < _randomBullet.Count; i++)
        {
            Debug.LogWarning(_randomBullet[i]);
        }
        
    }



    [ClientRpc]
    public void MoveToPlayer(GameObject player)
    {
        if (player != null)
        {
            this.gameObject.transform.rotation = player.GetComponent<LocalPlayer>()._gunPos.rotation;
            //this.gameObject.transform.eulerAngles = Vector3.zero;
            this.gameObject.transform.Translate((player.GetComponent<LocalPlayer>()._gunPos.position - this.gameObject.transform.position) * _moveSpeed * Time.deltaTime,Space.World);

        }
    }
    [ClientRpc]
    public void ResetMove(GameObject gunPos)
    {
        if (gunPos != null)
        {
            this.gameObject.transform.Translate((gunPos.transform.position - gameObject.transform.position) * _moveSpeed * Time.deltaTime);
            //this.gameObject.transform.rotation = gunPos.transform.rotation;

        }
    }
    [ClientRpc]
    public void ShoutGunAimingSelf(GameObject gunPos)
    {
        this.gameObject.transform.LookAt(gunPos.GetComponent<LocalPlayer>().localPlayer_Camera.transform);
    }
    [ClientRpc]
    public void ShoutGunAimingForward(GameObject gunPos)
    {
        this.gameObject.transform.rotation=gunPos.GetComponent<LocalPlayer>()._gunPos.rotation;
    }
}
