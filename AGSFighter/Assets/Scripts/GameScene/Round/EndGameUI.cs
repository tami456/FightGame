using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel; // �G���h�Q�[���p�l��
    [SerializeField] private Text endGameMessageText; // �G���h�Q�[�����b�Z�[�W��\������e�L�X�g

    private void Start()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false); // ������ԂŃp�l�����\���ɂ���
        }
    }

    // �G���h�Q�[���p�l����\�����A���b�Z�[�W��ݒ肷��
    public void ShowEndGamePanel(string message)
    {
        if (endGamePanel != null && endGameMessageText != null)
        {
            endGameMessageText.text = message; // ���b�Z�[�W��ݒ肷��
            endGamePanel.SetActive(true); // �p�l����\������
        }
    }

    // �G���h�Q�[���p�l�����\���ɂ���
    public void HideEndGamePanel()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false); // �p�l�����\���ɂ���
        }
    }
}
