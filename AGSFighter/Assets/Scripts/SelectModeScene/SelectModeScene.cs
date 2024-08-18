using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class SelectModeScene : MonoBehaviour
    {
        // ����������
        private void Start()
        {
            // �K�v�ȏ���������������΂����ɒǉ�
        }

        // VS�V�[���Ɉړ����鏈��
        public void GoToVSScene()
        {
            SceneManager.LoadScene("SelectStageScene");
        }

        // �\���V�[���Ɉړ����鏈��
        public void GoToSoloScene()
        {
            SceneManager.LoadScene("SoloGameScene");
            SoundManager.Instance.PlayBGM("wanderer");
        }
    }
}
