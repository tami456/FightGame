using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCollider : MonoBehaviour
{
    // �`���[�g���A���̃X�e�b�v���`����񋓌^
    private enum Tutorial
    {
        MoveToTarget,
        Step,
        Jump,
        N_Weak,
        N_Middle,
        N_Strong,
        Crouching,
        JumpAttack,
        TutorialFinish,
        None
    }

    [SerializeField]
    private SandBag sandbag; // �T���h�o�b�O�̎Q��
    private Tutorial tutorial; // ���݂̃`���[�g���A���X�e�b�v

    [SerializeField]
    private PlayerAction player; // �v���C���[
    [SerializeField]
    private PlayerAction tuto; // �`���[�g���A���p�̓G

    [SerializeField]
    private List<GameObject> targetObj = new List<GameObject>(); // �ڕW�I�u�W�F�N�g�̃��X�g
    [SerializeField]
    private List<GameObject> targetText = new List<GameObject>(); // �ڕW�e�L�X�g�̃��X�g
    [SerializeField]
    private List<Transform> targetsPos = new List<Transform>(); // �ڕW�ʒu�̃��X�g
    private int currentIndex = 0; // ���݂̖ڕW�C���f�b�N�X

    [SerializeField]
    private Animator anim; // �A�j���[�^�[
    private bool isJump = false; // �W�����v��Ԃ̃t���O

    private void Start()
    {
        tutorial = Tutorial.MoveToTarget; // �����X�e�b�v��ݒ�
    }

    private void Update()
    {
        // ���݂̃`���[�g���A���X�e�b�v�ɉ����ď��������s
        switch (tutorial)
        {
            case Tutorial.MoveToTarget:
                MoveToTarget();
                break;
            case Tutorial.Step:
                Step();
                break;
            case Tutorial.Jump:
                Jump();
                break;
            case Tutorial.N_Weak:
                N_Weak();
                break;
            case Tutorial.N_Middle:
                N_Middle();
                break;
            case Tutorial.N_Strong:
                N_Strong();
                break;
            case Tutorial.Crouching:
                Crouching();
                break;
            case Tutorial.JumpAttack:
                JumpAttack();
                break;
            case Tutorial.TutorialFinish:
                TutorialFinish();
                break;
        }
    }

    // ����̈ʒu�܂ŕ���
    private void MoveToTarget()
    {
        // �L�����N�^�[���ڕW�n�_�ɓ��B���������m�F����
        if (Mathf.Abs(player.controller.transform.position.x - targetsPos[currentIndex].position.x) < 2f)
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 1 completed!");
            UpdateTutorialStep();
            // ���̃X�e�b�v�Ɉڍs����
            tutorial = Tutorial.Step;
        }
    }

    // ����Ɍ������ăX�e�b�v
    private void Step()
    {
        bool stepR = anim.GetBool("StepR");
        if (stepR)
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 2 completed!");
            UpdateTutorialStep();
            // ���̃X�e�b�v�Ɉڍs
            tutorial = Tutorial.Jump;
        }
    }

    // �W�����v
    private void Jump()
    {
        if (Mathf.Abs(player.controller.transform.position.y - targetsPos[currentIndex].position.y) < 0.1f)
        {
            isJump = true;
        }

        if (!isJump)
        {
            return;
        }

        if (player.GetIsGround())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 3 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.N_Weak;
        }
    }

    // ����ɑ΂��Ď�U��
    private void N_Weak()
    {
        if (sandbag.GetIsWeak())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 4 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.N_Middle;
        }
    }

    // ����ɑ΂��Ē��U��
    private void N_Middle()
    {
        if (sandbag.GetIsMiddle())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 5 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.N_Strong;
        }
    }

    // ����ɑ΂��ċ��U��
    private void N_Strong()
    {
        if (sandbag.GetIsStrong())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 6 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.Crouching;
        }
    }

    // ���Ⴊ��
    private void Crouching()
    {
        bool isCrouching = anim.GetBool("Crouching");
        if (isCrouching)
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 7 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.JumpAttack;
        }
    }

    // ����ɑ΂��ăW�����v�U��
    private void JumpAttack()
    {
        if (sandbag.GetIsJumpAttack())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 8 completed!");
            UpdateTutorialStep();
            tuto.SetDefaultPos();
            tutorial = Tutorial.TutorialFinish;
        }
    }

    // �`���[�g���A���I������
    private void TutorialFinish()
    {
        // �`���[�g���A���J�n�̏���
        UpdateTutorialStep();
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("SelectModeScene");
        }
    }

    // �`���[�g���A�����N���A�����Ƃ��̏���
    private void UpdateTutorialStep()
    {
        StartCoroutine(NextTutorial());
        player.animState.SetAnimFalse("Crouching");
        // �`���[�g���A�����N���A������A���̃`���[�g���A�����n�܂�܂œ����Ȃ��悤�ɂ���
        player.State = PlayerAction.MyState.Freeze;
        // �`���[�g���A���J�n�̏���
        StartCoroutine(WarpAndToggleObjects());
    }

    // �v���C���[���f�t�H���g�̈ʒu�Ƀ��[�v���A�ڕW�I�u�W�F�N�g��؂�ւ���
    IEnumerator WarpAndToggleObjects()
    {
        yield return new WaitForSeconds(1.0f);
        // �v���C���[���f�t�H���g�̈ʒu�Ƀ��[�v
        player.SetDefaultPos();
        player.animState.ResetAnim();
        player.State = PlayerAction.MyState.Game;
        // ���̖ڕW�I�u�W�F�N�g��false�ɂ��āA���̖ڕW�I�u�W�F�N�g��true�ɂ���
        targetObj[currentIndex].SetActive(false);
        targetObj[currentIndex + 1].SetActive(true);
        targetText[currentIndex].SetActive(false);
        targetText[currentIndex + 1].SetActive(true);
        currentIndex += 1;
    }

    // ���̃`���[�g���A���X�e�b�v�ɐi��
    private IEnumerator NextTutorial()
    {
        yield return FadeController.Instance.StartCoroutine(FadeController.Instance.FadeOut());
        FadeController.Instance.StartCoroutine(FadeController.Instance.FadeIn());
    }
}
