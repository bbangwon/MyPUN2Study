using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviourPunCallbacks
{
    [SerializeField]
    float moveSpeed = 1f;

    [SerializeField]
    float rotateSpeed = 1f;

    Vector3 moveValue;

    Animator animator;

    [SerializeField]
    TextMeshProUGUI nicknameText;

    public static Player LocalPlayer { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if(photonView.IsMine)
        {
            LocalPlayer = this;
        }
    }

    private void Start()
    {
        if(photonView.IsMine)
        {
            nicknameText.text = PhotonNetwork.NickName;
        }
        else
        {
            nicknameText.text = photonView.Owner.NickName;
        }
    }

    void OnMove(InputValue value)
    {
        if (!photonView.IsMine)
            return;

        var inputValue = value.Get<Vector2>();

        moveValue = new Vector3(inputValue.x, 0, inputValue.y);
        bool isMoving = moveValue != Vector3.zero;
        animator.SetBool("move", isMoving);
    }

    void OnJump()
    {
        if (!photonView.IsMine)
            return;

        animator.SetTrigger("jump");

    }

    private void Update()
    {
        //닉네임 텍스트가 항상 카메라를 바라보도록
        nicknameText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

        if (!photonView.IsMine)
            return;

        transform.position += moveSpeed * Time.deltaTime * moveValue;        
        if(moveValue != Vector3.zero)
        {
            var rotValue = moveValue;
            rotValue.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(rotValue);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
