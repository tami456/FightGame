using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    //列挙型
    public enum MyState
    {
        Freeze,  //ゲーム開始待機
        Game,    //ゲーム中
    }

    public MyState state;

    //インスペクタに表示する変数
    [SerializeField]
    private Vector3 respawn;  //チュートリアル用変数 - 元の位置に戻る
    [SerializeField]
    private LayerMask layerMask = default;  //接地判定 - Rayの判定に用いるLayer
    [SerializeField]
    private string originalLayer;   //ゲーム開始時のレイヤー
    [SerializeField]
    Vector3 boxHalf;  //接地判定の大きさ
    [SerializeField]
    private float gravity = 30.0f;  //重力
    [SerializeField]
    private float speed = 0.0f;  //移動速度
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;  //移動量
    [SerializeField]    
    private string hitHadoTag;  //波動拳をどのタグに当てるか
    [SerializeField]
    private bool mirrorFlag = false;    //アニメーション反転用変数

    //インスペクタに表示しない
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

    //プライベートフィールド
    private ColAnimationEvent colAnimEvent;
    private PlayerCheckInversion checkInversion;
    private bool isJumpFront = false;  //ジャンプbool(どの方向に飛んでいるか)
    private bool isJumpVer = false;  //ジャンプbool(どの方向に飛んでいるか)
    private bool isJumpBack = false;  //ジャンプbool(どの方向に飛んでいるか)
    private bool speedMirror = false;  //移動量用ミラー
    private bool isGrounded = false;  //inspectorで確認する用
    private bool isCrouching = false;  //inspectorで確認する用

    //パブリックプロパティ
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
        //0キーを押すと時間・HP・ラウンド取得数以外が初期化される
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

        //反転した時に前後の移動量も反転させる
        speedMirror = anim.GetBool("Mirror");

        if (!isGrounded) return;

        //地上時の処理
        HandleGroundedState();
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGrounded(); 
        
        //ジャンプ時に相手に乗らないようにレイヤーを変更する
        if (anim.GetBool("JumpSF"))
        {
            gameObject.layer = LayerMask.NameToLayer("JumpPlayer");
        }
        if (isGrounded)
        {
            gameObject.layer = LayerMask.NameToLayer(originalLayer);
        }

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        //Game状態でしゃがんでいなかったら動ける
        if (state == MyState.Game && !isCrouching)
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    //着地判定の表示
    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(boxHalf.x, boxHalf.y, boxHalf.z));
    }

    //コンポーネントの初期化
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

    //地上にいる時
    private void HandleGroundedState()
    {
        //ジャンプ後、着地した時
        if (playerInput.IsJump)
        {
            StopJumpAnimation();
            DetectPadHorizontalInput();
        }

        //反転処理
        if (!checkInversion.GetIsInversion())
        {
            ToggleMirror();
        }
    }

    //着地時にジャンプ系のアニメーション・変数をfalseにする
    private void StopJumpAnimation()
    {
        animState.SetAnimFalse("JumpKick");
        animState.SetAnimFalse("JumpKick2");
        animState.SetAnimFalse("JumpPunch");
        animState.SetJumpState(5); // Consider replacing '5' with a named constant
        playerInput.IsJump = false;
        colAnimEvent.Finish();
    }

    //アニメーション終了時にスティックを倒している場合動作する
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

    //右入力時
    private void HandleRightInput()
    {
        moveDirection.x = speedMirror ? 5.1f : 8f;
        string animToStart = anim.GetBool("Mirror") ? "BackMove" : "FrontMove";
        animState.SetAnimTrue(animToStart);
    }

    //左入力時
    private void HandleLeftInput()
    {
        moveDirection.x = speedMirror ? -8f : -5.1f;
        string animToStart = anim.GetBool("Mirror") ? "FrontMove" : "BackMove";
        animState.SetAnimTrue(animToStart);
    }

    //上入力時
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

    //無入力時
    private void HandleNoInput()
    {
        moveDirection.x = 0.0f;
        animState.SetAnimFalse("FrontMove");
        animState.SetAnimFalse("BackMove");
    }

    //アニメーションミラー
    private void ToggleMirror()
    {
        bool currentMirror = anim.GetBool("Mirror");
        anim.SetBool("Mirror", !currentMirror);
    }

    //ポジションリセット
    public void SetDefaultPos()
    {
        controller.enabled = false;
        transform.position = new Vector3(respawn.x, respawn.y, respawn.z);
        controller.enabled = true;
    }

    //接地判定
    private bool CheckGrounded()
    {
        return Physics.CheckBox(transform.position,
            new Vector3(boxHalf.x, boxHalf.y, boxHalf.z), Quaternion.identity,
            layerMask, QueryTriggerInteraction.Ignore);
    }
}
