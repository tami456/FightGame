using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCollider : MonoBehaviour
{
    public enum Tutorial
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
    private SandBag sandbag;
    [SerializeField]
    public Tutorial tutorial;

    [SerializeField]
    private CharacterController playerController;
    [SerializeField]
    private PlayerInputSystem playerInputSystem;
    [SerializeField]
    private PlayerAction player;
    [SerializeField]
    private PlayerAction tuto;
    [SerializeField]
    private List<GameObject> targetObj = new List<GameObject>();
    [SerializeField]
    private List<GameObject> targetText = new List<GameObject>();
    [SerializeField]
    private List<Transform> targetsPos = new List<Transform>();
    [SerializeField]
    private int currentIndex = 0;
    bool t = false;
    [SerializeField]
    private Animator anim;
    private bool isJump = false;

    private void Start()
    {
        tutorial = Tutorial.MoveToTarget;
        
    }

    private void Update()
    {
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

    //����̈ʒu�܂ŕ���
    private void MoveToTarget()
    {
        // �L�����N�^�[���ڕW�n�_�ɓ��B���������m�F����
        if (DistanceX(playerController.transform.position.x, 
            targetsPos[currentIndex].position.x) < 2f)
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 1 completed!");
            UpdateTutorialStep();
            // ���̃X�e�b�v�Ɉڍs����
            tutorial = Tutorial.Step;
        }
    }

    //����Ɍ������ăX�e�b�v
    private void Step()
    {
        bool stepR = anim.GetBool("StepR");
        if (stepR == true)
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 2 completed!");
            UpdateTutorialStep();
            //���̃X�e�b�v�Ɉڍs
            tutorial = Tutorial.Jump;
        }
    }

    //�W�����v
    private void Jump()
    {
        if(DistanceY(playerController.transform.position.y, 
            targetsPos[currentIndex].position.y) < 0.1f)
        {
            isJump = true;
        }

        if(!isJump)
        {
            return;
        }

        if(player.GetIsGround())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 3 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.N_Weak;
        }
    }

    //����ɑ΂��Ď�U��
    private void N_Weak()
    {
        if(sandbag.GetIsWeak())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 4 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.N_Middle;
        }
    }

    //����ɑ΂��Ē��U��
    private void N_Middle()
    {
        if(sandbag.GetIsMiddle())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 5 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.N_Strong;
        }
    }

    //����ɑ΂��ċ��U��
    private void N_Strong()
    {
        if(sandbag.GetIsStrong())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 6 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.Crouching;
        }
    }

    //���Ⴊ��
    private void Crouching()
    {
        bool isCrouching = anim.GetBool("Crouching");
        if(isCrouching)
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 7 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.JumpAttack;
        }
    }

    //����ɑ΂��ăW�����v�U��
    private void JumpAttack()
    {
        if(sandbag.GetIsJumpAttack())
        {
            // �X�e�b�v�����������ꍇ�̏���
            Debug.Log("Step 8 completed!");
            UpdateTutorialStep();
            tuto.SetDefaultPos();
            tutorial = Tutorial.TutorialFinish;
        }
    }

    private void TutorialFinish()
    {
        //�`���[�g���A���J�n�̏���
        UpdateTutorialStep();
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("SelectModeScene");
        }
    }

    //�`���[�g���A�����N���A�����Ƃ��̏���
    private void UpdateTutorialStep()
    {
        FadeController.Instance.SetDuration(1.0f);
        StartCoroutine(NextTutorial());
        player.animState.SetAnimFalse("Crouching");
        //�`���[�g���A�����N���A������A���̃`���[�g���A�����n�܂�܂œ����Ȃ��悤�ɂ���
        player.State = PlayerAction.MyState.Freeze;
        //�`���[�g���A���J�n�̏���
        StartCoroutine(WarpAndToggleObject());
    }


    IEnumerator WarpAndToggleObject()
    {
        yield return new WaitForSeconds(1.0f);
        //�v���C���[���f�t�H���g�̈ʒu�Ƀ��[�v
        player.SetDefaultPos();
        player.animState.ResetAnim();
        player.State = PlayerAction.MyState.Game;
        //���̖ڕW�I�u�W�F�N�g��false�ɂ��āA���̖ڕW�I�u�W�F�N�g��true�ɂ���
        targetObj[currentIndex].SetActive(false);
        targetObj[currentIndex + 1].SetActive(true);
        targetText[currentIndex].SetActive(false);
        targetText[currentIndex + 1].SetActive(true);
        currentIndex += 1;
    }
    private IEnumerator NextTutorial()
    {
        yield return FadeController.Instance.StartCoroutine(FadeController.Instance.FadeOut());
        FadeController.Instance.StartCoroutine(FadeController.Instance.FadeIn());
    }

    private float DistanceX(float x1,float x2)
    {
        return x2 - x1;
    }

    private float DistanceY(float y1, float y2)
    {
        return y2 - y1;
    }
}
