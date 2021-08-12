using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    private Hand currentHand;

    //공격중
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    //공격 준비
    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                // 공격 코루틴 실행.
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    
    //공격 실행
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

    //Hit실행
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (checkedObject())
            {
                isSwing = !isSwing;
                // 충돌했음.
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }
    
    //충돌했는지 확인해주고 어떤 객체가 충돌했는지 알려준다.
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
