using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    //�񋓌^
    public enum MyState
    {
        Freeze,  //�Q�[���J�n�ҋ@
        Game,    //�Q�[����
    }

    public MyState state;

    //�C���X�y�N�^�ɕ\������ϐ�
    [SerializeField]
    private Vector3 respawn;  //�`���[�g���A���p�ϐ� - ���̈ʒu�ɖ߂�
    [SerializeField]
    private LayerMask layerMask = default;  //�ڒn���� - Ray�̔���ɗp����Layer
    [SerializeField]
    private string originalLayer;   //�Q�[���J�n���̃��C���[
    [SerializeField]
    Vector3 boxHalf;  //�ڒn����̑傫��
    [SerializeField]
    private float gravity = 30.0f;  //�d��
    [SerializeField]
    private float speed = 0.0f;  //�ړ����x
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;  //�ړ���
    [SerializeField]    
    private string hitHadoTag;  //�g�������ǂ̃^�O�ɓ��Ă邩
    [SerializeField]
    private bool mirrorFlag = false;    //�A�j���[�V�������]�p�ϐ�

    //�C���X�y�N�^�ɕ\�����Ȃ�
    [NonSerialized]
    public CharacterController controller;
    [NonSerialized]
    public Animator anim;
    [NonSerialized]
    public AnimationState animState;
    [NonSerialized]
    public PlayerInputSystem playerInput;
    [NonSerialized]
    public PlayerAnimationEvent playerAnimEvent;
    [NonSerialized]
    public PlayerHit playerHit;

    //�v���C�x�[�g�t�B�[���h
    private ColAnimationEvent colAnimEvent;
    private PlayerCheckInversion checkInversion;
    private bool isJumpFront = false;  //�W�����vbool(�ǂ̕����ɔ��ł��邩)
    private bool isJumpVer = false;  //�W�����vbool(�ǂ̕����ɔ��ł��邩)
    private bool isJumpBack = false;  //�W�����vbool(�ǂ̕����ɔ��ł��邩)
    private bool speedMirror = false;  //�ړ��ʗp�~���[
    private bool isGrounded = false;  //inspector�Ŋm�F����p
    private bool isCrouching = false;  //inspector�Ŋm�F����p

    //�p�u���b�N�v���p�e�B
    public MyState State
    {
        get { return state; }
        set { state = value; }
    }

    public Vector3 MoveDirection
    {
        get { return moveDirection; }
        set { moveDirection = value; }
    }

    public bool IsJumpBack
    {
        get { return isJumpBack; }
        set { isJumpBack = value; }
    }

    public bool IsJumpFront
    {
        get { return isJumpFront; }
        set { isJumpFront = value; }
    }

    public bool IsJumpVertical
    {
        get { return isJumpVer; }
        set { isJumpVer = value; }
    }

    public bool IsCrouching
    {
        get { return isCrouching; }
        set { isCrouching = value; }
    }
    public bool GetIsGround() => isGrounded;

    public float GetSpeed() => speed;

    public string GetHitTag() => hitHadoTag;

    void Start()
    {
        if (!InitializeComponents())
        {
            return;
        }
    }

    private void Update()
    {
        //0�L�[�������Ǝ��ԁEHP�E���E���h�擾���ȊO�������������
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            anim.SetBool("Mirror",mirrorFlag);
            playerHit.ResetCamera(false);
            animState.ResetAnim();
            animState.ResetDamageAnim();
            playerInput.JumpFlag = false;
            playerInput.AttackFlag = false;
            playerInput.guardCFlag = false;
            playerInput.guardCPFlag = false;
            playerInput.guardSFlag = false;
            playerInput.guardSPFlag = false;
            SetDefaultPos();
            StartCoroutine(playerHit.Cam());
        }

        //���]�������ɑO��̈ړ��ʂ����]������
        speedMirror = anim.GetBool("Mirror");

        if (!isGrounded) return;

        //�n�㎞�̏���
        HandleGroundedState();
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGrounded(); 
        
        //�W�����v���ɑ���ɏ��Ȃ��悤�Ƀ��C���[��ύX����
        if (anim.GetBool("JumpSF"))
        {
            gameObject.layer = LayerMask.NameToLayer("JumpPlayer");
        }
        if (isGrounded)
        {
            gameObject.layer = LayerMask.NameToLayer(originalLayer);
        }

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        //Game��Ԃł��Ⴊ��ł��Ȃ������瓮����
        if (state == MyState.Game && !isCrouching)
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    //���n����̕\��
    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(boxHalf.x, boxHalf.y, boxHalf.z));
    }

    //�R���|�[�l���g�̏�����
    private bool InitializeComponents()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController component is missing");
            return false;
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing");
            return false;
        }

        animState = GetComponent<AnimationState>();
        if (animState == null)
        {
            Debug.LogError("AnimationState component is missing");
            return false;
        }

        checkInversion = GetComponent<PlayerCheckInversion>();
        if (checkInversion == null)
        {
            Debug.LogError("PlayerCheckInversion component is missing");
            return false;
        }

        colAnimEvent = GetComponent<ColAnimationEvent>();
        if (colAnimEvent == null)
        {
            Debug.LogError("ColAnimationEvent component is missing");
            return false;
        }

        playerInput = GetComponent<PlayerInputSystem>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInputSystem component is missing");
            return false;
        }

        playerAnimEvent = GetComponent<PlayerAnimationEvent>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerAnimationEvent component is missing");
            return false;
        }

        playerHit = GetComponent<PlayerHit>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerHit component is missing");
            return false;
        }

        return true;
    }

    //�n��ɂ��鎞
    private void HandleGroundedState()
    {
        //�W�����v��A���n������
        if (playerInput.IsJump)
        {
            StopJumpAnimation();
            DetectPadHorizontalInput();
        }

        //���]����
        if (!checkInversion.GetIsInversion())
        {
            ToggleMirror();
        }
    }

    //���n���ɃW�����v�n�̃A�j���[�V�����E�ϐ���false�ɂ���
    private void StopJumpAnimation()
    {
        animState.SetAnimFalse("JumpKick");
        animState.SetAnimFalse("JumpKick2");
        animState.SetAnimFalse("JumpPunch");
        animState.SetJumpState(5); // Consider replacing '5' with a named constant
        playerInput.IsJump = false;
        colAnimEvent.Finish();
    }

    //�A�j���[�V�����I�����ɃX�e�B�b�N��|���Ă���ꍇ���삷��
    public void DetectPadHorizontalInput()
    {
        if (!anim.GetBool("Cancel")) return;

        if (playerInput.RV() == 1)
        {
            HandleRightInput();
        }
        else if (playerInput.LV() == 1)
        {
            HandleLeftInput();
        }
        else if (playerInput.DV() == 1)
        {
            animState.SetAnimTrue("CStart");
        }
        else if(playerInput.UV() == 1)
        {
            HandleJumpInput();
        }
        else
        {
            HandleNoInput();
        }
    }

    //�E���͎�
    private void HandleRightInput()
    {
        moveDirection.x = speedMirror ? 5.1f : 8f;
        string animToStart = anim.GetBool("Mirror") ? "BackMove" : "FrontMove";
        animState.SetAnimTrue(animToStart);
    }

    //�����͎�
    private void HandleLeftInput()
    {
        moveDirection.x = speedMirror ? -8f : -5.1f;
        string animToStart = anim.GetBool("Mirror") ? "FrontMove" : "BackMove";
        animState.SetAnimTrue(animToStart);
    }

    //����͎�
    private void HandleJumpInput()
    {
        if (playerInput.CanJump())
        {
            playerInput.JumpFlag = true;
            StartCoroutine(playerInput.Count());
            IsJumpVertical = true;
            moveDirection.x = 0.0f;
            moveDirection.y = 50f;
            animState.SetJump(true);
        }
    }

    //�����͎�
    private void HandleNoInput()
    {
        moveDirection.x = 0.0f;
        animState.SetAnimFalse("FrontMove");
        animState.SetAnimFalse("BackMove");
    }

    //�A�j���[�V�����~���[
    private void ToggleMirror()
    {
        bool currentMirror = anim.GetBool("Mirror");
        anim.SetBool("Mirror", !currentMirror);
    }

    //�|�W�V�������Z�b�g
    public void SetDefaultPos()
    {
        controller.enabled = false;
        transform.position = new Vector3(respawn.x, respawn.y, respawn.z);
        controller.enabled = true;
    }

    //�ڒn����
    private bool CheckGrounded()
    {
        return Physics.CheckBox(transform.position,
            new Vector3(boxHalf.x, boxHalf.y, boxHalf.z), Quaternion.identity,
            layerMask, QueryTriggerInteraction.Ignore);
    }
}
