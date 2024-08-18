using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.Utilities;

public class PlayerInputSystem : MonoBehaviour
{
    // �v���C���[�֘A�̕ϐ�
    private PlayerAction player; // �v���C���[�̃A�N�V�������Ǘ�����N���X
    private PlayerHit pHit; // �v���C���[�̃q�b�g�����Ǘ�����N���X
    private PlayerAnimationEvent playerAnim; // �v���C���[�̃A�j���[�V�����C�x���g���Ǘ�����N���X
    private AnimationState animState; // �A�j���[�V�����̏�Ԃ��Ǘ�����N���X
    private PlayerGuard playerGuard; // �v���C���[�̃K�[�h��Ԃ��Ǘ�����N���X
    private PlayerGuardPose playerGuardPose; // �v���C���[�̃K�[�h�|�[�Y���Ǘ�����N���X

    // �����̗ʂ��Ǘ�����ϐ�
    private Vector3 moveDirection; // �v���C���[�̈ړ�����
    private float speed; // �v���C���[�̈ړ����x

    // �X�e�b�v���̈ړ��ʂ��Ǘ�����ϐ�
    [SerializeField]
    private float step = 10.0f; // �X�e�b�v���̈ړ���

    // ���͊m�F�p�̕ϐ�
    private float rv = 0.0f; // �E�����̓��͒l
    private float lv = 0.0f; // �������̓��͒l
    private float uv = 0.0f; // ������̓��͒l
    private float dv = 0.0f; // �������̓��͒l

    // �W�����v�֘A�̕ϐ�
    private bool isJump = false; // �W�����v�����ǂ����̃t���O
    private float jumpSpeed = 50.0f; // �W�����v�̑��x
    private float jumpSpeedX = 10.0f; // �W�����v�̉������̑��x

    // �Z�̏�Ԃ��Ǘ�����ϐ�
    [SerializeField]
    private bool attackFlag = false; // �U�������ǂ����̃t���O

    // �W�����v�̏�Ԃ��Ǘ�����ϐ�
    [SerializeField]
    private bool jumpFlag = false; // �W�����v�����ǂ����̃t���O

    // �K�[�h�֘A�̕ϐ�
    [SerializeField]
    float guard = 0.0f; // �K�[�h�̋��x
    [SerializeField]
    private float guardMove = 0.0f; // �K�[�h���̈ړ���
    public bool guardSFlag = false; // �����K�[�h�̃t���O
    public bool guardSPFlag = false; // �����K�[�h�|�[�Y�̃t���O
    public bool guardCFlag = false; // ���Ⴊ�݃K�[�h�̃t���O
    public bool guardCPFlag = false; // ���Ⴊ�݃K�[�h�|�[�Y�̃t���O
    [SerializeField]
    private bool assistFlag = false; // �A�V�X�g�{�^���������Ă��邩�t���O
    private const float RecoilTime = 0.3f; // ���R�C�����Ԃ̒萔


    //Get�֐�Set�֐�
    public bool AttackFlag
    {
        get { return attackFlag; }
        set { attackFlag = value; }
    }
    public bool IsJump
    {
        get { return isJump; }
        set { isJump = value; }
    }

    public bool JumpFlag
    {
        get { return jumpFlag; }
        set { jumpFlag = value; }
    }

    public bool GuardSFlag => guardSFlag;

    public bool GuardCFlag => guardCFlag;

    //�E�����̓��͂̎󂯓n��
    public float RV() => rv;

    //�������̓��͂̎󂯓n��
    public float LV() => lv;

    //������̓��͂̎󂯓n��
    public float UV() => uv;

