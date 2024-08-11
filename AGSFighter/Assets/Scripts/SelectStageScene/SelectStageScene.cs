using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class SelectStageScene : MonoBehaviour
    {
        public GameObject loadingSpinner; // ���[�f�B���O�X�s�i�[�̎Q��

        private void Start()
        {
            // ���[�f�B���O�X�s�i�[��������Ԃł͔�\���ɐݒ�
            loadingSpinner.SetActive(false);

            if (RoundManager.Instance == null)
            {
                Debug.LogError("RoundManager�̃C���X�^���X��������܂���B�V�[���ɔz�u����Ă��邱�Ƃ��m�F���Ă��������B");
            }
        }

        public void GoToOtherScene(string stage)
        {
            // ���̃V�[���f�[�^��RoundManager�ɕۑ�
            if (RoundManager.Instance != null)
            {
                RoundManager.Instance.SetSelectedStage(stage);
            }
            else
            {
                Debug.LogError("RoundManager�̃C���X�^���X������܂���");
            }

            // �񓯊��ǂݍ��݂��J�n
            StartCoroutine(LoadSceneAsync(stage));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // ���[�f�B���O�X�s�i�[��\��
            loadingSpinner.SetActive(true);

            // �V�[���̔񓯊��ǂݍ��݂��J�n
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            // �ǂݍ��݂���������܂ő҂�
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            // �ǂݍ��݂����������烍�[�f�B���O�X�s�i�[���\���ɂ���
            loadingSpinner.SetActive(false);
        }
    }
}
