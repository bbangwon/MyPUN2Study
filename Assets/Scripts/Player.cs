using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviourPunCallbacks
{
    [SerializeField]
    float moveSpeed = 20f;

    [SerializeField]
    float rotateSpeed = 1f;

    [SerializeField]
    float jumpPower = 10f;

    [SerializeField]
    float gravity = 20f;

    Vector3 moveValue;

    Animator animator;
    CharacterController characterController;

    [SerializeField]
    TextMeshProUGUI nicknameText;

    public static Player LocalPlayer { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (photonView.IsMine)
        {
            LocalPlayer = this;
        }
    }

    private void Start()
    {
        if(photonView.IsMine)
        {
            nicknameText.text = PhotonNetwork.NickName;
            nicknameText.color = Color.green;
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

        if (GameManager.Instance.IsPlayerMovable)
        {
            var inputValue = value.Get<Vector2>();
            moveValue = new Vector3(inputValue.x, 0, inputValue.y) * moveSpeed;
        }
        else
        {
            moveValue = Vector3.zero;
        }           

        bool isMoving = moveValue != Vector3.zero;
        animator.SetBool("move", isMoving);
    }

    void OnJump()
    {
        if (!photonView.IsMine)
            return;

        if(!GameManager.Instance.IsPlayerMovable)
            return;

        if(!characterController.isGrounded)
            return;

        animator.SetTrigger("jump");

        moveValue.y = jumpPower;
    }

    private void Update()
    {       
        //닉네임 텍스트가 항상 카메라를 바라보도록
        nicknameText.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

        if (!photonView.IsMine)
            return;

        var moveDirection = new Vector3(moveValue.x, 0f, moveValue.z);
        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        this.moveValue.y -= gravity * Time.deltaTime;
        characterController.Move(this.moveValue * Time.deltaTime);
    }
}
