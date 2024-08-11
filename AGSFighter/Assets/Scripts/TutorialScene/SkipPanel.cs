using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipPanel : MonoBehaviour
{
    //�X�L�b�v�m�F�p�l��
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Button leftButton;
    [SerializeField]
    private Button rightButton;

    //�`���[�g���A�����󂯂邩�ǂ����̃e�L�X�g
    [SerializeField]
    private GameObject tutoText;
    [SerializeField]
    private GameObject tutoTextSkip;

    //�`���[�g���A�����X�L�b�v���邩�ǂ����̃e�L�X�g
    [SerializeField]
    private GameObject skipText;
    [SerializeField]
    private GameObject skipTextNo;

    //�ŏ��̃`���[�g���A��
    [SerializeField]
    private GameObject tuto1;

    //�`���[�g���A�����W�߂��e
    [SerializeField]
    private GameObject tuto;

    //�v���C���[��State�ύX�̂���
    [SerializeField]
    private GameObject player;
    private PlayerAction playerState;
    private bool tutorialSkipped = false;

    private void Start()
    {
        StartCoroutine(FadeController.Instance.FadeIn());
        playerState = player.GetComponent<PlayerAction>(); 
        playerState.State = PlayerAction.MyState.Freeze;
        SetUpMenuUIEvent();
    }

    private void OnEnable()
    {
        leftButton.Select();
    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if(!context.started)
        {
            return;
        }
        //�`���[�g���A���̓r���ŃX�L�b�v�p�l�����o��
        if (playerState.State == PlayerAction.MyState.Game)
        {
            SkipTutorial();
            leftButton.Select();
        }
    }

    //�`���[�g���A�����X�L�b�v
    private void SkipTutorial()
    {
        tuto.SetActive(false);
        panel.SetActive(true);
        skipText.SetActive(true);
        tutoText.SetActive(false);
        tutoTextSkip.SetActive(false);
        skipTextNo.SetActive(true);
        playerState.State = PlayerAction.MyState.Freeze;
        tutorialSkipped = true;
    }

    //�V�[���ǂݍ���
    private void LoadSelectModeScene()
    {
        SceneManager.LoadScene("SelectModeScene");
    }

    private void SetUpMenuUIEvent()
    {
        leftButton.onClick.AddListener(() =>
        {
            tuto.SetActive(true);
            panel.SetActive(false);
            tutoText.SetActive(false);
            skipText.SetActive(false);

            //�������m��n�̃{�^���ɂ���������
            //�`���[�g���A���̓r���ŃX�L�b�v�p�l�����\������Ă�����
            //�X�L�b�v����p�{�^���ɂȂ�
            if (!tutorialSkipped)
            {
                tuto1.SetActive(true);
            }
            else
            {
                LoadSelectModeScene();
            }

            playerState.State = PlayerAction.MyState.Game;
        });

        rightButton.onClick.AddListener(() =>
        {
            if (!tutorialSkipped)
            {
                LoadSelectModeScene();
            }
            else
            {
                tuto.SetActive(true);
                skipText.SetActive(false);
                panel.SetActive(false);
                playerState.State = PlayerAction.MyState.Game;
            }
        });
    }
}
