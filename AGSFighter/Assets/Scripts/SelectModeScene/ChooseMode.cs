using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class ChooseMode : MonoBehaviour
    {

        private void Start()
        {
        }
        //�@�L�����N�^�[��I���������Ɏ��s���L�����N�^�[�f�[�^��MyGameManagerData�ɃZ�b�g
        public void OnSelectMode()
        {
            //�R���g���[���[�g�p�̏ꍇ�͂���������
            //�@�{�^���̑I����Ԃ��������đI�������{�^���̃n�C���C�g�\�����\�ɂ���ׂɎ��s
            EventSystem.current.SetSelectedGameObject(null);

        }
        //�@�L�����N�^�[��I���������ɔw�i���I���ɂ���
        public void SwitchButtonBackground(int buttonNumber)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i == buttonNumber - 1)
                {
                    transform.GetChild(i).Find("Background").gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(i).Find("Background").gameObject.SetActive(false);
                }
            }
        }
    }
}
