using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAnimationEvent : MonoBehaviour
{
    // �W�����v�p�X�e�[�g�̗񋓌^
    public enum JumpState
    {
        State0,
        State1,
        State2,
        State3,
        State4,
        State5,
    }

    private PlayerAction player; // �v���C���[�̃A�N�V����

    [SerializeField]
    private CinemachineImpulseSource cineCamera; // �J�����̃C���p���X�\�[�X

    private bool stepR, stepL = false; // �X�e�b�v�t���O

    // �g�����֘A�̃t�B�[���h
    [SerializeField]
    private GameObject hadoken; // �g�����̃Q�[���I�u�W�F�N�g
    [SerializeField]
    private Hadoken hadokenMove; // �g�����̓���
    private bool hadoFlag = false; // �g�����̃t���O
    [SerializeField]
    private GameObject hadokenPosR; // �g�����̉E���̈ʒu
    [SerializeField]
    private GameObject hadokenPosL; // �g�����̍����̈ʒu
    [SerializeField]
    private string HadoHitPlayer; // �g������������v���C���[�̃��C���[
    [SerializeField]
    private string originalLayer; // ���̃��C���[

    // �A�j���[�V�����̈ړ��֘A�̃t�B�[���h
    private float tatsumakiMove = 25.0f; // ���������r�̈ړ�����
    private float syoryuMove = 10.0f; // �������̈ړ�����
    private float syoryuMoveY = 70f; // �������̐����ړ�����
    private bool tatsumakiMoveFlag = false; // ���������r�̈ړ��t���O
    private bool syoryuMoveFlag = false; // �������̈ړ��t���O
    private bool sakotsuwariMove = false; // ��������̈ړ��t���O

    private float hitMove = 15.0f; // �q�b�g���̈ړ�����
    private bool hitFlag = false; // �q�b�g�t���O

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerAction>(); // �v���C���[�̃A�N�V�������擾
    }

    private void Update()
    {
        // ���������r�܂��͍�������̈ړ�����
        if (tatsumakiMoveFlag || sakotsuwariMove)
        {
            float offset = player.anim.GetBool("Mirror") ? -tatsumakiMove : tatsumakiMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }
        // �������̈ړ�����
        else if (syoryuMoveFlag)
        {
            float offset = player.anim.GetBool("Mirror") ? -syoryuMove : syoryuMove;
            Vector3 moveDirection = new Vector3(offset, syoryuMoveY, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }

        // �L�[���͂ɂ��A�j���[�V�����̐ݒ�
        if (Input.GetKeyDown(KeyCode.C))
        {
            player.animState.SetAnimTrue("Huttobi");
        }

        // �q�b�g���̈ړ�����
        if (hitFlag)
        {
            gameObject.layer = LayerMask.NameToLayer("HitNow");
            float offset = player.anim.GetBool("Mirror") ? hitMove : -hitMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }
    }

    // �A�j���[�V�����C�x���g
    // �g��������
    void Hadoken()
    {
        if (!hadoFlag)
        {
            StartCoroutine(HadoFlag());
            Vector3 direction = player.anim.GetBool("Mirror") ? Vector3.left : Vector3.right;
            hadokenMove.Initialize(direction, HadoHitPlayer);
            Instantiate(hadoken, player.anim.GetBool("Mirror") ? hadokenPosL.transform.position : hadokenPosR.transform.position, Quaternion.identity);
        }
    }

    // �g�����𕡐����������Ȃ����߂̃t���O�Ǘ�
    IEnumerator HadoFlag()
    {
        hadoFlag = true;
        yield return new WaitForSeconds(0.5f);
        hadoFlag = false;
    }

    // �g�����A�j���[�V�����I��
    void HadoFinish()
    {
        player.animState.SetAnimFalse("Hadoken");
        // ���E���͊m�F
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // �g�����A�j���[�V�����I��
    void HasyogekiFinish()
    {
        player.animState.SetAnimFalse("Hasyogeki");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // �������̈ړ��J�n
    void SyoryuMove()
    {
        syoryuMoveFlag = true;
    }

    // �������̈ړ��I��
    void SyoryuMoveFalse()
    {
        syoryuMoveFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.MoveDirection = moveDirection;
    }

    // �������A�j���[�V�����I��
    void SyoryuFinish()
    {
        player.animState.SetAnimFalse("Syoryuken");
        // ���E���͊m�F
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // ���������r�̈ړ��J�n
    void TatsumakiMove()
    {
        tatsumakiMoveFlag = true;
    }

    // ���������r�̈ړ��I��
    void TatsumakiMoveFalse()
    {
        tatsumakiMoveFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.controller.Move(moveDirection);
    }

    // ���������r�A�j���[�V�����I��
    void TatsumakiFalse()
    {
        player.animState.SetAnimFalse("Tatsumaki");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // �U���A�j���[�V�����I��
    void NPFinish()
    {
        player.animState.SetAnimFalse("N_Weak");
        player.animState.SetAnimFalse("N_Mid");
        player.animState.SetAnimFalse("N_Stro");
        // ���E���͊m�F
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // �󒆍U���A�j���[�V�����I��
    void NAFinish()
    {
        player.animState.SetAnimFalse("NA_Mid");
        player.DetectPadHorizontalInput();
    }

    // ���Ⴊ�ݍU���A�j���[�V�����I��
    void CKFinish()
    {
        player.animState.SetAnimFalse("C_Weak");
        player.animState.SetAnimFalse("C_Mid");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // �������̈ړ�����
    void AshibaraiMove()
    {
        float offset = player.anim.GetBool("Mirror") ? -2f : 2f;
        Vector3 moveDirection = new Vector3(offset, 0, 0);
        player.controller.Move(moveDirection);
    }

    // �������A�j���[�V�����I��
    void AshibaraiFalse()
    {
        player.animState.SetAnimFalse("Ashibarai");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // ��������̈ړ��J�n
    void SakotsuwariMove()
    {
        sakotsuwariMove = true;
    }

    // ��������A�j���[�V�����I��
    void SakotsuwariFalse()
    {
        player.animState.SetAnimFalse("Sakotsuwari");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // �J�����̃C���p���X����
    void CameraImpulse()
    {
        cineCamera.GenerateImpulse();
    }

    // �O�X�e�b�v�I��
    void StepRF()
    {
        player.animState.SetAnimFalse("StepR");
        StartCoroutine(StepFalse());
    }

    // �O�X�e�b�v�ړ��I��
    void StepRMoveF()
    {
        Vector3 moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }

    // ���X�e�b�v�I��
    void StepLF()
    {
        player.animState.SetAnimFalse("StepL");
        StartCoroutine(StepFalse());
    }

    // ���X�e�b�v�ړ��I��
    void StepLMoveF()
    {
        Vector3 moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }

    // ���ׂĂ̕����̃W�����v�A�j���[�V�����J�n
    void JStart()
    {
        // �W�����v�J�n���W�����v��(�����A�O�A���)
        if (player.IsJumpVertical)
        {
            player.animState.SetJumpState((int)JumpState.State2);
        }
        else if (player.IsJumpFront)
        {
            player.animState.SetJumpState((int)JumpState.State3);
        }
        else if (player.IsJumpBack)
        {
            player.animState.SetJumpState((int)JumpState.State4);
        }
    }

    // �W�����v�A�j���[�V�����I��
    void JFinish()
    {
        player.animState.SetJumpState((int)JumpState.State0);
        player.animState.SetJump(false);
        player.IsJumpVertical = false;
        player.IsJumpFront = false;
        player.IsJumpBack = false;
        player.playerInput.JumpFlag = false;
        StartCoroutine(AttackFalse());
        player.DetectPadHorizontalInput();
    }

    // �W�����v�A�j���[�V�����̃��Z�b�g
    void JumpFF()
    {
        player.animState.ResetAnim();
    }

    // ���Ⴊ�݊J�n�A�j���[�V�����J�n
    void CStart()
    {
        player.animState.SetAnimTrue("Crouching");
    }

    // ���Ⴊ�݊J�n�A�j���[�V�����I��
    void CStartFalse()
    {
        player.animState.SetAnimFalse("CStart");
    }

    // �q�b�g���C���[�̐ݒ�
    void HitLayer()
    {
        hitFlag = true;
    }

    // �q�b�g�ړ��̏I��
    void HitMoveFalse()
    {
        hitFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.MoveDirection = moveDirection;
    }

    // �q�b�g�A�j���[�V�����̏I��
    void HitFalse()
    {
        player.animState.SetAnimFalse("SWeakHit");
        player.animState.SetAnimFalse("SMidHit");
        player.animState.SetAnimFalse("SStrongHit");
        player.animState.SetAnimFalse("Huttobi");
        gameObject.layer = LayerMask.NameToLayer(originalLayer);
        player.DetectPadHorizontalInput();
    }

    // �L�����Z���t���[���̐ݒ�
    void Cancel()
    {
        player.animState.SetAnimTrue("Cancel");
        StartCoroutine(AttackFalse());
        stepL = false;
        stepR = false;
    }

    // �L�����Z���t���[���̉���
    void CancelFalse()
    {
        player.animState.SetAnimFalse("Cancel");
    }

    // �E�X�e�b�v�̃v���p�e�B
    public bool StepR
    {
        get { return stepR; }
        set { stepR = value; }
    }

    // ���X�e�b�v�̃v���p�e�B
    public bool StepL
    {
        get { return stepL; }
        set { stepL = value; }
    }

    // �U���I���ɒx����ǉ�
    public IEnumerator AttackFalse()
    {
        yield return new WaitForSeconds(0.1f);
        player.playerInput.AttackFlag = false;
    }

    // �X�e�b�v�I���ɒx����ǉ�
    public IEnumerator StepFalse()
    {
        yield return new WaitForSeconds(0.1f);
        // ���E���͊m�F
        player.DetectPadHorizontalInput();
        stepL = false;
        stepR = false;
    }

    // �L�����Z�����Ăяo��
    public void CallCancel()
    {
        Cancel();
    }

}
