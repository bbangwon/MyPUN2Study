using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Lobby : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    const string GameScene = "Game";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    
    }

    public void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 연결");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"연결 종료 : {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("랜덤 방 참가 실패");

        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가!!");
        PhotonNetwork.LoadLevel(GameScene);
    }

}
