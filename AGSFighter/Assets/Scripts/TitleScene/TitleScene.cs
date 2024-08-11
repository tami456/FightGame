using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class TitleScene : MonoBehaviour
    {

        private void Start()
        {
            // ����̃I�u�W�F�N�g���폜����
            GameObject player1 = GameObject.Find("yuji");
            GameObject player2 = GameObject.Find("yuji2P");

            if (player1 != null)
                Destroy(player1);

            if (player2 != null)
                Destroy(player2);
            StartCoroutine(FadeController.Instance.FadeIn());
        }

        public void Update()
        {
            if (Input.anyKeyDown)
            {
                SoundManager.Instance.PlayUIClip("�K���X�������1");
                //�@�`���[�g���A���V�[����
                SceneManager.LoadScene("TutorialScene");
            }
        }
    }
}
