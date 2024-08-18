using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class TitleScene : MonoBehaviour
    {
        // �X�^�[�g���Ƀt�F�[�h�C�����J�n
        private void Start()
        {
            StartCoroutine(FadeController.Instance.FadeIn());
        }

        // ���t���[���Ăяo�����
        public void Update()
        {
            // �����L�[�������ꂽ�ꍇ
            if (Input.anyKeyDown)
            {
                // ���ʉ����Đ�
                SoundManager.Instance.PlayUIClip("�K���X�������1");
                // �`���[�g���A���V�[���ֈړ�
                SceneManager.LoadScene("TutorialScene");
            }
        }
    }
}
