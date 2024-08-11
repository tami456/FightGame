using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAnimationEvent : MonoBehaviour
{
    //�W�����v�p�X�e�[�g
    public enum JumpState
    {
        State0,
        State1,
        State2,
        State3,
        State4,
        State5,
    }

    private PlayerAction player;
    [SerializeField]
    private GameObject playerPos;

    [SerializeField]
    private CinemachineImpulseSource cineCamera;

    bool stepR, stepL = false;

    //�g����
    [SerializeField]
    GameObject hadoken;
    [SerializeField]
    Hadoken hadokenMove;
    bool hadoFlag = false;
    [SerializeField]
    private GameObject hadokenPosR;
    [SerializeField]
    private GameObject hadokenPosL;
    [SerializeField]
    private string HadoHitPlayer;
    [SerializeField]
    private string originalLayer;

    //�A�j���[�V�����̈ړ�
    private float tatsumakiMove = 25.0f;
    private float syoryuMove = 10.0f;
    private float syoryuMoveY = 50f;
    private bool tatsumakiMoveFlag = false;
    private bool syoryuMoveFlag = false;
    private bool sakotsuwariMove = false;

    private float hitMove = 15.0f;
    private bool hitFlag = false;

    //�d������
    private bool rigorFlag = false;
    private float rigorTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerAction>();
    }

    private void Update()
    {
        if (tatsumakiMoveFlag || sakotsuwariMove)
        {
            float offset = player.anim.GetBool("Mirror") ? -tatsumakiMove : tatsumakiMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }
        else if (syoryuMoveFlag)
        {
            float offset = player.anim.GetBool("Mirror") ? -syoryuMove : syoryuMove;
            Vector3 moveDirection = new Vector3(offset, syoryuMoveY, 0) * Time.deltaTime;
            player.MoveDirection = moveDirection;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            player.animState.SetAnimTrue("Huttobi");
        }

        if (hitFlag)
        {
            gameObject.layer = LayerMask.NameToLayer("HitNow");
            float offset = player.anim.GetBool("Mirror") ? hitMove : -hitMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }

        //�d������
        if (!rigorFlag)
        {
            rigorTime = 0.0f;
        }
    }

    //�A�j���[�V�����C�x���g
    //�g��������

    void Hadoken()
    {
        if(!hadoFlag)
        {
            StartCoroutine(HadoFlag());
            Vector3 direction = player.anim.GetBool("Mirror") ? Vector3.left : Vector3.right;
            hadokenMove.Initialize(direction, HadoHitPlayer);
            Instantiate(hadoken, player.anim.GetBool("Mirror") ? hadokenPosL.transform.position : hadokenPosR.transform.position, Quaternion.identity);
        }
    }

    //�g�����𕡐����������Ȃ�����
    IEnumerator HadoFlag()
    {
        hadoFlag = true;
        yield return new WaitForSeconds(0.5f);
        hadoFlag = false;
    }

    //�g�����A�j���[�V�����I��
    void HadoFinish()
    {
        player.animState.SetAnimFalse("Hadoken");
        //���E���͊m�F
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void HasyogekiFinish()
    {
        player.animState.SetAnimFalse("Hasyogeki");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void SyoryuMove()
    {
        syoryuMoveFlag = true;
    }

    void SyoryuMoveFalse()
    {
        syoryuMoveFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.MoveDirection = moveDirection;
    }

    void SyoryuFinish()
    {
        player.animState.SetAnimFalse("Syoryuken");
        //���E���͊m�F
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void TatsumakiMove()
    {
        tatsumakiMoveFlag = true;
    }

    void TatsumakiMoveFalse()
    {
        tatsumakiMoveFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.controller.Move(moveDirection);
    }

    void TatsumakiFalse()
    {
        player.animState.SetAnimFalse("Tatsumaki");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    //�U���A�j���[�V�����I��
    void NPFinish()
    {
        player.animState.SetAnimFalse("N_Weak");
        player.animState.SetAnimFalse("N_Mid");
        player.animState.SetAnimFalse("N_Stro");
        //���E���͊m�F
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void NAFinish()
    {
        player.animState.SetAnimFalse("NA_Mid");
        player.DetectPadHorizontalInput();
    }

    void CKFinish()
    {
        player.animState.SetAnimFalse("C_Weak");
        player.animState.SetAnimFalse("C_Mid");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    //���ݍ��ޓ��삪���邽�߁A�����O�֓���
    void AshibaraiMove()
    {
        float offset = player.anim.GetBool("Mirror") ? -2f : 2f;
        Vector3 moveDirection = new Vector3(offset, 0, 0);
        player.controller.Move(moveDirection);
    }

    //�������I��
    void AshibaraiFalse()
    {
        player.animState.SetAnimFalse("Ashibarai");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    //��������ړ�
    void SakotsuwariMove()
    {
        sakotsuwariMove = true;
    }

    //��������I��
    void SakotsuwariFalse()
    {
        player.animState.SetAnimFalse("Sakotsuwari");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    void CameraImpulse()
    {
        cineCamera.GenerateImpulse();
    }

    //�O�X�e�b�v�I��
    void StepRF()
    {
        player.animState.SetAnimFalse("StepR");
        StartCoroutine(StepFalse());
    }

    //�O�X�e�b�v�ړ��I��
    void StepRMoveF()
    {
        Vector3 moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }

    //���X�e�b�v�I��
    void StepLF()
    {
        player.animState.SetAnimFalse("StepL");
        StartCoroutine(StepFalse());
    }

    //���X�e�b�v�ړ��I��
    void StepLMoveF()
    {
        Vector3 moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }


    //���ׂĂ̕����̃W�����v�A�j���[�V�����J�n
    void JStart()
    {
        //�W�����v�J�n���W�����v��(�����A�O�A���)
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

    //�W�����v�A�j���[�V�����I��
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

    void JumpFF()
    {
        player.animState.ResetAnim();
    }

    //���Ⴊ�݊J�n�A�j���[�V�����J�n
    void CStart()
    {
        player.animState.SetAnimTrue("Crouching");
    }

    //���Ⴊ�݊J�n�A�j���[�V�����I��
    void CStartFalse()
    {
        player.animState.SetAnimFalse("CStart");
    }

    void HitRigorTime()
    {
        player.animState.SetAnimTrue("Cancel");
        rigorFlag = true;
    }

    void HitLayer()
    {
        hitFlag = true;
    }

    void HitMoveFalse()
    {
        hitFlag = false;
    }

    void HitFalse()
    {
        player.animState.SetAnimFalse("SWeakHit");
        player.animState.SetAnimFalse("SMidHit");
        player.animState.SetAnimFalse("SStrongHit");
        player.animState.SetAnimFalse("Huttobi");
        gameObject.layer = LayerMask.NameToLayer(originalLayer);
        player.DetectPadHorizontalInput();
    }

    //�L�����Z���t���[��
    void Cancel()
    {
        player.animState.SetAnimTrue("Cancel");
        StartCoroutine(AttackFalse());
        stepL = false;
        stepR = false;
    }

    void CancelFalse()
    {
        player.animState.SetAnimFalse("Cancel");
    }

    public bool StepR
    {
        get { return stepR; }
        set { stepR = value; }
    }

    public bool StepL
    {
        get { return stepL; }
        set { stepL = value; }
    }

    //�U���I���ɒx��
    public IEnumerator AttackFalse()
    {
        yield return 0.1f;
        player.playerInput.AttackFlag = false;
    }

    public IEnumerator StepFalse()
    {
        yield return 0.1f;
        //���E���͊m�F
        player.DetectPadHorizontalInput();
        stepL = false;
        stepR = false;
    }

    public void CallCancel()
    {
        Cancel();
    }
}
