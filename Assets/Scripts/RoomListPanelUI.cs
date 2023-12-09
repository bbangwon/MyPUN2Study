using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanelUI : MonoBehaviour
{
    [SerializeField] GameObject roomButtonPrefab;
    [SerializeField] Transform roomListContent;

    TMP_InputField roomNameInputField;
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    private void Awake()
    {
        roomNameInputField = GetComponentInChildren<TMP_InputField>();
    }

    public void OnCreateRoomButtonClicked()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            Debug.LogError("방 이름이 없습니다.");
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    public void UpdateRoomList(List<RoomInfo> roomInfos)
    {
        UpdateCachedRoomList(roomInfos);
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var roomInfo in cachedRoomList.Values)
        {
            Debug.Log($"방 이름 : {roomInfo.Name}, 방 인원 : {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}");

            var roomButton = Instantiate(roomButtonPrefab, roomListContent);
            var roomName = roomInfo.Name;
            roomButton.GetComponentInChildren<TextMeshProUGUI>().text = roomName;
            roomButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log($"룸 입장 : {roomName}");
                PhotonNetwork.JoinRoom(roomName);
            });
        }

    }
}
