using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCountDown : MonoBehaviour
{
    [SerializeField]
    protected bool isCount = false;
    [SerializeField]
    protected float initialCountDownTime; // �����J�E���g�_�E���^�C��
    [SerializeField]
    protected float countDownTime; // �J�E���g�_�E���^�C��
    [SerializeField]
    protected Text textCountDown; // �\���p�e�L�X�gUI
    [SerializeField]
    protected PlayerAction player;

    protected virtual void Start()
    {
        ResetCountDown();
    }

    protected virtual void Update()
    {
        if (!isCount && countDownTime > 0)
        {
            // �J�E���g�_�E���^�C���𐮌`���ĕ\��
            textCountDown.text = string.Format("{0:00}", countDownTime);
            // �o�ߎ����������Ă���
            countDownTime -= Time.deltaTime;

            if (countDownTime <= 0)
            {
                OnCountDownEnd();
            }
        }
    }

    public virtual void ResetCountDown()
    {
        countDownTime = initialCountDownTime;
        textCountDown.text = string.Format("{0:00}", countDownTime);
    }

    public virtual void StartStopTime(bool isCount)
    {
        this.isCount = isCount;
    }

    protected abstract void OnCountDownEnd();
}
