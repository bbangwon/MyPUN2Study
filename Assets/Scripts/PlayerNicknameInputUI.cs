using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNicknameInputUI : MonoBehaviour
{
    const string playerNamePrefKey = "PlayerName";

    TMP_InputField nickNameInputField;

    Lobby lobby;

    void Start()
    {
        nickNameInputField = GetComponentInChildren<TMP_InputField>();  
        lobby = FindObjectOfType<Lobby>();

        string defaultName = "";
        if(PlayerPrefs.HasKey(playerNamePrefKey))
        {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            nickNameInputField.text = defaultName;
        }

        PhotonNetwork.NickName = defaultName;
    }

    public void OnConnectButtonClicked()
    {
        if (string.IsNullOrEmpty(nickNameInputField.text))
        {
            Debug.LogError("플레이어 이름이 없습니다.");
            return;
        }

        PhotonNetwork.NickName = nickNameInputField.text;
        PlayerPrefs.SetString(playerNamePrefKey, nickNameInputField.text);

        lobby.Connect();
    }
}
