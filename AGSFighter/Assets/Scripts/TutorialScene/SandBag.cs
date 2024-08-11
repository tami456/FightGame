using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandBag : MonoBehaviour
{
    [SerializeField]
    TutorialCollider tuto;
    //�`���[�g���A���e�X�g�p�̓G
    [SerializeField]
    private Animator anim;
    private AnimationState state;
    private bool isWeak = false;
    private bool isMiddle = false;
    private bool isStrong = false;
    private bool isJumpAttack = false;
    Animator animator;
    [SerializeField]
    GameObject impactEffect,time;

    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<AnimationState>();
        animator = GetComponent<Animator>();
        animator.SetBool("Mirror", true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Attack")
        {
            // �Փ˂����ꏊ�ɃG�t�F�N�g�𐶐�
            Vector3 position = other.ClosestPoint(transform.position); // �Փ˂����ꏊ�̋߂��̓_���擾
            Instantiate(impactEffect, position, Quaternion.identity);

            if (anim.GetBool("N_Weak"))
            {
                isWeak = true;
            }
            else if(anim.GetBool("N_Mid"))
            {
                isMiddle = true;
            }
            else if(anim.GetBool("N_Stro"))
            {
                isStrong = true;
            }
            else if(anim.GetBool("JumpKick") || anim.GetBool("JumpKick2") || anim.GetBool("JumpPunch"))
            {
                isJumpAttack = true;
            }
        }
    }

    public bool GetIsWeak()
    {
        return isWeak;
    }

    public bool GetIsMiddle()
    {
        return isMiddle;
    }

    public bool GetIsStrong()
    {
        return isStrong;
    }

    public bool GetIsJumpAttack()
    {
        return isJumpAttack;
    }
}
