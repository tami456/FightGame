using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIChange : MonoBehaviour
{
    //�f�o�C�X�m�F�p
    [SerializeField] 
    private PlayerInput playerInput;
    //�R���g���[���[�pUI
    [SerializeField]
    private List<GameObject> tutoPadUI = new List<GameObject>();
    //�L�[�{�[�h�pUI
    [SerializeField]
    private List<GameObject> tutoKeyUI = new List<GameObject>();

    private bool keyboardActive = false;

    private void Update()
    {
        // PlayerInput��null�ł��邩�A�L���ȃv���C���[�ł͂Ȃ��ꍇ�͏������Ȃ�
        if (playerInput == null || !playerInput.user.valid)
        {
            Debug.Log("�A�N�e�B�u�ȃv���C���[�ł͂���܂���");
            return;
        }

        // Keyboard���A�N�e�B�u�ȏꍇ��Keyboard UI��\�����A����ȊO�̏ꍇ��Pad UI��\��
        foreach (var device in playerInput.devices)
        {
            if (device.name == "Keyboard")
            {
                foreach(var pad in tutoPadUI)
                {
                    pad.SetActive(false);
                }
                foreach (var key in tutoKeyUI)
                {
                    key.SetActive(true);
                }
                keyboardActive = true;
                // Keyboard�����������瑦���Ƀ��^�[��
                return; 
            }
            else
            {
                keyboardActive = false;
            }
        }

        // Keyboard��������Ȃ������ꍇ��Pad UI��\��
        if (!keyboardActive)
        {
            foreach (var pad in tutoPadUI)
            {
                pad.SetActive(true);
            }
            foreach (var key in tutoKeyUI)
            {
                key.SetActive(false);
            }
        }
    }
}
