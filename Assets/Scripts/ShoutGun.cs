using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;

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
    public TextMesh text;
    private float _moveSpeed = 3.0f;
    void Start()
    {
        
    }

    void Update()
    {
        Debug.DrawRay(transform.position, (ShoutGun_firePos.transform.position-transform.position)*100,Color.green);
        text.text = netId.ToString();
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
    public void FireShoutGun(GameObject player)
    {
        Debug.DrawRay(transform.position, (ShoutGun_firePos.transform.position - transform.position) * 100, Color.red,1400);
        player.GetComponent<LocalPlayer>()._isLocalPlayerTurn = false;
        if (Physics.Raycast(this.gameObject.transform.position, ShoutGun_firePos.transform.position - transform.position, out RaycastHit hitPlayer, 100, ~0, QueryTriggerInteraction.Collide)) 
        {
            _hitPlayer = hitPlayer.transform.gameObject;
            if (_randomBullet[_nowShougunIndex] != 0)
            {
                //죽음
                Debug.LogWarning("실탄");
            }
            else
            {
                //삼
                Debug.LogWarning("공탄");
                CheckBullect();
                //이때맞은 플레이어의 턴을 true로
                _hitPlayer.GetComponent<LocalPlayer>()._isLocalPlayerTurn = true;
                _nowShougunIndex++;
            }
        }
        else
        {
            Debug.LogWarning("감지 못함");
        }
    }
    [ClientRpc]
    public void CheckBullect()
    {

        
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

    [Server]
    public void TurnOnServer(GameObject aaa)
    {
        this.gameObject.transform.LookAt(aaa.transform);
    }

    [Server]
    public void TurnOnServer2(Quaternion aaa)
    {
        this.gameObject.transform.rotation = aaa;
    }

    [ClientRpc]
    public void ShoutGunAimingForward(GameObject gunPos)
    {
        this.gameObject.transform.rotation=gunPos.GetComponent<LocalPlayer>()._gunPos.rotation;
    }
    [Server]
    public void ReloadTransform(GameObject gun)
    {
        this.gameObject.transform.position = gun.transform.position;
        this.gameObject.transform.rotation = gun.transform.rotation;
    }
}
