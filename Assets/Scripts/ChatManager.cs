using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using UnityEngine;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    [SerializeField] ChatUI chatUI;

    public bool IsChatting => chatUI.IsChatting;

    ChatClient chatClient;

    string channelName;

    private void Awake()
    {
        if(!PhotonNetwork.IsConnected)
        {
            DestroyImmediate(gameObject);            
        }
    }

    void Start()
    {
        var chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();

        channelName = PhotonNetwork.CurrentRoom.Name;

        chatClient = new ChatClient(this);
        chatClient.UseBackgroundWorkerForSending = true;
        chatClient.AuthValues = new AuthenticationValues(PhotonNetwork.NickName);
        chatClient.ConnectUsingSettings(chatAppSettings);
    }

    private void OnDestroy()
    {
        if(chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    private void OnApplicationQuit()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    private void Update()
    {
        if(chatClient != null)
        {
            chatClient.Service();
        }
    }

    public void SendChatMessage(string message)
    {
        if(string.IsNullOrEmpty(message))
        {
            return;
        }

        if(chatClient != null)
        {
            chatClient.PublishMessage(channelName, message);
        }
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        bool found = this.chatClient.TryGetChannel(channelName, out ChatChannel channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        chatUI.SetChatMessages(channel.ToStringMessages());
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void Disconnect()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    public void OnDisconnected()
    {
        Debug.Log("PhotonChat : OnDisconnected()");
    }

    public void OnConnected()
    {
        Debug.Log("PhotonChat : OnConnected()");
        chatClient.Subscribe(new string[] { channelName });
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log($"OnGetMessage {channelName}");
        if(channelName.Equals(this.channelName))
        {
            ShowChannel(channelName);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }
}
