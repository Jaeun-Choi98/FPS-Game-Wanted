using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManger : MonoBehaviour
{
    //Ÿ���� ���� �ʵ峻�� �ִ��� Ȯ��
    private bool isTargetAlive;
    //���� �ð�
    private float currentTime;
    //�����Ǵ� �ð����� ����
    private float minTime = 3f;
    private float maxTime = 8f;
    private float createTime;
    //�����Ǵ� ��ġ
    private float positionX;
    private float positionZ;
    private bool isPositive;
    //Ÿ�� ����
    [SerializeField]
    private GameObject targetFactory;
    //�� ����� Ÿ�� ��
    public int totalDestroyTarget;
    //Ÿ�� ������Ʈ Ǯ
    private int poolSize = 10;
    GameObject[] targetObjectPool;

    void Start()
    {
        //���� ������ ���� ó���� Ÿ�ټ� ����
        totalDestroyTarget = -1;
        //�����Ǵ� �ð� ����
        createTime = UnityEngine.Random.Range(minTime, maxTime);
        //�����Ǵ� ��� ����
        isPositive = (UnityEngine.Random.Range(0, 2)==1)?true:false;
        positionX = (isPositive) ? UnityEngine.Random.Range(15, 30) : -1*UnityEngine.Random.Range(15, 30);
        isPositive = (UnityEngine.Random.Range(0, 2) == 1) ? true : false;
        positionZ = (isPositive) ? UnityEngine.Random.Range(15, 30) : -1 * UnityEngine.Random.Range(15, 30);
        //Ÿ�� ������Ʈ Ǯ ����
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
            //���ӸŴ����� ���� ó���� Ÿ�� ��
            totalDestroyTarget++;
            //Target ����
            MakeTarget();
            currentTime = 0;
            //�����Ǵ� �ð� ����
            createTime = UnityEngine.Random.Range(minTime, maxTime);
            //�����Ǵ� ��� ����
            isPositive = (UnityEngine.Random.Range(0, 2) == 1) ? true : false;
            positionX = (isPositive) ? UnityEngine.Random.Range(15, 30) : -1 * UnityEngine.Random.Range(15, 30);
            isPositive = (UnityEngine.Random.Range(0, 2) == 1) ? true : false;
            positionZ = (isPositive) ? UnityEngine.Random.Range(15, 30) : -1 * UnityEngine.Random.Range(15, 30);
        }
    }
    
    private void MakeTarget()
    {
        Debug.Log("�� ����");
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
