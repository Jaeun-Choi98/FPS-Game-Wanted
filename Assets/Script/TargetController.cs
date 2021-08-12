using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    //���� ���� �� �Ҹ� �����ϱ� ���� ������Ʈ
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Animator anim;

    //�÷��̾ �ֺ��� ���� �� ���� ������ �ϱ� ���� ����
    private bool isAttack;
    private RaycastHit hit;
    private bool isContact;
    [SerializeField]
    private float attackDistance;

    //Ÿ���� �������� ����� ���� ���� �� ������Ʈ
    private Rigidbody targetRigid;

    private void Awake()
    {
        targetRigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        StartCoroutine(MoveAndRotation()); //AI������ ����
    }

    private void Update()
    {
        IsAttack(); //���� ��� ����
        TryAttack(); //���� ���� üũ 
    }
    
    // Ÿ�� AI �������� ������ �ڷ�ƾ 
    IEnumerator MoveAndRotation()
    {
        int count = 0;
        float distance = 0f;
        float end = 1f;
        int moveCount = 0;

        Vector3 deltRotation = new Vector3(0, 18f, 0);
        while (true)
        {
            //�¿� ȸ��
            if (count == 5)
            {
                deltRotation = -deltRotation;
            }
            if(count == 15)
            {
                deltRotation = -deltRotation;
            }
            targetRigid.MoveRotation(targetRigid.rotation * Quaternion.Euler(deltRotation));
            yield return new WaitForSeconds(0.2f);
            count++;
            if (count == 20)
            {
                //�յ� ������
                count = 0;
                anim.SetBool("Run", true);
                yield return new WaitForSeconds(1f);
                while (moveCount < 15)
                {
                    distance = Mathf.Lerp(distance, end, 0.05f);
                    Vector3 destination = transform.forward * distance;
                    transform.position += destination;
                    yield return new WaitForSeconds(0.1f);
                    moveCount++;
                }
                anim.SetBool("Run", false);
                yield return new WaitForSeconds(1f);
                targetRigid.MoveRotation(targetRigid.rotation * Quaternion.Euler(new Vector3(0, 180f, 0)));
                yield return new WaitForSeconds(1f);
                distance = 0;
                moveCount = 0;
            }
        }
        
    }

    private void IsAttack()
    {
        isContact = Physics.Raycast(transform.position + new Vector3(0,1,0), transform.forward, out hit, attackDistance);
        Debug.DrawRay(transform.position + new Vector3(0,1,0), transform.forward * attackDistance, Color.red);
        if ((isContact && hit.collider.name != "Player") || !isContact)
            isAttack = false;
    }

    private void TryAttack()
    {
        if (isContact && hit.collider.name == "Player" && !isAttack)
        {
            isAttack = true;
            anim.SetTrigger("Attack");
            GameObject.Find("GameManager").GetComponent<GameManager>().energyCut++;
            transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bullet(Clone)")
        {
            PlaySE(hitSound);
            Debug.Log("�� �Ҹ�");
            anim.SetTrigger("Die");
            Invoke("Die", 1.5f);
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
