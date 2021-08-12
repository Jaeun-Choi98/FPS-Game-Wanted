using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;
    //총알 프리팹 받기
    [SerializeField]
    private GameObject bulletFactory;
    //총알이 생성되는 장소 
    [SerializeField]
    private GameObject FirePosition;

    private float currentFireRate;

    private AudioSource audioSource;
    //탄창에 총알이 장전된 수
    public int numBullet;

    //총알 오브젝트 풀 
    private int poolSize = 10;
    GameObject[] bulletObjectPool;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //총알 오브젝트 풀 생성
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
        GunFireRateCalc(); // 발사 체크
        TryFire(); // 발사 기능 구현
        Reload(); // 장전 기능 구현
        Walk(); // 플레이어가 걷고 있을 때 애니메이션 구현
        Run(); // '' 달리고 있을 때 ''
    }
   
    //걷기 애니메이터
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

    //달리기 애니메이터
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

    // 장전 or 장전 애니메이터
    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && numBullet==0)
        {
            currentGun.anim.SetTrigger("Reload");
            numBullet++;
        }
    }

    // 연사시간 계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    // 발사 준비
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && numBullet == 1)
        {
            Fire();
            numBullet--;
        }
    }

    // 방아쇠 당김
    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    // 발사 
    private void Shoot()
    {
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        MakeBullet();
        Debug.Log("총알 발사");
    }

    // 총알을 만듬.
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
