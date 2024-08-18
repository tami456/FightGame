using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    // �{�^���̎Q��
    [SerializeField]
    private Button SPButton;
    [SerializeField]
    private Button SPMButton;
    [SerializeField]
    private Button SystemButton;

    // �R�}���h�̎Q��
    [SerializeField]
    private GameObject SPCom;
    [SerializeField]
    private GameObject SPMCom;
    [SerializeField]
    private GameObject SystemCom;
    [SerializeField]
    private GameObject SelectCom;

    // ����������
    void Start()
    {
        SetupMenuUIEvent();
    }

    // �L�������̏���
    private void OnEnable()
    {
        SPButton.Select();
    }

    // ���j���[UI�̃C�x���g�ݒ�
    private void SetupMenuUIEvent()
    {
        SPButton.onClick.AddListener(() => ShowCommand(SPCom));
        SPMButton.onClick.AddListener(() => ShowCommand(SPMCom));
        SystemButton.onClick.AddListener(() => ShowCommand(SystemCom));
    }

    // �R�}���h��\�����鏈��
    private void ShowCommand(GameObject activeCommand)
    {
        SPCom.SetActive(activeCommand == SPCom);
        SPMCom.SetActive(activeCommand == SPMCom);
        SystemCom.SetActive(activeCommand == SystemCom);
    }
}
