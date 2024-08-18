using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    // �{�^���̎Q��
    [SerializeField]
    private Button backBattleButton;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button modeButton;
    [SerializeField]
    private Button titleButton;

    // ���j���[UI�Ɣw�i
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private GameObject menuBG;

    // �|�[�Y���j���[�̏�Ԃ��Ǘ�����t���O
    private bool isPaused = false;

    // ����������
    void Start()
    {
        SetupMenuUIEvent();
    }

    // �L�������̏���
    private void OnEnable()
    {
        backBattleButton.Select();
    }

    // �|�[�Y���j���[�̓��͏���
    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        TogglePauseMenu();
    }

    // �|�[�Y���j���[�̕\��/��\����؂�ւ���
    private void TogglePauseMenu()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        menuUI.SetActive(isPaused);
        menuBG.SetActive(isPaused);
    }

    // ���j���[UI�̃C�x���g�ݒ�
    private void SetupMenuUIEvent()
    {
        backBattleButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        modeButton.onClick.AddListener(LoadSelectModeScene);
        titleButton.onClick.AddListener(LoadTitleScene);
    }

    // �Q�[�����ĊJ���鏈��
    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        menuUI.SetActive(false);
        menuBG.SetActive(false);
    }

    // �Q�[�����ăX�^�[�g���鏈��
    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SoloGameScene");
    }

    // ���[�h�I���V�[����ǂݍ��ޏ���
    private void LoadSelectModeScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SelectModeScene");
    }

    // �^�C�g���V�[����ǂݍ��ޏ���
    private void LoadTitleScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }
}
