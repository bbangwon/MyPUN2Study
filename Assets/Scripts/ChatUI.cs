using TMPro;
using UnityEngine;

public class ChatUI : MonoBehaviour
{    
    [SerializeField] TextMeshProUGUI chatMessagesText;
    [SerializeField] TMP_InputField inputField;
    ChatManager chatManager;

    public bool IsChatting => inputField.isFocused;

    private void Start()
    {
        chatManager = FindObjectOfType<ChatManager>();
    }

    public void SetChatMessages(string messages)
    {
        Debug.Log(messages);
        chatMessagesText.text = messages;
    }

    public void OnChatSendButtonClicked()
    {
        string message = inputField.text;
        if(string.IsNullOrEmpty(message))
        {
            return;
        }

        chatManager.SendChatMessage(message);
        inputField.text = string.Empty;
    }

    void OnInputFieldSubmit(string message)
    {
        OnChatSendButtonClicked();
        inputField.ActivateInputField();
    }

    private void OnEnable()
    {
        inputField.onSubmit.AddListener(OnInputFieldSubmit);
    }

    private void OnDisable()
    {
        inputField.onSubmit.RemoveListener(OnInputFieldSubmit);
    }
}
