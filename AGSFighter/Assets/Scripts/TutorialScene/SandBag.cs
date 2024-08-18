using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandBag : MonoBehaviour
{
    [SerializeField]
    private TutorialCollider tuto; // �`���[�g���A���p�̃R���C�_�[
    [SerializeField]
    private Animator anim; // �A�j���[�^�[�R���|�[�l���g
    private AnimationState state; // �A�j���[�V�����X�e�[�g
    private bool isWeak = false; // ��U���t���O
    private bool isMiddle = false; // ���U���t���O
    private bool isStrong = false; // ���U���t���O
    private bool isJumpAttack = false; // �W�����v�U���t���O
    private Animator animator; // �A�j���[�^�[
    [SerializeField]
    private GameObject impactEffect; // �Փ˃G�t�F�N�g
    [SerializeField]
    private GameObject time; // ���ԃI�u�W�F�N�g�i�p�r�s���j

    // �e�U���t���O�̃Q�b�^�[
    public bool GetIsWeak() => isWeak;
    public bool GetIsMiddle() => isMiddle;
    public bool GetIsStrong() => isStrong;
    public bool GetIsJumpAttack() => isJumpAttack;

    // Awake�̓X�N���v�g�C���X�^���X�����[�h���ꂽ�Ƃ��ɌĂяo�����
    void Awake()
    {
        state = GetComponent<AnimationState>();
        animator = GetComponent<Animator>();
        animator.SetBool("Mirror", true);
    }

    // �g���K�[�ɏՓ˂����Ƃ��ɌĂяo�����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            // �Փ˂����ꏊ�ɃG�t�F�N�g�𐶐�
            Vector3 position = other.ClosestPoint(transform.position); // �Փ˂����ꏊ�̋߂��̓_���擾
            Instantiate(impactEffect, position, Quaternion.identity);

            // �U���̎�ނɉ����ăt���O��ݒ�
            isWeak = anim.GetBool("N_Weak");
            isMiddle = anim.GetBool("N_Mid");
            isStrong = anim.GetBool("N_Stro");
            isJumpAttack = anim.GetBool("JumpKick") || anim.GetBool("JumpKick2") || anim.GetBool("JumpPunch");
        }
    }

}
