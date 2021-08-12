using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ���콺������ ������� ���� ����
    public bool isPutExit = false;
    // �Ѿ� ������ �����ֱ� ���� ����
    [SerializeField]
    private Text bulletText;
    private int totalBullet = 1;
    private int currentBullet = 0;
    // ó���� Ÿ���� �����ֱ� ���� ����
    [SerializeField]
    private Text kliiText;
    private int goalKill = 8;
    private int currentKill;
    // �÷��̾� �������ٸ� �����ֱ��� ����

    [SerializeField]
    private Text energyText;
    public int energyCut = 0;
    private int energyBar = 5;
    private int i;

    // Ű ���۹��� �����ֱ��� ����
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
        //���� UI��� ����
        IsPutExit();
        PutExit();
        UpdateBulletText();
        UpdateKillText();
        UpdateEnergyBar();
        SetGuide();
    }

    // Ű ���۹� U.I
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

    // �÷��̾� �������� U.I
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

    // ó���� Ÿ�� U.I
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

    // �Ѿ� U.I
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
