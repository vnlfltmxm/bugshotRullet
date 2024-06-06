using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class GameManger : Singleton<GameManger>
{
    public GameObject Gun_Prefab;
    public Transform Gun_GunSpawnPos;

    [SyncVar]
    private GameObject[] _severPlayers = new GameObject[2];
    private GameObject[] _players = new GameObject[2];

    [HideInInspector]
    public GameObject _gun;

    private ShoutGun _shoutGun;

    [HideInInspector]
    [SyncVar]
    public GameObject _nowPlayer;

    [SyncVar]
    private int _index;
    private int _nowPlayerIndex;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isServer)
        //{
        //    return;
        //}

        //if(_gun != null)
        //{
        //    if (_nowPlayer != null && _gun.transform.position != _nowPlayer.GetComponent<LocalPlayer>()._gunPos.position)
        //    {
        //        MoveGunToPlayer(_nowPlayer);
        //    }
        //}

       
    }
    [Server]
    public void SetPlayers(GameObject player)
    {
        if(player == null)
            return;

        if (_severPlayers[0] == null)
        {
            _severPlayers[0] = player;
        }
        else
        {
            _severPlayers[1] = player;
            GameObject gun = Instantiate(Gun_Prefab, Gun_GunSpawnPos.position, Gun_GunSpawnPos.rotation);
            NetworkServer.Spawn(gun);
            _shoutGun=gun.GetComponent<ShoutGun>();
            ReseveSeverPlayer(_severPlayers, gun);
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
        ReseveSeverIndex(index);
        SetNowPlayer(index);
    }

    [Server]
    private void SetNowPlayer(int index)
    {
        ClientSetPlayer(index);
        ReadyGame();
    }
    [ClientRpc]
    private void ReseveSeverIndex(int index)
    {
        _index = index;
        _nowPlayerIndex = _index;
    }
    [ClientRpc]
    private void ReseveSeverPlayer(GameObject[] severPlayers,GameObject gun)
    {
        _players = severPlayers;
        _gun = gun;
        Debug.LogWarning(_players[0].name);
        Debug.LogWarning(_players[1].name);
        Debug.LogWarning(_gun);
    }
    [ClientRpc]
    private void ClientSetPlayer(int index)
    {
        _index = index;
        _nowPlayer = _players[_nowPlayerIndex];
        _nowPlayer.GetComponent<LocalPlayer>()._isLocalPlayerTurn = true;
    }
}
