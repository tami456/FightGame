using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class SelectStageScene : MonoBehaviour
    {
        // ���[�f�B���O�X�s�i�[�̎Q��
        public GameObject loadingSpinner;

        // ����������
        private void Start()
        {
            InitializeLoadingSpinner();
            CheckRoundManagerInstance();
        }

        // ���[�f�B���O�X�s�i�[��������ԂŔ�\���ɐݒ�
        private void InitializeLoadingSpinner()
        {
            loadingSpinner.SetActive(false);
        }

        // RoundManager�̃C���X�^���X���`�F�b�N
        private void CheckRoundManagerInstance()
        {
            if (RoundManager.Instance == null)
            {
                Debug.LogError("RoundManager�̃C���X�^���X��������܂���B�V�[���ɔz�u����Ă��邱�Ƃ��m�F���Ă��������B");
            }
        }

        // ���̃V�[���Ɉړ����鏈��
        public void GoToOtherScene(string stage)
        {
            SaveSelectedStage(stage);
            StartCoroutine(LoadSceneAsync(stage));
        }

        // �I�����ꂽ�X�e�[�W��RoundManager�ɕۑ�
        private void SaveSelectedStage(string stage)
        {
            if (RoundManager.Instance != null)
            {
                RoundManager.Instance.SetSelectedStage(stage);
            }
            else
            {
                Debug.LogError("RoundManager�̃C���X�^���X������܂���");
            }
        }

        // �V�[���̔񓯊��ǂݍ��ݏ���
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            ShowLoadingSpinner();

            // �V�[���̔񓯊��ǂݍ��݂��J�n
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            // �ǂݍ��݂���������܂ő҂�
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            HideLoadingSpinner();
        }

        // ���[�f�B���O�X�s�i�[��\��
        private void ShowLoadingSpinner()
        {
            loadingSpinner.SetActive(true);
        }

        // ���[�f�B���O�X�s�i�[���\��
        private void HideLoadingSpinner()
        {
            loadingSpinner.SetActive(false);
        }
    }
}
