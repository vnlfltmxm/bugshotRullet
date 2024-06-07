using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Mirror;
using Org.BouncyCastle.Asn1.Cmp;

public class LocalPlayer : NetworkBehaviour
{
    [HideInInspector]
    [SyncVar] public bool _isLocalPlayerTurn = false;
    [HideInInspector]
    [SyncVar] public bool _isDie = false;
    public Transform _gunPos;

    public Camera localPlayer_Camera;

    private float _mouseXRotate;
    private float _mouseYRotate;
    private float _cameraXRotate;
    private float _cameraYRotate;
    private float _rotateSpeed = 150.0f;

    public TextMesh _textMesh;
    [SyncVar]
    private string _text;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            localPlayer_Camera.gameObject.SetActive(false);
            return;
        }
        localPlayer_Camera.transform.forward = transform.forward;        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        RegistrationSelf(this.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        _textMesh.text = netId.ToString();
        if (!isLocalPlayer)
        {
            return;
        }
        //if(Input.mousePosition.x < Screen.width 
        //    && Input.mousePosition.y < Screen.height)
        //{
        //    CameraRotate();
        //}
        CameraRotate();
        MouseTest();
        //ShoutGunTest();
        
        if (_isLocalPlayerTurn)
        {
            if (Vector3.Distance(this._gunPos.position, GameManger.Instance._gun.transform.position) >= 10.0f)
            {
                MoveGunToPlayer(this.gameObject);
                AimingForward(this.gameObject);
            }

            AimingShoutGun();
            ReadyShoutGun(this.gameObject);
            //MoveGunToSever();
        }
    }
    [Command]
    private void MoveGunToPlayer(GameObject player)
    {
        ShoutGun.Instance.MoveToPlayer(player);
        ShoutGun.Instance.MoveToPlayerOnSever(player);
        //MoveGun(player);

    }
    [Command]
    private void MoveGunToSever()
    {
        ShoutGun.Instance.ReloadTransform(GameManger.Instance._gun);

    }
    private void CameraRotate()
    {
        _mouseYRotate = -Input.GetAxis("Mouse Y") * Time.deltaTime * _rotateSpeed;
        _mouseXRotate = Input.GetAxis("Mouse X") * Time.deltaTime * _rotateSpeed;

        _cameraXRotate += _mouseXRotate;
        _cameraYRotate += _mouseYRotate;

        _cameraYRotate = Mathf.Clamp(_cameraYRotate, -10, 20);
        _cameraXRotate = Mathf.Clamp(_cameraXRotate, -60, 60);
        localPlayer_Camera.transform.localRotation = Quaternion.Euler(_cameraYRotate, _cameraXRotate, 0f);
        //localPlayer_Camera.transform.eulerAngles = new Vector3(_cameraYRotate, _cameraXRotate, 0);
    }

    private void MouseTest()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void AimingShoutGun()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AimingSelf(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AimingForward(this.gameObject);
        }
    }

    [Command]
    private void Fire(GameObject player)
    {
        ShoutGun.Instance.FireShoutGun(player);
    }

    public void GameOver()
    {
        Debug.LogWarning("주금");
    }
    public void Survival()
    {
        Debug.LogWarning("사름");
    }
    private void ReadyShoutGun(GameObject player)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this._isLocalPlayerTurn = false;
            Fire(player);
        }
            
    }


    [Command]
    private void RegistrationSelf(GameObject player)
    {
        GameManger.Instance.SetPlayers(this.gameObject);
    }
    [Command]
    private void AimingSelf(GameObject player)
    {
        ShoutGunRotate(player);
    }
    [Command]
    private void AimingForward(GameObject player)
    {
        ShoutGunForward(player);
    }
    [Server]
    private void ShoutGunForward(GameObject player)
    {
        var shotGun = GameManger.Instance._gun.GetComponent<ShoutGun>();
        if(shotGun == null)
        {
            Debug.LogWarning("null for");
        }

        GameManger.Instance._gun.GetComponent<ShoutGun>().ShoutGunAimingForward(player);
        GameManger.Instance._gun.GetComponent<ShoutGun>().TurnOnServer2(player.GetComponent<LocalPlayer>()._gunPos.gameObject);
    }
    [Server]
    private void ShoutGunRotate(GameObject player)
    {
        var shotGun = GameManger.Instance._gun.GetComponent<ShoutGun>();
        if (shotGun == null)
        {
            Debug.LogWarning("null rota");
        }
        GameManger.Instance._gun.GetComponent<ShoutGun>().ShoutGunAimingSelf(player);
        GameManger.Instance._gun.GetComponent<ShoutGun>().TurnOnServer(player.GetComponent<LocalPlayer>().localPlayer_Camera.gameObject);
    }
}
