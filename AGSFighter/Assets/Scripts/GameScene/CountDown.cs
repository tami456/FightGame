using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountDown : MonoBehaviour
{
    [SerializeField]
    private bool isCount = false;
    [SerializeField]
    private float initialCountDownTime; // �����J�E���g�_�E���^�C��
    [SerializeField]
    private float countDownTime; // �J�E���g�_�E���^�C��
    [SerializeField]
    private Text textCountDown; // �\���p�e�L�X�gUI
  
    // Start is called before the first frame update
    void Start()
    {
        ResetCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCount&& countDownTime > 0)
        {
            // �J�E���g�_�E���^�C���𐮌`���ĕ\��
            textCountDown.text = string.Format("{0:00}", countDownTime);
                // �o�ߎ����������Ă���
                countDownTime -= Time.deltaTime;
            if(!RoundManager.Instance.isRoundOver && countDownTime <= 0)
            {
                RoundManager.Instance.TimeOut();
                Debug.Log("Finish!!");
                ResetCountDown(); // �J�E���g�_�E���^�C�}�[�����Z�b�g����
            }
        }
    }


    public void ResetCountDown()
    {
        isCount = true; ;
        countDownTime = initialCountDownTime;
        textCountDown.text = string.Format("{0:00}", countDownTime);
    }

    public void StartStopTime(bool isCount)
    {
        this.isCount = isCount;
    }
}
