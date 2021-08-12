using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    private Hand currentHand;

    //������
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    //���� �غ�
    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                // ���� �ڷ�ƾ ����.
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    
    //���� ����
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }

    //Hit����
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (checkedObject())
            {
                isSwing = !isSwing;
                // �浹����.
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }
    
    //�浹�ߴ��� Ȯ�����ְ� � ��ü�� �浹�ߴ��� �˷��ش�.
    private bool checkedObject()
    {
        if(Physics.Raycast(transform.position,transform.forward,out hitInfo,currentHand.range))
        {
            return true;
        }
        return false;
    }

    public void HandChange(bool changeWeapon)
    {
        if(changeWeapon)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
