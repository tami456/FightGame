using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    // �v���C���[�̃A�N�V�������Ǘ�����N���X
    [SerializeField]
    private PlayerAction player;

    // UI�v�f
    [SerializeField]
    private GameObject ready;
    [SerializeField]
    private GameObject go;
    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private GameObject finishOption;
    [SerializeField]
    private FInishOption finishPanel;

    // �I�[�f�B�I�֘A
    [SerializeField]
    private AudioSource readyAudioSource;
    [SerializeField]
    private AudioClip readyAudio;

    // �A�j���[�^�[�ƃJ�E���g�_�E��
    private Animator anim;
    private SoloModeCountDown countDown;

    // ����������
    private void Start()
    {
        InitializeComponents();
        StartCountdown();
        TriggerReadyAnimation();
        FreezePlayer();
    }

    // �R���|�[�l���g�̏�����
    private void InitializeComponents()
    {
        countDown = GetComponent<SoloModeCountDown>();
        anim = GetComponent<Animator>();
    }

    // �J�E���g�_�E���̊J�n
    private void StartCountdown()
    {
        countDown.StartStopTime(true);
    }

    // "Ready"�A�j���[�V�����̃g���K�[
    private void TriggerReadyAnimation()
    {
        anim.SetTrigger("Ready");
    }

    // �v���C���[���t���[�Y��Ԃɂ���
    private void FreezePlayer()
    {
        player.State = PlayerAction.MyState.Freeze;
    }

    // �X�R�A���L���ɂȂ����Ƃ��̏���
    private void ScoreTrue()
    {
        anim.SetTrigger("ScoreTrue");
        ui.SetActive(false);
    }

    // �Q�[���I���A�j���[�V�����̃g���K�[
    public void SceneAnimFalse()
    {
        anim.SetTrigger("GameFinish");
    }

    // "Ready"�I�[�f�B�I�̍Đ�
    private void ReadyAudio()
    {
        readyAudioSource.PlayOneShot(readyAudio);
    }

    // "GO"�e�L�X�g�̕\���ƃA�j���[�V�����̃g���K�[
    private void GOTextStart()
    {
        ready.SetActive(false);
        go.SetActive(true);
        anim.SetTrigger("GO");
        SoundManager.Instance.PlayUIClip("�j�O�u�n�߂��b�I�v");
    }

    // "GO"�e�L�X�g�̔�\���ƃJ�E���g�_�E���̍ĊJ
    private void GOFalse()
    {
        go.SetActive(false);
        countDown.StartStopTime(false);
        player.State = PlayerAction.MyState.Game;
    }

    // �I���I�v�V�����̕\��
    private void FinishOptionTrue()
    {
        finishOption.SetActive(true);
        finishPanel.ButtonSelect();
    }
}
