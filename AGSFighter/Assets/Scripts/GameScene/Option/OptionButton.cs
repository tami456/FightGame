using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OptionButton : MonoBehaviour
{
    // �I�v�V�������j���[�֘A��UI�v�f
    [SerializeField]
    private GameObject option, optionBG, audioSource, commandList, commandSP, key;
    [SerializeField]
    private List<GameObject> commandImage;
    [SerializeField]
    private Button audioButton;
    [SerializeField]
    private Button commandButton;
    [SerializeField]
    private Button keyButton;
    [SerializeField]
    private Button titleButton;
    [SerializeField]
    private Button closeButton;

    // �I�v�V�������j���[�̏�Ԃ��Ǘ�����t���O
    private bool optionActive = false;

    // ����������
    void Start()
    {
        SetupMenuUIEvent();
    }

    // ���t���[���Ăяo�����X�V����
    private void Update()
    {
        HandleOptionClose();
    }

    // �I�v�V�������j���[����鏈��
    private void HandleOptionClose()
    {
        if (OptionActiveFalse(audioSource.activeSelf))
        {
            CloseOptionMenu(audioSource);
        }
        else if (OptionActiveFalse(commandList.activeSelf))
        {
            CloseOptionMenu(commandList);
            foreach (GameObject command in commandImage)
            {
                command.SetActive(false);
            }
        }
        else if (OptionActiveFalse(key.activeSelf))
        {
            CloseOptionMenu(key);
        }
    }

    // �I�v�V�������j���[�����������`�F�b�N
    private bool OptionActiveFalse(bool optionActive)
    {
        return optionActive && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Joystick1Button2));
    }

    // �|�[�Y���j���[�̓��͏���
    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        ToggleOptionMenu();
    }

    // �I�v�V�������j���[�̕\��/��\����؂�ւ���
    private void ToggleOptionMenu()
    {
        optionActive = !optionActive;
        Time.timeScale = optionActive ? 0f : 1f;
        option.SetActive(optionActive);
        if (optionActive)
        {
            audioButton.Select();
        }
        else
        {
            CloseAllOptionMenus();
        }
    }

    // �S�ẴI�v�V�������j���[�����
    private void CloseAllOptionMenus()
    {
        option.SetActive(false);
        audioSource.SetActive(false);
        commandList.SetActive(false);
        foreach (GameObject command in commandImage)
        {
            command.SetActive(false);
        }
        key.SetActive(false);
    }

    // ���j���[UI�̃C�x���g�ݒ�
    private void SetupMenuUIEvent()
    {
        audioButton.onClick.AddListener(() => OpenOptionMenu(audioSource));
        commandButton.onClick.AddListener(() => OpenOptionMenu(commandList, commandSP));
        keyButton.onClick.AddListener(() => OpenOptionMenu(key));
        titleButton.onClick.AddListener(LoadTitleScene);
        closeButton.onClick.AddListener(CloseOptionMenu);
    }

    // �I�v�V�������j���[���J������
    private void OpenOptionMenu(GameObject menu, GameObject additionalMenu = null)
    {
        option.SetActive(false);
        optionBG.SetActive(true);
        menu.SetActive(true);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }
    }

    // �I�v�V�������j���[����鏈��
    private void CloseOptionMenu(GameObject menu)
    {
        option.SetActive(true);
        optionBG.SetActive(false);
        menu.SetActive(false);
    }

    // �^�C�g���V�[����ǂݍ��ޏ���
    private void LoadTitleScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
        SoundManager.Instance.StopBGM();
    }

    // �I�v�V�������j���[����鏈��
    private void CloseOptionMenu()
    {
        Time.timeScale = 1f;
        option.SetActive(false);
        optionActive = false;
    }
}
