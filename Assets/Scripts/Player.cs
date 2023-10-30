using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1f;

    [SerializeField]
    float rotateSpeed = 1f;

    Vector3 moveValue;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        var inputValue = value.Get<Vector2>();

        moveValue = new Vector3(inputValue.x, 0, inputValue.y);
        bool isMoving = moveValue != Vector3.zero;
        animator.SetBool("move", isMoving);
    }

    void OnJump()
    {
        animator.SetTrigger("jump");
    }

    private void Update()
    {
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
