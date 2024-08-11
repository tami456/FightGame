using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �A�j���[�V�����̏�Ԃ��Ǘ�����R���|�[�l���g
public class AnimationState : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMove(Vector3 moveDirection)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.x);
        }
    }
    
    //Jump�̏��(�O�W�����v�A���W�����v�A�����W�����v)
    public void SetJumpState(int jump)
    {
        if (animator != null)
        {
            animator.SetInteger("Jump", jump);
        }
    }

    //�W�����v���Ă��邩�ǂ���
    public void SetJump(bool jump)
    {
        if (animator != null)
        {
            animator.SetBool("JumpSF", jump);
        }
    }

    //�A�j���[�V������true�ɂ���
    public void SetAnimTrue(string name)
    {
        if (animator != null)
        {
            animator.SetBool(name, true);
        }
    }

    //�A�j���[�V������false�ɂ���
    public void SetAnimFalse(string name)
    {
        if (animator != null)
        {
            animator.SetBool(name, false);
        }
    }

    public void SetAnimTrigger(string name)
    {
        if (animator != null)
        {
            animator.SetTrigger(name);
        }
    }

    public void ResetAnim()
    {
        SetAnimFalse("NA_Mid");
        SetAnimFalse("NA_Stro");
        SetAnimFalse("N_Weak");
        SetAnimFalse("N_Mid");
        SetAnimFalse("N_Stro");
        SetAnimFalse("C_Weak");
        SetAnimFalse("C_Mid");
        SetAnimFalse("Ashibarai");
        SetAnimFalse("CA_Weak");
        SetAnimFalse("CA_Mid");
        SetAnimFalse("Hadoken");
        SetAnimFalse("Syoryuken");
        SetAnimFalse("StepR");
        SetAnimFalse("StepL");
        SetAnimFalse("StandingGuard");
        SetAnimFalse("CrouchingGuard");
        SetAnimFalse("FrontMove");
        SetAnimFalse("BackMove");
        SetAnimFalse("ThrowMiss");
        SetAnimFalse("Sakotsuwari");
    }

    public void ResetDamageAnim()
    {
        SetAnimFalse("SWeakHit");
        SetAnimFalse("SMidHit");
        SetAnimFalse("SStrongHit");
        SetAnimFalse("Huttobi");
    }
}
