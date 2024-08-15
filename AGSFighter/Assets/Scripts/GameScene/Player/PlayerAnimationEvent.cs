using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAnimationEvent : MonoBehaviour
{
    // ジャンプ用ステートの列挙型
    public enum JumpState
    {
        State0,
        State1,
        State2,
        State3,
        State4,
        State5,
    }

    private PlayerAction player; // プレイヤーのアクション

    [SerializeField]
    private CinemachineImpulseSource cineCamera; // カメラのインパルスソース

    private bool stepR, stepL = false; // ステップフラグ

    // 波動拳関連のフィールド
    [SerializeField]
    private GameObject hadoken; // 波動拳のゲームオブジェクト
    [SerializeField]
    private Hadoken hadokenMove; // 波動拳の動き
    private bool hadoFlag = false; // 波動拳のフラグ
    [SerializeField]
    private GameObject hadokenPosR; // 波動拳の右側の位置
    [SerializeField]
    private GameObject hadokenPosL; // 波動拳の左側の位置
    [SerializeField]
    private string HadoHitPlayer; // 波動拳が当たるプレイヤーのレイヤー
    [SerializeField]
    private string originalLayer; // 元のレイヤー

    // アニメーションの移動関連のフィールド
    private float tatsumakiMove = 25.0f; // 竜巻旋風脚の移動距離
    private float syoryuMove = 10.0f; // 昇龍拳の移動距離
    private float syoryuMoveY = 70f; // 昇龍拳の垂直移動距離
    private bool tatsumakiMoveFlag = false; // 竜巻旋風脚の移動フラグ
    private bool syoryuMoveFlag = false; // 昇龍拳の移動フラグ
    private bool sakotsuwariMove = false; // 鎖骨割りの移動フラグ

    private float hitMove = 15.0f; // ヒット時の移動距離
    private bool hitFlag = false; // ヒットフラグ

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerAction>(); // プレイヤーのアクションを取得
    }

    private void Update()
    {
        // 竜巻旋風脚または鎖骨割りの移動処理
        if (tatsumakiMoveFlag || sakotsuwariMove)
        {
            float offset = player.anim.GetBool("Mirror") ? -tatsumakiMove : tatsumakiMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }
        // 昇龍拳の移動処理
        else if (syoryuMoveFlag)
        {
            float offset = player.anim.GetBool("Mirror") ? -syoryuMove : syoryuMove;
            Vector3 moveDirection = new Vector3(offset, syoryuMoveY, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }

        // キー入力によるアニメーションの設定
        if (Input.GetKeyDown(KeyCode.C))
        {
            player.animState.SetAnimTrue("Huttobi");
        }

        // ヒット時の移動処理
        if (hitFlag)
        {
            gameObject.layer = LayerMask.NameToLayer("HitNow");
            float offset = player.anim.GetBool("Mirror") ? hitMove : -hitMove;
            Vector3 moveDirection = new Vector3(offset, 0, 0) * Time.deltaTime;
            player.controller.Move(moveDirection);
        }
    }

    // アニメーションイベント
    // 波動拳生成
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

    // 波動拳を複数個生成させないためのフラグ管理
    IEnumerator HadoFlag()
    {
        hadoFlag = true;
        yield return new WaitForSeconds(0.5f);
        hadoFlag = false;
    }

    // 波動拳アニメーション終了
    void HadoFinish()
    {
        player.animState.SetAnimFalse("Hadoken");
        // 左右入力確認
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // 波動拳アニメーション終了
    void HasyogekiFinish()
    {
        player.animState.SetAnimFalse("Hasyogeki");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // 昇龍拳の移動開始
    void SyoryuMove()
    {
        syoryuMoveFlag = true;
    }

    // 昇龍拳の移動終了
    void SyoryuMoveFalse()
    {
        syoryuMoveFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.MoveDirection = moveDirection;
    }

    // 昇龍拳アニメーション終了
    void SyoryuFinish()
    {
        player.animState.SetAnimFalse("Syoryuken");
        // 左右入力確認
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // 竜巻旋風脚の移動開始
    void TatsumakiMove()
    {
        tatsumakiMoveFlag = true;
    }

    // 竜巻旋風脚の移動終了
    void TatsumakiMoveFalse()
    {
        tatsumakiMoveFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.controller.Move(moveDirection);
    }

    // 竜巻旋風脚アニメーション終了
    void TatsumakiFalse()
    {
        player.animState.SetAnimFalse("Tatsumaki");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // 攻撃アニメーション終了
    void NPFinish()
    {
        player.animState.SetAnimFalse("N_Weak");
        player.animState.SetAnimFalse("N_Mid");
        player.animState.SetAnimFalse("N_Stro");
        // 左右入力確認
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // 空中攻撃アニメーション終了
    void NAFinish()
    {
        player.animState.SetAnimFalse("NA_Mid");
        player.DetectPadHorizontalInput();
    }

    // しゃがみ攻撃アニメーション終了
    void CKFinish()
    {
        player.animState.SetAnimFalse("C_Weak");
        player.animState.SetAnimFalse("C_Mid");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // 足払いの移動処理
    void AshibaraiMove()
    {
        float offset = player.anim.GetBool("Mirror") ? -2f : 2f;
        Vector3 moveDirection = new Vector3(offset, 0, 0);
        player.controller.Move(moveDirection);
    }

    // 足払いアニメーション終了
    void AshibaraiFalse()
    {
        player.animState.SetAnimFalse("Ashibarai");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // 鎖骨割りの移動開始
    void SakotsuwariMove()
    {
        sakotsuwariMove = true;
    }

    // 鎖骨割りアニメーション終了
    void SakotsuwariFalse()
    {
        player.animState.SetAnimFalse("Sakotsuwari");
        player.DetectPadHorizontalInput();
        StartCoroutine(AttackFalse());
    }

    // カメラのインパルス生成
    void CameraImpulse()
    {
        cineCamera.GenerateImpulse();
    }

    // 前ステップ終了
    void StepRF()
    {
        player.animState.SetAnimFalse("StepR");
        StartCoroutine(StepFalse());
    }

    // 前ステップ移動終了
    void StepRMoveF()
    {
        Vector3 moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }

    // 後ろステップ終了
    void StepLF()
    {
        player.animState.SetAnimFalse("StepL");
        StartCoroutine(StepFalse());
    }

    // 後ろステップ移動終了
    void StepLMoveF()
    {
        Vector3 moveDirection = Vector3.zero;
        player.MoveDirection = moveDirection;
    }

    // すべての方向のジャンプアニメーション開始
    void JStart()
    {
        // ジャンプ開始→ジャンプ中(垂直、前、後ろ)
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

    // ジャンプアニメーション終了
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

    // ジャンプアニメーションのリセット
    void JumpFF()
    {
        player.animState.ResetAnim();
    }

    // しゃがみ開始アニメーション開始
    void CStart()
    {
        player.animState.SetAnimTrue("Crouching");
    }

    // しゃがみ開始アニメーション終了
    void CStartFalse()
    {
        player.animState.SetAnimFalse("CStart");
    }

    // ヒットレイヤーの設定
    void HitLayer()
    {
        hitFlag = true;
    }

    // ヒット移動の終了
    void HitMoveFalse()
    {
        hitFlag = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        player.MoveDirection = moveDirection;
    }

    // ヒットアニメーションの終了
    void HitFalse()
    {
        player.animState.SetAnimFalse("SWeakHit");
        player.animState.SetAnimFalse("SMidHit");
        player.animState.SetAnimFalse("SStrongHit");
        player.animState.SetAnimFalse("Huttobi");
        gameObject.layer = LayerMask.NameToLayer(originalLayer);
        player.DetectPadHorizontalInput();
    }

    // キャンセルフレームの設定
    void Cancel()
    {
        player.animState.SetAnimTrue("Cancel");
        StartCoroutine(AttackFalse());
        stepL = false;
        stepR = false;
    }

    // キャンセルフレームの解除
    void CancelFalse()
    {
        player.animState.SetAnimFalse("Cancel");
    }

    // 右ステップのプロパティ
    public bool StepR
    {
        get { return stepR; }
        set { stepR = value; }
    }

    // 左ステップのプロパティ
    public bool StepL
    {
        get { return stepL; }
        set { stepL = value; }
    }

    // 攻撃終了に遅延を追加
    public IEnumerator AttackFalse()
    {
        yield return new WaitForSeconds(0.1f);
        player.playerInput.AttackFlag = false;
    }

    // ステップ終了に遅延を追加
    public IEnumerator StepFalse()
    {
        yield return new WaitForSeconds(0.1f);
        // 左右入力確認
        player.DetectPadHorizontalInput();
        stepL = false;
        stepR = false;
    }

    // キャンセルを呼び出す
    public void CallCancel()
    {
        Cancel();
    }

}
