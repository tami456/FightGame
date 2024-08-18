using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoloModeCountDown : MonoBehaviour
{
    // �J�E���g�_�E�����L�����ǂ����̃t���O
    [SerializeField]
    private bool isCount = false;
    // �c�莞��
    private int remainingTime;
    // �J�E���g�_�E���^�C��
    [SerializeField]
    private float countDownTime;
    // �\���p�e�L�X�gUI
    [SerializeField]
    private Text textCountDown;
    // �v���C���[�̃A�N�V�������Ǘ�����N���X
    [SerializeField]
    private PlayerAction player;
    // �X�R�A���Ǘ�����N���X
    [SerializeField]
    private Score score;
    // �Ԃ̃_���[�W���Ǘ�����N���X
    [SerializeField]
    private CarDamage carDamage;
    // UI�A�j���[�V�������Ǘ�����N���X
    private UIAnimation uiAnim;

    // �c�莞�Ԃ��擾����v���p�e�B
    public int GetTime() => remainingTime;

    // �J�E���g�_�E���̊J�n/��~��ݒ肷�郁�\�b�h
    public void StartStopTime(bool isCount)
    {
        this.isCount = isCount;
    }

    // ����������
    private void Start()
    {
        uiAnim = GetComponent<UIAnimation>();
    }

    // ���t���[���Ăяo�����X�V����
    private void Update()
    {
        if (!isCount)
        {
            UpdateCountDown();
        }
    }

    // �J�E���g�_�E���̍X�V����
    private void UpdateCountDown()
    {
        // �J�E���g�_�E���^�C���𐮌`���ĕ\��
        textCountDown.text = string.Format("{0:00}", countDownTime);
        if (countDownTime > 0)
        {
            // �o�ߎ����������Ă���
            countDownTime -= Time.deltaTime;
            remainingTime = Mathf.RoundToInt(countDownTime);
        }
        else
        {
            EndCountDown();
        }
    }

    // �J�E���g�_�E���I�����̏���
    private void EndCountDown()
    {
        player.State = PlayerAction.MyState.Freeze;
        uiAnim.SceneAnimFalse();
        score.TotalScore(carDamage.GetCarScore());
        SoundManager.Instance.StopBGM();
        Debug.Log("Finish!!");
    }
}