    //�������̓��͂̎󂯓n��
    public float DV() => dv;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerAction>();
        pHit = GetComponent<PlayerHit>();
        playerAnim = GetComponent<PlayerAnimationEvent>();
        animState = GetComponent<AnimationState>();
        playerGuard = this.GetComponentInChildren<PlayerGuard>();
        playerGuardPose = this.GetComponentInChildren<PlayerGuardPose>();
        speed = player.GetSpeed();
    }

    private void FixedUpdate()
    {
        HandleStep();
        HandleGuard();
    }

    // �X�e�b�v�����𕪗�
    private void HandleStep()
    {
        if (playerAnim.StepL)
        {
            MovePlayer(-step);
            SetStepAnimation("StepL", "StepR");
        }
        else if (playerAnim.StepR)
        {
            MovePlayer(step);
            SetStepAnimation("StepR", "StepL");
        }
    }

    // �v���C���[�̈ړ�����
    private void MovePlayer(float step)
    {
        Vector3 moveDirection = new Vector3(step, 0, 0) * Time.deltaTime;
        player.controller.Move(moveDirection);
        player.animState.SetAnimFalse("BackMove");
        player.animState.SetAnimFalse("FrontMove");
    }

    // �X�e�b�v�A�j���[�V�����̐ݒ�
    private void SetStepAnimation(string animTrue, string animFalse)
    {
        if (!player.anim.GetBool("Mirror"))
        {
            player.animState.SetAnimTrue(animTrue);
        }
        else
        {
            player.animState.SetAnimTrue(animFalse);
        }
    }

    // �K�[�h�����𕪗�
    private void HandleGuard()
    {
        if (guardSFlag || guardCFlag)
        {
            guard++;
        }

        if (pHit.Damage.Item3 != AttackLevel.NullLevel)
        {
            HandleGuardState();
        }

        if (!playerGuard.IsGuard && !playerGuardPose.IsGuard && guard > 50f)
        {
            ResetGuard();
        }
    }

    // �K�[�h��Ԃ̏���
    private void HandleGuardState()
    {
        bool isMirror = player.anim.GetBool("Mirror");
        bool isLeftValid = (!isMirror && lv == 1);
        bool isRightValid = (isMirror && rv == 1);

        if (isLeftValid || isRightValid)
        {
            if (!player.IsCrouching && playerGuard.IsGuard && !guardSFlag)
            {
                ExecuteGuard("StandingGuard");
                playerGuard.InstantiateGuardEffect();
                guardSFlag = true;
                StartCoroutine(Recoil(isLeftValid ? 2f : -2f));
            }
            else if (!player.IsCrouching && playerGuardPose.IsGuard && !guardSPFlag)
            {
                ExecuteGuard("StandingGuard");
                guardSPFlag = true;
                StartCoroutine(Recoil(isLeftValid ? 2f : -2f));
            }

            if (player.IsCrouching && playerGuard.IsGuard && !guardCFlag
                && (pHit.Damage.Item3 == AttackLevel.Low || pHit.Damage.Item3 == AttackLevel.High))
            {
                ExecuteGuard("CrouchingGuard");
                playerGuard.InstantiateGuardEffect();
                guardCFlag = true;
                StartCoroutine(Recoil(isLeftValid ? 2f : -2f));
            }
            else if (player.IsCrouching && playerGuardPose.IsGuard && !guardCPFlag)
            {
                ExecuteGuard("CrouchingGuard");
                guardCPFlag = true;
                StartCoroutine(Recoil(isLeftValid ? 2f : -2f));
            }
        }
    }

    // �K�[�h�̃��Z�b�g
    private void ResetGuard()
    {
        guardSFlag = false;
        guardSPFlag = false;
        guardCFlag = false;
        guardCPFlag = false;
        playerAnim.CallCancel();
        guard = 0.0f;
        player.animState.SetAnimFalse("StandingGuard");
        player.animState.SetAnimFalse("CrouchingGuard");
        player.DetectPadHorizontalInput();
    }


    //InputSystem
    //�v���C���[�ړ��E����
    public void OnMoveFront(InputAction.CallbackContext context)
    {
        // �ړ��������󂯎��
        var v = context.ReadValue<float>();
        rv = v;

        if (CanMove())
        {
            //�v���C���[�ړ�
            moveDirection = new Vector3(-v, 0.0f, 0.0f);
            moveDirection = transform.TransformDirection(moveDirection);

            animState.ResetAnim();

            if (!player.anim.GetBool("Mirror"))
            {
                moveDirection = moveDirection * speed;
                player.animState.SetAnimTrue("FrontMove");
            }
            else
            {
                moveDirection = moveDirection * speed / 1.5f;
                player.animState.SetAnimTrue("BackMove");
            }

            if (context.canceled)
            {
                ResetMove();
            }

            player.MoveDirection = moveDirection;
        }
    }

    //������
    public void OnMoveBack(InputAction.CallbackContext context)
    {
        // �ړ��������󂯎��
        var v = context.ReadValue<float>();
        lv = v;

        if (CanMove())
        {
            //�v���C���[�ړ�
            moveDirection = new Vector3(v, 0.0f, 0.0f);
            moveDirection = transform.TransformDirection(moveDirection);

            animState.ResetAnim();

            if (!player.anim.GetBool("Mirror"))
            {
                moveDirection = moveDirection * speed / 1.5f;
                player.animState.SetAnimTrue("BackMove");
            }
            else
            {
                moveDirection = moveDirection * speed;
                player.animState.SetAnimTrue("FrontMove");
            }

            if (context.canceled)
            {
                ResetMove();
            }

            player.MoveDirection = moveDirection;
        }
    }

    //�X�e�b�v
    public void OnStep(InputAction.CallbackContext context)
    {
        if(!context.started)
        {
            return;
        }

        if (player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && !playerAnim.StepL && !playerAnim.StepR
            && !player.IsCrouching && player.anim.GetBool("Cancel"))
        {
            player.animState.ResetAnim();
            moveDirection.y = 0.0f;

            if (lv == 1)
            {
                playerAnim.StepL = true;
            }
            else if (rv == 1)
            {
                playerAnim.StepR = true;
            }
        }
    }

    //�W�����v
    public void OnJump(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        uv = v;

        if (!context.started)
        {
            return;
        }

        if (CanJump())
        {
            jumpFlag = true;
            animState.ResetAnim();
            player.IsJumpVertical = true;
            StartCoroutine(Count());
            moveDirection.x = 0.0f;
            moveDirection.y = jumpSpeed;
            player.animState.SetJump(true);

            player.MoveDirection = moveDirection;
        }
    }

    //�O�W�����v
    public void OnJumpR(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (CanJump())
        {
            animState.ResetAnim();
            jumpFlag = true;
            if (!player.anim.GetBool("Mirror"))
            {
                player.IsJumpFront = true;
            }
            else
            {
                player.IsJumpBack = true;
            }
            StartCoroutine(Count());
            //�W�����v���̈ړ���
            moveDirection.x = jumpSpeedX;
            moveDirection.y = jumpSpeed;
            player.animState.SetJump(true);

            player.MoveDirection = moveDirection;
        }
    }

    //���W�����v
    public void OnJumpL(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (CanJump())
        {
            animState.ResetAnim();
            jumpFlag = true;
            if (!player.anim.GetBool("Mirror"))
            {
                player.IsJumpBack = true;
            }
            else
            {
                player.IsJumpFront = true;
            }
            StartCoroutine(Count());
            //�W�����v���̈ړ���
            moveDirection.x = -jumpSpeedX;
            moveDirection.y = jumpSpeed;
            player.animState.SetJump(true);

            player.MoveDirection = moveDirection;
        }
    }

    //�A�V�X�g�{�^��
    public void AssistButton(InputAction.CallbackContext context)
    {
        assistFlag = true;
        if(context.canceled)
        {
            assistFlag = false;
        }
    }

    //��U��
    public void OnNWP(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        if (CanNormalAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("N_Weak");
        }
        else if(CanJumpAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteJumpAttack("JumpKick");
        }
        else if (CanNormalAttack(player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("C_Weak");
        }
        else if(CanNormalAttack(!player.IsCrouching) && assistFlag)
        {
            ExecuteCombo("NA_Weak");
        }
    }

    //���U��
    public void OnNMP(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (CanNormalAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("N_Mid");
        }
        else if (CanJumpAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteJumpAttack("JumpKick2");
        }
        else if (CanNormalAttack(player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("C_Mid");
        }
        else if(CanNormalAttack(!player.IsCrouching) && assistFlag)
        {
            ExecuteCombo("NA_Mid");
        }
    }

    //���U��
    public void OnNSP(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (CanNormalAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("N_Stro");
        }
        else if (CanJumpAttack(!player.IsCrouching) && !assistFlag)
        {
            ExecuteJumpAttack("JumpPunch");
        }
        else if (CanNormalAttack(player.IsCrouching) && !assistFlag)
        {
            ExecuteAttack("Ashibarai");
        }
        else if (CanNormalAttack(!player.IsCrouching) && assistFlag)
        {
            ExecuteCombo("NA_Stro");
        }
    }

    //��������
    public void OnSakotsuwari(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, !player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Sakotsuwari");
        }
    }

    //��������
    public void OnMirrorSakotsuwari(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Sakotsuwari");
        }
    }

    //�g����
    public void OnHadoken(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && !player.IsCrouching && !attackFlag
            && player.anim.GetBool("Cancel"))
        {
            ExecuteAttack("Hadoken");
        }
    }

    //������
    public void OnSyoryuken(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, !player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Syoryuken");
        }
    }

    //���������]
    public void OnMirrorSyoryuken(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching,player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Syoryuken");
        }
    }

    //���������r
    public void OnTatsumaki(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, !player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Tatsumaki");
        }
    }

    //�������]
    public void OnMirrorTatsumaki(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (MirrorAnimation(!player.IsCrouching, player.anim.GetBool("Mirror")))
        {
            ExecuteAttack("Tatsumaki");
        }
    }

    //�g�Ռ�
    public void OnHasyogeki(InputAction.CallbackContext context)
    {
        if(!context.started)
        {
            return;
        }

        if (player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && player.IsCrouching && !attackFlag && player.anim.GetBool("Cancel"))
        {
            ExecuteAttack("Hasyogeki");
        }

    }

    //���Ⴊ��
    public void OnCrouching(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        dv = v;

        if (player.State == PlayerAction.MyState.Game && player.GetIsGround())
        {
            if (context.started)
            {
                player.animState.SetAnimTrue("CStart");
                player.animState.SetAnimFalse("FrontMove");
                player.animState.SetAnimFalse("BackMove");
                player.IsCrouching = true;
                moveDirection = Vector3.zero;
                player.MoveDirection = moveDirection;
            }
            else if (context.canceled)
            {
                player.animState.SetAnimFalse("Crouching");
                player.animState.SetAnimFalse("CStart");
                player.IsCrouching = false;
                //���E���͊m�F
                player.DetectPadHorizontalInput();
            }
        }
    }

    //�A�j���[�V�������Z�b�g
    private void ResetMove()
    {
        player.animState.SetAnimFalse("FrontMove");
        player.animState.SetAnimFalse("BackMove");
        player.animState.SetAnimFalse("StandingGuard");
        player.animState.SetAnimFalse("CrouchingGuard");
    }

    //�U�����̃A�j���[�V�����ƈړ�����
    //�ʏ�U���ƕK�E�Z
    private void ExecuteAttack(string attackName)
    {
        if(player.anim.GetBool(attackName))
        {
            player.animState.SetAnimFalse(attackName);
            StartCoroutine(DelayAnim(attackName));
            return;
        }

        animState.ResetAnim();

        player.animState.SetAnimTrue(attackName);
        attackFlag = true;
        moveDirection *= 0;
        player.MoveDirection = moveDirection;
    }

    //�A�V�X�g�R���{
    private void ExecuteCombo(string attackName)
    {
        if (player.anim.GetBool(attackName))
        {
            StartCoroutine(DelayAnim(attackName));
            return;
        }

        animState.ResetAnim();

        player.animState.SetAnimTrigger(attackName);
        attackFlag = true;
        moveDirection *= 0;
        player.MoveDirection = moveDirection;
    }

    //�K�[�h
    private void ExecuteGuard(string guardName)
    {
        animState.ResetAnim();

        player.animState.SetAnimTrue(guardName);
        player.MoveDirection = moveDirection;
    }

    //�W�����v�U��
    private void ExecuteJumpAttack(string attackName)
    {
        player.animState.SetAnimTrue(attackName);
        attackFlag = true;
    }

    //�񓯊�����
    //�W�����v�����u�Ԃ�true�ɂ���ƃW�����v���Ȃ��Ȃ�̂Œx��������
    public IEnumerator Count()
    {
        yield return new WaitForSeconds(0.5f);
        isJump = true;
    }

    //�K�[�h���̌ジ����
    IEnumerator Recoil(float speed)
    {
        float recoilSpeed = speed;
        float timeElapsed = 0.0f;

        while (timeElapsed < RecoilTime)
        {
            float currentSpeed = recoilSpeed * Mathf.Sqrt(1 - Mathf.Pow(timeElapsed / RecoilTime, 2));
            moveDirection -= new Vector3(currentSpeed * Time.deltaTime, 0, 0);
            timeElapsed += Time.deltaTime;
            player.MoveDirection = moveDirection;
            yield return null;
        }

        playerGuard.IsGuard = false;
        playerGuardPose.IsGuard = false;
        player.DetectPadHorizontalInput();
    }

    //�L�����Z���t���[�����ɓ����U����A���ŏo����悤�ɂ���
    IEnumerator DelayAnim(string attackName)
    {
        yield return new WaitForSeconds(0.001f);

        player.animState.SetAnimTrue(attackName);
        attackFlag = true;
        moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }

    //bool�֐�
    //�ړ��ł��邩
    private bool CanMove()
    {
        return player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && !player.IsCrouching && !playerAnim.StepL && !playerAnim.StepR
            && player.anim.GetBool("Cancel") && player.anim.GetInteger("Jump") == 0
            && !player.anim.GetBool("JumpSF") && !playerGuard.IsGuard;
    }

    //�W�����v�ł��邩
    public bool CanJump()
    {
        return player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && !player.IsCrouching && !playerAnim.StepL && !playerAnim.StepR
            && player.anim.GetBool("JumpSF") == false && jumpFlag == false
            && player.anim.GetBool("Cancel");
    }

    //�U���ł��邩
    private bool CanNormalAttack(bool isCrouching)
    {
        return player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && isCrouching && !attackFlag && player.anim.GetBool("Cancel");
    }

    //�W�����v�ł��邩
    private bool CanJumpAttack(bool isCrouching)
    {
        return player.State == PlayerAction.MyState.Game && player.anim.GetBool("JumpSF") == true
            && isCrouching && !attackFlag && player.anim.GetBool("Cancel");
    }

    //���]�������p�̃A�j���[�V����
    private bool MirrorAnimation(bool isCrouching, bool mirror)
    {
        return player.State == PlayerAction.MyState.Game && player.GetIsGround()
            && isCrouching && !attackFlag && mirror && player.anim.GetBool("Cancel");
    }
}
