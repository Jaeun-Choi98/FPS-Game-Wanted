using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManger : MonoBehaviour
{
    //타겟이 현재 필드내에 있는지 확인
    private bool isTargetAlive;
    //현재 시간
    private float currentTime;
    //생성되는 시간간격 조절
    private float minTime = 3f;
    private float maxTime = 8f;
    private float createTime;
    //생성되는 위치
    private float positionX;
    private float positionZ;
    private bool isPositive;
    //타겟 공장
    [SerializeField]
    private GameObject targetFactory;
    //총 사라진 타겟 수
    public int totalDestroyTarget;
    //타겟 오브젝트 풀
    private int poolSize = 10;
    GameObject[] targetObjectPool;

    void Start()
    {
        //게임 로직을 위한 처리된 타겟수 조작
        totalDestroyTarget = -1;
        //생성되는 시간 랜덤
        createTime = UnityEngine.Random.Range(minTime, maxTime);
        //생성되는 장소 랜덤
        isPositive = (UnityEngine.Random.Range(0, 2)==1)?true:false;
        positionX = (isPositive) ? UnityEngine.Random.Range(15, 30) : -1*UnityEngine.Random.Range(15, 30);
        isPositive = (UnityEngine.Random.Range(0, 2) == 1) ? true : false;
        positionZ = (isPositive) ? UnityEngine.Random.Range(15, 30) : -1 * UnityEngine.Random.Range(15, 30);
        //타겟 오브젝트 풀 생성
        targetObjectPool = new GameObject[poolSize];
        for(int i = 0; i < poolSize; i++)
        {
            GameObject target = Instantiate(targetFactory);
            targetObjectPool[i] = target;
            target.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        isTargetAlive = (GameObject.Find("Target(Clone)") == null) ? false : true;
        currentTime += Time.deltaTime;
      
        if (!isTargetAlive && currentTime > createTime)
        {
            //게임매니저를 위한 처리된 타겟 수
            totalDestroyTarget++;
            //Target 생성
            MakeTarget();
            currentTime = 0;
            //생성되는 시간 랜덤
            createTime = UnityEngine.Random.Range(minTime, maxTime);
            //생성되는 장소 랜덤
            isPositive = (UnityEngine.Random.Range(0, 2) == 1) ? true : false;
            positionX = (isPositive) ? UnityEngine.Random.Range(15, 30) : -1 * UnityEngine.Random.Range(15, 30);
            isPositive = (UnityEngine.Random.Range(0, 2) == 1) ? true : false;
            positionZ = (isPositive) ? UnityEngine.Random.Range(15, 30) : -1 * UnityEngine.Random.Range(15, 30);
        }
    }
    
    private void MakeTarget()
    {
        Debug.Log("적 생성");
        for(int i = 0; i < poolSize; i++)
        {
            GameObject target = targetObjectPool[i];
            if(target.activeSelf == false)
            {
                target.SetActive(true);
                TargetAlivePosition(target);
                break;
            }
        }
    }

    void TargetAlivePosition(GameObject target)
    {
        target.transform.position = new Vector3(positionX,0,positionZ);
    }
}
