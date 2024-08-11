using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ColAnimationEvent : MonoBehaviour
{
    //�U���I���C�x���g
    public event Action<bool> OnAttackEnded;

    //�����蔻��
    //�E��A����
    [SerializeField]
    private CapsuleCollider rightArmCapsuleCol, leftArmCapsuleCol;

    //�������A�ӂ���͂�
    [SerializeField]
    private CapsuleCollider rightLegUpCapsuleCol, rightLegCapsuleCol;
    [SerializeField]
    private CapsuleCollider leftLegUpCapsuleCol, leftLegCapsuleCol;

    //���K
    [SerializeField]
    private BoxCollider rightHipCapsuleCol, leftHipCapsuleCol;

    //�g�Ռ�
    [SerializeField]
    private CapsuleCollider rightHasyoCol, leftHasyoCol;

    //������
    [SerializeField]
    private CapsuleCollider rightSyoryuCol, leftSyoryuCol;

    private Dictionary<string, Collider> colliders;

    [SerializeField]
    private bool mirror;

    [NonSerialized]
    public Animator anim;

    private void Start()
    {
        // �A�j���[�^�[�R���|�[�l���g�̎擾�ƃk���`�F�b�N
        if (!TryGetComponent(out anim))
        {
            Debug.LogError("Animator �R���|�[�l���g��������܂���B");
            return;
        }

        // �R���C�_�[�������ɓo�^
        colliders = new Dictionary<string, Collider>()
        {
            { nameof(rightArmCapsuleCol), rightArmCapsuleCol },
            { nameof(leftArmCapsuleCol), leftArmCapsuleCol },
            { nameof(rightLegUpCapsuleCol), rightLegUpCapsuleCol },
            { nameof(rightLegCapsuleCol), rightLegCapsuleCol },
            { nameof(leftLegUpCapsuleCol), leftLegUpCapsuleCol },
            { nameof(leftLegCapsuleCol), leftLegCapsuleCol },
            { nameof(rightHipCapsuleCol), rightHipCapsuleCol },
            { nameof(leftHipCapsuleCol), leftHipCapsuleCol },
            { nameof(rightHasyoCol), rightHasyoCol },
            { nameof(leftHasyoCol), leftHasyoCol },
            { nameof(rightSyoryuCol), rightSyoryuCol },
            { nameof(leftSyoryuCol), leftSyoryuCol },
        };

        // �R���C�_�[���ݒ肳��Ă��邩�`�F�b�N
        foreach (var collider in colliders.Values)
        {
            if (collider == null)
            {
                Debug.LogError("�J�v�Z���R���C�_�[���ݒ肳��Ă��Ȃ��ӏ�������܂��B");
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        // �~���[�����O�t���O�̍X�V
        if (anim != null)
        {
            mirror = anim.GetBool("Mirror");
        }
    }

    /// <summary>
    /// �R���C�_�[�̗L��/������ݒ�
    /// </summary>
    /// <param name="colliderName">�R���C�_�[�̖��O</param>
    /// <param name="enable">�L��/�����t���O</param>
    private void EnableCollider(string colliderName, bool enable)
    {
        if (colliders.TryGetValue(colliderName, out Collider collider))
        {
            collider.enabled = enable;
        }
        else
        {
            Debug.LogError($"'{colliderName}' �̃R���C�_�[��������܂���B");
        }
    }

    /// <summary>
    /// �U���������n���h��
    /// </summary>
    /// <param name="colliderName">�R���C�_�[�̖��O</param>
    private void HandleAttack(string colliderName)
    {
        EnableCollider(mirror ? GetMirrorColliderName(colliderName) : colliderName, true);
    }

    /// <summary>
    /// �~���[�����O���̃R���C�_�[�����擾
    /// </summary>
    /// <param name="colliderName">���̃R���C�_�[��</param>
    /// <returns>�~���[�����O���̃R���C�_�[��</returns>
    private string GetMirrorColliderName(string colliderName)
    {
        return colliderName switch
        {
            nameof(rightArmCapsuleCol) => nameof(leftArmCapsuleCol),
            nameof(leftArmCapsuleCol) => nameof(rightArmCapsuleCol),
            nameof(rightLegUpCapsuleCol) => nameof(leftLegUpCapsuleCol),
            nameof(leftLegUpCapsuleCol) => nameof(rightLegUpCapsuleCol),
            nameof(rightLegCapsuleCol) => nameof(leftLegCapsuleCol),
            nameof(leftLegCapsuleCol) => nameof(rightLegCapsuleCol),
            nameof(rightHipCapsuleCol) => nameof(leftHipCapsuleCol),
            nameof(leftHipCapsuleCol) => nameof(rightHipCapsuleCol),
            nameof(rightHasyoCol) => nameof(leftHasyoCol),
            nameof(leftHasyoCol) => nameof(rightHasyoCol),
            nameof(rightSyoryuCol) => nameof(leftSyoryuCol),
            nameof(leftSyoryuCol) => nameof(rightSyoryuCol),
            _ => colliderName
        };
    }

    // �e�U�����\�b�h
    void RightArmAttack() => HandleAttack(nameof(rightArmCapsuleCol));
    void LeftArmAttack() => HandleAttack(nameof(leftArmCapsuleCol));
    void RightLegUpAttack() => HandleAttack(nameof(rightLegUpCapsuleCol));
    void RightLegAttack() => HandleAttack(nameof(rightLegCapsuleCol));
    void LeftLegUpAttack() => HandleAttack(nameof(leftLegUpCapsuleCol));
    void LeftLegAttack() => HandleAttack(nameof(leftLegCapsuleCol));
    void LeftHipAttack() => HandleAttack(nameof(leftHipCapsuleCol));
    void Hasyogeki() => HandleAttack(nameof(rightHasyoCol));
    void SyoryuAttack() => HandleAttack(nameof(rightSyoryuCol));

    /// <summary>
    /// �U���I������
    /// </summary>
    void AttackFinish()
    {
        foreach (var collider in colliders.Values)
        {
            collider.enabled = false;
        }
        OnAttackEnded?.Invoke(false);
    }

    /// <summary>
    /// �U���I�����O������Ăяo��
    /// </summary>
    public void Finish()
    {
        AttackFinish();
    }
}