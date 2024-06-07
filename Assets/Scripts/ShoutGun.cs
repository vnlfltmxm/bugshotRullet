using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class ShoutGun : Singleton<ShoutGun>
{
    private Queue<int> _bullet=new Queue<int>();
    [SerializeField]
    private GameObject ShoutGun_firePos;
    //[SyncVar]
    //public int[] _randomBullet=new int[8];
    public SyncList<int> _randomBullet = new SyncList<int>();
    [SyncVar]
    private GameObject _hitPlayer;
    [SyncVar]
    public int _nowShougunIndex;
    private float _moveSpeed = 3.0f;
    void Start()
    {
        
    }

    void Update()
    {
        Debug.DrawRay(transform.position, (ShoutGun_firePos.transform.position-transform.position)*100,Color.green);
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
        _nowShougunIndex = 0;
        //Test();
    }
    [Server]
    public void FireShoutGun()
    {
        if(Physics.Raycast(this.gameObject.transform.position, ShoutGun_firePos.transform.position - transform.position, out RaycastHit hitPlayer, 100))
        {
            _hitPlayer = hitPlayer.transform.gameObject;
            if (_randomBullet[_nowShougunIndex] != 0)
            {
                //Á×À½
                Debug.LogWarning("½ÇÅº");
            }
            else
            {
                //»ï
                Debug.LogWarning("°øÅº");
                CheckBullect();
                //ÀÌ¶§¸ÂÀº ÇÃ·¹ÀÌ¾îÀÇ ÅÏÀ» true·Î
                _hitPlayer.GetComponent<LocalPlayer>()._isLocalPlayerTurn = true;
                _nowShougunIndex++;
            }
        }
    }
    [ClientRpc]
    public void CheckBullect()
    {

        Debug.LogWarning(_nowShougunIndex);

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
