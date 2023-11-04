using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    const string GameScene = "Game";

    public enum UIStates
    {
        Ready,   
        Title,
        RoomList,
    }

    [SerializeField] TitlePanelUI titlePanelUI;
    [SerializeField] RoomListPanelUI roomListPanelUI;

    UIStates uiState = UIStates.Ready;
    UIStates UIState 
    { 
        get => uiState;
        set
        {
            uiState = value;

            switch (uiState)
            {
                case UIStates.Ready:
                    titlePanelUI.gameObject.SetActive(false);
                    roomListPanelUI.gameObject.SetActive(false);
                    break;
                case UIStates.Title:
                    titlePanelUI.gameObject.SetActive(true);
                    roomListPanelUI.gameObject.SetActive(false);
                    break;
                case UIStates.RoomList:
                    titlePanelUI.gameObject.SetActive(false);
                    roomListPanelUI.gameObject.SetActive(true);
                    break;
            }
        }      
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    
    }

    private void Start()
    {        
        UIState = UIStates.Ready;
        if (!PhotonNetwork.IsConnected)
        { 
            UIState = UIStates.Title;
        }
    }

    public void Connect()
    {
        if(!PhotonNetwork.IsConnected)            
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 연결");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"연결 종료 : {cause}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가!!");
        PhotonNetwork.LoadLevel(GameScene);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장!!");
        UIState = UIStates.RoomList;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("룸 리스트 업데이트!!");
        roomListPanelUI.UpdateRoomList(roomList);
    }
}
