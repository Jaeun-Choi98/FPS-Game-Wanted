using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �Ѿ� �ӵ�
    [SerializeField]
    private float bulletSpeed;
    // �Ѿ� ȸ�� ����
    [SerializeField]
    private float bulletRotateSensetivity;
    // �ʿ��� ���۳�Ʈ
    Rigidbody bulletRigid;

    // Update is called once per frame
    private void Start()
    {
        bulletRigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        BulletState(); //�Ѿ� ������ ����
        BulletMove(); // �Ѿ� ���� ����1
        Exit(); // �Ѿ� ���� ����2
        
    }
    
    // �Ѿ� ���� �ı��ϱ�
    void Exit()
    {
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            gameObject.SetActive(false);
        }
    }
    //�Ѿ� ����
    void BulletMove()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            BulletYRotate();
            BulletXRotate();
        }
    }

    //�Ѿ� �¿� ȸ��
    void BulletYRotate()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 bulletRotation = new Vector3(0f, yRotation, 0f) * bulletRotateSensetivity;
        bulletRigid.MoveRotation(bulletRigid.rotation * Quaternion.Euler(bulletRotation));
    }

    //�Ѿ� ���� ȸ��
    void BulletXRotate()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float currentBulletRotationX = bulletRigid.rotation.x;
        currentBulletRotationX -= xRotation * bulletRotateSensetivity;
        currentBulletRotationX = Mathf.Clamp(currentBulletRotationX, -70, 70);
        Vector3 bulletRotation = new Vector3(currentBulletRotationX, 0f, 0f) * bulletRotateSensetivity;
        bulletRigid.rotation = bulletRigid.rotation * Quaternion.Euler(bulletRotation);
    }
    void BulletState()
    {
        Vector3 velocity = transform.forward * bulletSpeed * Time.deltaTime;
        transform.position += velocity;
    }
    private void OnTriggerEnter(Collider collision)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Debug.Log("�Ѿ� �Ҹ�!");   
    }
}
