using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class GameManger : Singleton<GameManger>
{
    public GameObject Gun_Prefab;
    public Transform Gun_GunSpawnPos;

    [SyncVar]
    private GameObject[] _players = new GameObject[2];

    [HideInInspector]
    public GameObject _gun;

    private ShoutGun _shoutGun;

    [HideInInspector]
    [SyncVar]
    public GameObject _nowPlayer;

    [SyncVar]
    private int _nowPlayerIndex;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isClient)
        {
            return;
        }
        if(_gun != null)
        {
            if (_nowPlayer != null && _gun.transform.position != _nowPlayer.GetComponent<LocalPlayer>()._gunPos.position)
            {
                MoveGunToPlayer(_nowPlayer);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            _nowPlayer = null;
        }
        Debug.Log(_nowPlayerIndex);
        Debug.Log(_players[1]);
        Debug.Log(_players[1].GetComponent<LocalPlayer>()._gunPos.transform.position);
    }
    [Server]
    public void SetPlayers(GameObject player)
    {
        if(player == null)
            return;

        if (_players[0] == null)
        {
            _players[0] = player;
        }
        else
        {
            _players[1] = player;
            _gun = Instantiate(Gun_Prefab, Gun_GunSpawnPos.position, Gun_GunSpawnPos.rotation);
            NetworkServer.Spawn(_gun);
            _shoutGun=_gun.GetComponent<ShoutGun>();
            SetStartPlayer();
            
        }
    }

    [ClientRpc]//실험용
    private void ReadyGame()
    {

        
    }
   

    [Server]
    private void MoveGunToPlayer(GameObject player)
    {
        _shoutGun.MoveToPlayer(player);
        //MoveGun(player);
    }

    [Server]
    private void GunPosReset(GameObject gunPosObj)
    {
        _shoutGun.ResetMove(gunPosObj);
    }
    //[ClientRpc]//실험용
    //private void MoveGun(GameObject player)
    //{

    //}

    [Server]
    private void SetStartPlayer()
    {
        int index = Random.Range(0, 2);
        SetNowPlayer(1);
    }

    [Server]
    private void SetNowPlayer(int index)
    {
        ClientSetPlayer(index);
        ReadyGame();
    }

    [ClientRpc]
    private void ClientSetPlayer(int index)
    {
        _nowPlayerIndex = index;
        _nowPlayer = _players[_nowPlayerIndex];
    }
}
