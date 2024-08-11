using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using SelectCharacter; // �ǉ�

public class PadStageSelect : MonoBehaviour
{
    public GameObject[] topImages; // ��̉摜�̔z��
    public GameObject[] searchImage; // ���̉摜�̔z��
    public SelectStageScene selectStageScene; // SelectStageScene�̎Q�Ƃ�ǉ�

    private Vector2 currentStickInput;
    private Vector2 currentRightStickInput;
    private Vector2 currentLeftStickInput;
    private int currentIndex = 0;
    private bool stickInputReceived = false;
    private float stickInputCooldownDuration = 0.5f; // �X�e�B�b�N���͂̃N�[���_�E�����ԁi�b�j
    private float stickInputCooldownTimer = 0f; // �X�e�B�b�N���͂̃N�[���_�E���^�C�}�[

    void Start()
    {
        // �����ݒ�Ȃǂ�����΂����ɋL�q
    }

    void Update()
    {
        // �Q�[���p�b�h�̓��͂��擾
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return;

        // D�p�b�h�̓��͂��擾
        var dpadInput = gamepad.dpad.ReadValue();

        if (topImages.Length > 0)
        {
            // D�p�b�h�̍����͂����邩�`�F�b�N
            if (dpadInput.x < 0 && dpadInput.y == 0)
            {

                if (!stickInputReceived)
                {
                    SoundManager.Instance.PlayUIClip("�J�[�\���ړ�5");
                    // ���ɃX�N���[��
                    currentIndex = (currentIndex - 1 + topImages.Length) % topImages.Length;
                    SwitchTopImage(currentIndex);
                    SwitchSearchImage(currentIndex);
                    stickInputReceived = true;
                    stickInputCooldownTimer = stickInputCooldownDuration;
                }
            }
            // D�p�b�h�̉E���͂����邩�`�F�b�N
            else if (dpadInput.x > 0 && dpadInput.y == 0)
            {

                if (!stickInputReceived)
                {
                    SoundManager.Instance.PlayUIClip("�J�[�\���ړ�5");
                    // �E�ɃX�N���[��
                    currentIndex = (currentIndex + 1) % topImages.Length;
                    SwitchTopImage(currentIndex);
                    SwitchSearchImage(currentIndex);
                    stickInputReceived = true;
                    stickInputCooldownTimer = stickInputCooldownDuration;
                }
            }
        }

        // �E�X�e�B�b�N�̓��͂��擾
        currentRightStickInput = gamepad.rightStick.ReadValue();
        // ���X�e�B�b�N�̓��͂��擾
        currentLeftStickInput = gamepad.leftStick.ReadValue();

        // �E�X�e�B�b�N�̐��������̓��͂����邩�`�F�b�N
        if (Mathf.Abs(currentRightStickInput.x) > 0.5f)
        {
            if (!stickInputReceived)
            {
                SoundManager.Instance.PlayUIClip("�J�[�\���ړ�5");
                // �E�X�e�B�b�N�̐��������̓��͂ɉ����ĉ摜��؂�ւ���
                SwitchImageByStickInput(currentRightStickInput.x);
                stickInputReceived = true;
                stickInputCooldownTimer = stickInputCooldownDuration;
            }
        }
        // ���X�e�B�b�N�̐��������̓��͂����邩�`�F�b�N
        else if (Mathf.Abs(currentLeftStickInput.x) > 0.5f)
        {
            if (!stickInputReceived)
            {
                SoundManager.Instance.PlayUIClip("�J�[�\���ړ�5");
                // ���X�e�B�b�N�̐��������̓��͂ɉ����ĉ摜��؂�ւ���
                SwitchImageByStickInput(currentLeftStickInput.x);
                stickInputReceived = true;
                stickInputCooldownTimer = stickInputCooldownDuration;
            }
        }

        // �N�[���_�E���^�C�}�[���X�V
        if (stickInputReceived)
        {
            stickInputCooldownTimer -= Time.deltaTime;
            if (stickInputCooldownTimer <= 0f)
            {
                stickInputReceived = false;
            }
        }

        // A�{�^���������ꂽ���ǂ������`�F�b�N
        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            // A�{�^���������ꂽ�Ƃ��̏��������s
            Debug.Log("A button pressed.");
            for (int i = 0; i < searchImage.Length; i++)
            {
                if (searchImage[i] != null)
                {
                    if (i == currentIndex)
                    {
                        Debug.Log("Bottom image selected: " + i);
                        // �X�e�[�W�I���̏�����ǉ�
                        string stageName = searchImage[i].name; // �����ŃX�e�[�W�����擾
                        Debug.Log("Selected stage: " + stageName);
                        SoundManager.Instance.PlayUIClip("����{�^��������11");
                        selectStageScene.GoToOtherScene("Select"+stageName); // �V�[���J�ڂ����s
                    }
                }
            }
        }
    }

    void SwitchImageByStickInput(float stickHorizontalInput)
    {
        if (topImages.Length == 0)
            return;

        if (stickHorizontalInput > 0) // �E�����ɃX�e�B�b�N��|�����ꍇ
        {
            currentIndex = (currentIndex + 1) % topImages.Length;
        }
        else if (stickHorizontalInput < 0) // �������ɃX�e�B�b�N��|�����ꍇ
        {
            currentIndex = (currentIndex - 1 + topImages.Length) % topImages.Length;
        }

        SwitchTopImage(currentIndex);
        SwitchSearchImage(currentIndex);
    }

    void SwitchTopImage(int index)
    {
        for (int i = 0; i < topImages.Length; i++)
        {
            if (topImages[i] != null)
                topImages[i].SetActive(false);
        }
        if (index >= 0 && index < topImages.Length && topImages[index] != null)
            topImages[index].SetActive(true);
    }

    void SwitchSearchImage(int index)
    {
        for (int i = 0; i < searchImage.Length; i++)
        {
            if (searchImage[i] != null)
            {
                // �I����Ԃ̌����ڂ�ύX���鏈����ǉ�
                // ��: searchImage[i].GetComponent<Image>().color = (i == index) ? Color.red : Color.white;
            }
        }
    }
}
