using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class GameManger : Singleton<GameManger>
{
    public GameObject Gun_Prefab;
    public Transform Gun_GunSpawnPos;

    //[SyncVar]
    private GameObject[] _players = new GameObject[2];

    [HideInInspector]
    public GameObject _gun;

    private ShoutGun _shoutGun;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Gun(_players[0]);
        }
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
            ReadyGame();
        }
    }

    [ClientRpc]//실험용
    private void ReadyGame()
    {
        
        Debug.LogWarning("2명 준비됨");
    }
   

    [Server]//실험용
    private void Gun(GameObject player)
    {
        _shoutGun.Move(player);
        //MoveGun(player);
    }

    [ClientRpc]//실험용
    private void MoveGun(GameObject player)
    {
        
    }

}
