using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChooseMode : MonoBehaviour
{

    // ����������
    private void Start()
    {
        // �K�v�ȏ���������������΂����ɒǉ�
    }

    // �L�����N�^�[��I���������Ɏ��s���L�����N�^�[�f�[�^��MyGameManagerData�ɃZ�b�g
    public void OnSelectMode()
    {
        // �R���g���[���[�g�p�̏ꍇ�͂���������
        // �{�^���̑I����Ԃ��������đI�������{�^���̃n�C���C�g�\�����\�ɂ���ׂɎ��s
        EventSystem.current.SetSelectedGameObject(null);
    }

    // �L�����N�^�[��I���������ɔw�i���I���ɂ���
    public void SwitchButtonBackground(int buttonNumber)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SetButtonBackground(i, buttonNumber - 1);
        }
    }

    // �{�^���̔w�i��ݒ肷�鏈��
    private void SetButtonBackground(int index, int selectedButtonIndex)
    {
        bool isActive = (index == selectedButtonIndex);
        transform.GetChild(index).Find("Background").gameObject.SetActive(isActive);
    }
}

