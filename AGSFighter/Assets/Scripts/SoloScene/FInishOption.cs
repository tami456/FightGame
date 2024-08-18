using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FInishOption : MonoBehaviour
{
    // �{�^���̎Q��
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button modeButton;
    [SerializeField]
    private Button titleButton;

    // ����������
    void Start()
    {
        SetupMenuUIEvent();
    }

    // �{�^����I�����鏈��
    public void ButtonSelect()
    {
        restartButton.Select();
    }

    // ���j���[UI�̃C�x���g�ݒ�
    private void SetupMenuUIEvent()
    {
        restartButton.onClick.AddListener(RestartGame);
        modeButton.onClick.AddListener(LoadSelectModeScene);
        titleButton.onClick.AddListener(LoadTitleScene);
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
