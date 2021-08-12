using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ÃÑ¾Ë ¼Óµµ
    [SerializeField]
    private float bulletSpeed;
    // ÃÑ¾Ë È¸Àü °¨µµ
    [SerializeField]
    private float bulletRotateSensetivity;
    // ÇÊ¿äÇÑ ÄÄÆÛ³ÍÆ®
    Rigidbody bulletRigid;

    // Update is called once per frame
    private void Start()
    {
        bulletRigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        BulletState(); //ÃÑ¾Ë ¿òÁ÷ÀÓ ±¸Çö
        BulletMove(); // ÃÑ¾Ë Á¶ÀÛ ±¸Çö1
        Exit(); // ÃÑ¾Ë Á¶ÀÛ ±¸Çö2
        
    }
    
    // ÃÑ¾Ë °­Á¦ ÆÄ±«ÇÏ±â
    void Exit()
    {
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            gameObject.SetActive(false);
        }
    }
    //ÃÑ¾Ë Á¶Á¾
    void BulletMove()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            BulletYRotate();
            BulletXRotate();
        }
    }

    //ÃÑ¾Ë ÁÂ¿ì È¸Àü
    void BulletYRotate()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 bulletRotation = new Vector3(0f, yRotation, 0f) * bulletRotateSensetivity;
        bulletRigid.MoveRotation(bulletRigid.rotation * Quaternion.Euler(bulletRotation));
    }

    //ÃÑ¾Ë »óÇÏ È¸Àü
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
        Debug.Log("ÃÑ¾Ë ¼Ò¸ê!");   
    }
}
