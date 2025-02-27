﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] GameObject playerPrefab;
    ChatManager chatManager;

    public bool IsPlayerMovable => chatManager == null || !chatManager.IsChatting;

    private void Awake()
    {
        if(instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        chatManager = FindFirstObjectByType<ChatManager>();
    }

    void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Lobby");
            return;
        }

        if (PhotonNetwork.InRoom && Player.LocalPlayer == null)
        {
            PhotonNetwork.Instantiate(this.playerPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }

    public override void OnJoinedRoom()
    {
        if(Player.LocalPlayer == null)
        {
            PhotonNetwork.Instantiate(this.playerPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }

    public override void OnLeftRoom()
    {
        if(chatManager != null)
        {
            chatManager.Disconnect();
        }        
        SceneManager.LoadScene("Lobby");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
