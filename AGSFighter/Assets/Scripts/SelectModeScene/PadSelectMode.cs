using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PadSelectMode : MonoBehaviour
{
    public Selectable[] buttons; // �{�^���̔z��
    private int currentIndex = 0; // ���݂̃t�H�[�J�X����Ă���{�^���̃C���f�b�N�X
    private float maxIdleTime = 5f; // �t�H�[�J�X�����܂ł̑ҋ@����
    private float ignoreInputTime = 1f; // PAD ���͂𖳎����鎞��
    private float timeSinceLastInput = 0f; // �Ō�̓��͂���̌o�ߎ���
    private Vector2 previousMousePosition; // �O�t���[���̃}�E�X���W
    private bool inputEnabled = true; // PAD ���͂��L�����ǂ����̃t���O
    private bool canPlaySound = true; // �T�E���h�Đ����\���ǂ����̃t���O
    private float soundCooldown = 0.2f; // �T�E���h�Đ��̃N�[���_�E���^�C��

    private float previousHorizontalInput = 0f; // �O�t���[���̐����������͒l
    private bool[] buttonHovered; // �{�^���̃z�o�[��Ԃ�ǐՂ���z��

    void Start()
    {
        // ������Ԃŗ����̃{�^���̃t�H�[�J�X����������
        EventSystem.current.SetSelectedGameObject(null);
        buttonHovered = new bool[buttons.Length];
    }

    void Update()
    {
        if (!inputEnabled)
            return; // PAD ���͂�����������Ă���ꍇ�͏������s��Ȃ�

        // �Q�[���p�b�h�̓��͂��擾
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            // ���͂��������ꍇ�̓t�H�[�J�X�����̃J�E���g�����Z�b�g
            if (gamepad.leftStick.x.ReadValue() != 0 || gamepad.buttonSouth.wasPressedThisFrame)
            {
                timeSinceLastInput = 0f;
            }
            else
            {
                timeSinceLastInput += Time.deltaTime;
                if (timeSinceLastInput >= maxIdleTime)
                {
                    // ��莞�ԓ��͂��Ȃ������ꍇ�̓t�H�[�J�X������
                    EventSystem.current.SetSelectedGameObject(null);
                    return;
                }
            }

            // �{�^���̃t�H�[�J�X�𐅕������̓��͂ɉ����Đ؂�ւ���
            float horizontalInput = gamepad.leftStick.x.ReadValue();
            if (Mathf.Abs(horizontalInput) > 0.5f && canPlaySound && horizontalInput != previousHorizontalInput)
            {
                // �{�^���̃t�H�[�J�X��؂�ւ���
                currentIndex += (int)Mathf.Sign(horizontalInput);
                currentIndex = Mathf.Clamp(currentIndex, 0, buttons.Length - 1);
                EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
                SoundManager.Instance.PlayUIClip("�J�[�\���ړ�5");
                StartCoroutine(SoundCooldown()); // �T�E���h�̃N�[���_�E���^�C�����J�n
            }

            // ���������̓��͒l���X�V
            previousHorizontalInput = horizontalInput;

            // PAD��A�{�^���������ꂽ�Ƃ��ɁA�I������Ă���{�^����OnClick�C�x���g���g���K�[����
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                SoundManager.Instance.PlayUIClip("����{�^��������11");
                // �I������Ă���{�^���������OnClick�C�x���g���g���K�[����
                if (buttons.Length > 0 && currentIndex >= 0 && currentIndex < buttons.Length)
                {
                    // �{�^���̃C���f�b�N�X�ɂ���đJ�ڐ�����肷��
                    switch (currentIndex)
                    {
                        case 0: // �z��̈�ڂ̃{�^���������ꂽ�ꍇ
                            SceneManager.LoadScene("SelectStageScene");
                            break;
                        case 1: // �z��̓�ڂ̃{�^���������ꂽ�ꍇ
                            SceneManager.LoadScene("SoloGameScene");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // �}�E�X�̓��͂����m���ăt�H�[�J�X��ݒ肷��
        Vector2 mousePosition = Input.mousePosition;
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
            {
                if (!buttonHovered[i])
                {
                    buttonHovered[i] = true;
                    SoundManager.Instance.PlayUIClip("�J�[�\���ړ�5");
                }
            }
            else
            {
                buttonHovered[i] = false;
            }
        }

        // �}�E�X�N���b�N���̃T�E���h���Đ�
        if (Input.GetMouseButtonDown(0))
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject != null)
            {
                SoundManager.Instance.PlayUIClip("����{�^��������11");
            }
        }
    }

    // �T�E���h�Đ��̃N�[���_�E���^�C���𐧌䂷��R���[�`��
    private IEnumerator SoundCooldown()
    {
        canPlaySound = false;
        yield return new WaitForSeconds(soundCooldown);
        canPlaySound = true;
    }

    // �V�[�������[�h���ꂽ�ۂɌĂяo�����R�[���o�b�N
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // �V�[�����A�����[�h�����ۂɌĂяo�����R�[���o�b�N
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �V�[�������[�h���ꂽ���̏���
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �Z���N�g���[�h�V�[�������[�h���ꂽ���APAD���͂𖳌�������
        if (scene.name == "SelectModeScene")
        {
            inputEnabled = false;
            Invoke("EnableInput", ignoreInputTime); // ��莞�Ԍ�� PAD ���͂�L��������
            SoundManager.Instance.PlayUIClip("�V�[�����[�h�T�E���h");
        }
    }

    // PAD ���͂�L�������郁�\�b�h
    void EnableInput()
    {
        inputEnabled = true;
    }
}
