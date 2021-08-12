using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피트 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    //상태 변수
    private bool isRun = false;
    private bool isJump = true;
    private bool isCrouch = false;

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //민감도
    [SerializeField]
    private float lookSensitivity;

    //카메라 한개
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;

        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsJump(); //점프 기능 구현
        TryJump(); //점프 체크
        TryRun(); //달리기 기능 구현
        TryCrouch(); // 앉기 기능 구현
        Move(); // 움직임 기능 구현
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            CameraRotationLimit();
            CharacterRotation();
        }
    }

    //앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    //앉기
    private void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
    }
    //크루틴을 이용해 부드럽게 앉기
    IEnumerator CrouchCoroutine()
    {
        float posY = theCamera.transform.localPosition.y;
        int count = 0;
        while(posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    //점프하고 있는지 체크
    private void IsJump()
    {
        isJump = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
    //점프시도
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isJump)
        {
            Jump();
        }
    }
    //점프
    private void Jump()
    {
        //앉은 상태에서 점프시 앉은 상태 해제
        if (isCrouch)
            Crouch();

        myRigid.velocity = transform.up * jumpForce;
    }

    //달리기 시도
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    
    //달리기
    private void Running()
    {
        //앉은 상태에서 달리기시 앉은 상태 해제.
        if (isCrouch)
            Crouch();
        isRun = true;
        applySpeed = runSpeed;
    }

    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    //상화좌우 움직이기
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");
        

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        // Time.deltatime 약 0.016 -> 1초에 60프레임씩 움직이도록 해줌.
        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }
    //카메라 위아래 회전
    private void CameraRotationLimit()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;   
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    //플레이어 좌우 회전
    private void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation*Quaternion.Euler(characterRotationY));  
    }
}
