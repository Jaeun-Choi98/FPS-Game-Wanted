using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 마우스포인터 사라짐을 위한 변수
    public bool isPutExit = false;
    // 총알 갯수를 보여주기 위한 변수
    [SerializeField]
    private Text bulletText;
    private int totalBullet = 1;
    private int currentBullet = 0;
    // 처리한 타겟을 보여주기 위한 변수
    [SerializeField]
    private Text kliiText;
    private int goalKill = 8;
    private int currentKill;
    // 플레이어 에너지바를 보여주기한 변수

    [SerializeField]
    private Text energyText;
    public int energyCut = 0;
    private int energyBar = 5;
    private int i;

    // 키 조작법을 보여주기한 변수
    [SerializeField]
    private Text guideText;
    private bool setKeyText = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //각종 UI기능 구현
        IsPutExit();
        PutExit();
        UpdateBulletText();
        UpdateKillText();
        UpdateEnergyBar();
        SetGuide();
    }

    // 키 조작법 U.I
    void SetGuide()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (setKeyText)
            {
                guideText.gameObject.SetActive(false);
            }
            else
            {
                guideText.gameObject.SetActive(true);
            }
            setKeyText = !setKeyText;
        }
    }

    // 플레이어 에너지바 U.I
    void UpdateEnergyBar()
    {
        energyText.text = "";
        i = energyBar - energyCut;
        for(int j = 0; j < i; j++)
        {
            energyText.text += "a ";
        }
        if (i <= 0)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("DieScenes");
        }
    }

    // 처리한 타겟 U.I
    void UpdateKillText()
    {
        currentKill = GameObject.Find("TargetManager").GetComponent<TargetManger>().totalDestroyTarget;
        kliiText.text = "Goal " + currentKill.ToString() + "/" + goalKill.ToString();
        if(currentKill>= goalKill)
        {
            Debug.Log("Game Sucess");
            SceneManager.LoadScene("ClearScenes");
        }
    }

    // 총알 U.I
    void UpdateBulletText()
    {
        currentBullet = GameObject.Find("Holder").GetComponent<GunController>().numBullet;
        bulletText.text =  "Mask " + currentBullet.ToString() + "/" + totalBullet.ToString();
    }

    void IsPutExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPutExit = !isPutExit;
        }
    }

    void PutExit()
    {
        if (isPutExit)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
