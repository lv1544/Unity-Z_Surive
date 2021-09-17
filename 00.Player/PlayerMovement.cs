using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;// 이동속도
    public float rotateSpeed = 180f; // 좌우 회전 속도

    private PlayerInput playerInput;
    private Rigidbody   playerRigidbody;
    private Animator    playerAnimator;

    //컴포넌트 가져오기
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //회전 실행
        Rotate();
        //움직임 실행
        Move();
        //입력값에 따라 애니메이터의 Move 파라미터값 변경
        playerAnimator.SetFloat("Move", playerInput.move);

    }

    private void Move()
    {
        //상대적으로 이동할 거리 계산
        Vector3 moveDistance =
            playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        //리지드 바디를 이용해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void Rotate()
    {
        //상대적으로 회전할 수치계산
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        //리지드바디를 이용해 게임 오브젝트 회전 변경
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);



    }


}
