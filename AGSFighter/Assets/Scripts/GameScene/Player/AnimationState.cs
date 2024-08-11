using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アニメーションの状態を管理するコンポーネント
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
    
    //Jumpの状態(前ジャンプ、後ろジャンプ、垂直ジャンプ)
    public void SetJumpState(int jump)
    {
        if (animator != null)
        {
            animator.SetInteger("Jump", jump);
        }
    }

    //ジャンプしているかどうか
    public void SetJump(bool jump)
    {
        if (animator != null)
        {
            animator.SetBool("JumpSF", jump);
        }
    }

    //アニメーションをtrueにする
    public void SetAnimTrue(string name)
    {
        if (animator != null)
        {
            animator.SetBool(name, true);
        }
    }

    //アニメーションをfalseにする
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
