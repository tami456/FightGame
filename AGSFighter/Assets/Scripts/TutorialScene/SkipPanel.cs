using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipPanel : MonoBehaviour
{
    // �X�L�b�v�m�F�p�l��
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Button leftButton;
    [SerializeField]
    private Button rightButton;

    // �`���[�g���A�����󂯂邩�ǂ����̃e�L�X�g
    [SerializeField]
    private GameObject tutoText;
    [SerializeField]
    private GameObject tutoTextSkip;

    // �`���[�g���A�����X�L�b�v���邩�ǂ����̃e�L�X�g
    [SerializeField]
    private GameObject skipText;
    [SerializeField]
    private GameObject skipTextNo;

    // �ŏ��̃`���[�g���A��
    [SerializeField]
    private GameObject tuto1;

    // �`���[�g���A�����W�߂��e
    [SerializeField]
    private GameObject tuto;

    // �v���C���[��State�ύX�̂���
    [SerializeField]
    private GameObject player;
    private PlayerAction playerState;
    private bool tutorialSkipped = false;

    // ����������
    private void Start()
    {
        StartCoroutine(FadeController.Instance.FadeIn());
        playerState = player.GetComponent<PlayerAction>();
        playerState.State = PlayerAction.MyState.Freeze;
        SetUpMenuUIEvent();
    }

    // �L�������̏���
    private void OnEnable()
    {
        leftButton.Select();
    }

    // �|�[�Y���j���[�̓��͏���
    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        // �`���[�g���A���̓r���ŃX�L�b�v�p�l�����o��
        if (playerState.State == PlayerAction.MyState.Game)
        {
            SkipTutorial();
            leftButton.Select();
        }
    }

    // �`���[�g���A�����X�L�b�v
    private void SkipTutorial()
    {
        SetTutorialActive(false,false);
        panel.SetActive(true);
        SetSkipTextActive(true);
        playerState.State = PlayerAction.MyState.Freeze;
        tutorialSkipped = true;
    }

    // �V�[���ǂݍ���
    private void LoadSelectModeScene()
    {
        SceneManager.LoadScene("SelectModeScene");
    }

    // ���j���[UI�̃C�x���g�ݒ�
    private void SetUpMenuUIEvent()
    {
        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);
    }

    // ���̃{�^�����N���b�N���ꂽ�Ƃ��̏���
    private void OnLeftButtonClick()
    {
        SetTutorialActive(false,true);
        panel.SetActive(false);
        SetSkipTextActive(false);

        // �`���[�g���A���̓r���ŃX�L�b�v�p�l�����\������Ă�����X�L�b�v����p�{�^���ɂȂ�
        if (!tutorialSkipped)
        {
            tuto1.SetActive(true);
        }
        else
        {
            LoadSelectModeScene();
        }

        playerState.State = PlayerAction.MyState.Game;
    }

    // �E�̃{�^�����N���b�N���ꂽ�Ƃ��̏���
    private void OnRightButtonClick()
    {
        if (!tutorialSkipped)
        {
            LoadSelectModeScene();
        }
        else
        {
            SetTutorialActive(false,true);
            SetSkipTextActive(false);
            panel.SetActive(false);
            playerState.State = PlayerAction.MyState.Game;
        }
    }

    // �`���[�g���A���̕\��/��\����ݒ�
    private void SetTutorialActive(bool panelTextActive,bool isActive)
    {
        tuto.SetActive(isActive);
        tutoText.SetActive(panelTextActive);
        tutoTextSkip.SetActive(isActive);
    }

    // �X�L�b�v�e�L�X�g�̕\��/��\����ݒ�
    private void SetSkipTextActive(bool isActive)
    {
        skipText.SetActive(isActive);
        skipTextNo.SetActive(isActive);
    }
}
