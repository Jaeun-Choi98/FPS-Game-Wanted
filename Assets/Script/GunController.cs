using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;
    //�Ѿ� ������ �ޱ�
    [SerializeField]
    private GameObject bulletFactory;
    //�Ѿ��� �����Ǵ� ��� 
    [SerializeField]
    private GameObject FirePosition;

    private float currentFireRate;

    private AudioSource audioSource;
    //źâ�� �Ѿ��� ������ ��
    public int numBullet;

    //�Ѿ� ������Ʈ Ǯ 
    private int poolSize = 10;
    GameObject[] bulletObjectPool;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //�Ѿ� ������Ʈ Ǯ ����
        bulletObjectPool = new GameObject[poolSize];
        for(int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletFactory);
            bulletObjectPool[i] = bullet;
            bullet.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc(); // �߻� üũ
        TryFire(); // �߻� ��� ����
        Reload(); // ���� ��� ����
        Walk(); // �÷��̾ �Ȱ� ���� �� �ִϸ��̼� ����
        Run(); // '' �޸��� ���� �� ''
    }
   
    //�ȱ� �ִϸ�����
    void Walk()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.W))
        {
            currentGun.anim.SetBool("Walk", true);
        }
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)
            || Input.GetKeyUp(KeyCode.W))
        {
            currentGun.anim.SetBool("Walk", false);
        }
    }

    //�޸��� �ִϸ�����
    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentGun.anim.SetBool("Run", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentGun.anim.SetBool("Run", false);
        }
    }

    // ���� or ���� �ִϸ�����
    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && numBullet==0)
        {
            currentGun.anim.SetTrigger("Reload");
            numBullet++;
        }
    }

    // ����ð� ���
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    // �߻� �غ�
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && numBullet == 1)
        {
            Fire();
            numBullet--;
        }
    }

    // ��Ƽ� ���
    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    // �߻� 
    private void Shoot()
    {
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        MakeBullet();
        Debug.Log("�Ѿ� �߻�");
    }

    // �Ѿ��� ����.
    private void MakeBullet()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject bullet = bulletObjectPool[i];
            if ( bullet.activeSelf == false)
            {
                bullet.SetActive(true);
                bullet.transform.position = transform.position + (FirePosition.transform.position - transform.position);
                bullet.transform.rotation = transform.rotation;
                break;
            }
        } 
    }

    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

}
