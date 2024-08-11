using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private CapsuleCollider leftArmCapsuleCol;
    [SerializeField] private CapsuleCollider rightArmCapsuleCol;
    public bool mirror;
    [SerializeField] private Animator anim;

    private string currentAttackType;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        mirror = anim.GetBool("Mirror");
    }

    void EnableAttack(string attackType)
    {
        currentAttackType = attackType;

        // �R���W������L���ɂ���
        if (mirror == false)
        {
            rightArmCapsuleCol.enabled = true;
        }
        else
        {
            leftArmCapsuleCol.enabled = true;
        }
    }

    void AttackFinish()
    {
        rightArmCapsuleCol.enabled = false;
        leftArmCapsuleCol.enabled = false;
    }

    // ���݂̍U���^�C�v���擾����
    public string GetCurrentAttackType()
    {
        return currentAttackType;
    }

    // �A�j���[�V�����C�x���g�p�̃��\�b�h
    public void SetWeakAttack()
    {
        EnableAttack("N_Weak");
    }

    public void SetMediumAttack()
    {
        EnableAttack("N_Mid");
    }

    public void SetStrongAttack1()
    {
        EnableAttack("N_Stro");
    }

    public void SetStrongAttack2()
    {
        EnableAttack("NA_Stro");
    }

    // �V�����ǉ����ꂽ���r�̍U����L���ɂ��郁�\�b�h
    public void LeftArmAttack()
    {
        // ���r�̍U���^�C�v��ݒ�i�K�؂ȍU���^�C�v�ɒu�������Ă��������j
        EnableAttack("N_Weak");
        Debug.Log("���r�̍U�����L���ɂȂ�܂����B");
    }
}
