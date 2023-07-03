using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    // ���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;
    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float crouchSpeed;

    // ���� ����
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // �ɾ��� �� �󸶳� ������ ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // �� ���� ����
    private CapsuleCollider capsuleCollider;

    // �ΰ���
    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRoatation();
        CharacterRoatation();
    }

    // �ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    // �ɱ� ����
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
    }

    // �ε巯�� ���� ����
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);

        yield return new WaitForSeconds(applySpeed);
    }

    //���� üũ
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    // ���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    // ����
    private void Jump()
    {
        // ���� ���� ����
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }

    // �޸��� �õ�
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

    // �޸��� ����
    private void Running()
    {
        // ���� ���� ����
        if (isCrouch)
            Crouch();
        isRun = true;
        applySpeed = runSpeed;
    }

    // �޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    // ������ ����
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertiacal = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertiacal).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    // �¿� ĳ���� ȸ��
    private void CharacterRoatation()
    {
        float _yRoatation = Input.GetAxis("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRoatation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // ���� ī�޶� ȸ��
    private void CameraRoatation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRoatationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRoatationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
