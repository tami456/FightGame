using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCollider : MonoBehaviour
{
    // チュートリアルのステップを定義する列挙型
    private enum Tutorial
    {
        MoveToTarget,
        Step,
        Jump,
        N_Weak,
        N_Middle,
        N_Strong,
        Crouching,
        JumpAttack,
        TutorialFinish,
        None
    }

    [SerializeField]
    private SandBag sandbag; // サンドバッグの参照
    private Tutorial tutorial; // 現在のチュートリアルステップ

    [SerializeField]
    private PlayerAction player; // プレイヤー
    [SerializeField]
    private PlayerAction tuto; // チュートリアル用の敵

    [SerializeField]
    private List<GameObject> targetObj = new List<GameObject>(); // 目標オブジェクトのリスト
    [SerializeField]
    private List<GameObject> targetText = new List<GameObject>(); // 目標テキストのリスト
    [SerializeField]
    private List<Transform> targetsPos = new List<Transform>(); // 目標位置のリスト
    private int currentIndex = 0; // 現在の目標インデックス

    [SerializeField]
    private Animator anim; // アニメーター
    private bool isJump = false; // ジャンプ状態のフラグ

    private void Start()
    {
        tutorial = Tutorial.MoveToTarget; // 初期ステップを設定
    }

    private void Update()
    {
        // 現在のチュートリアルステップに応じて処理を実行
        switch (tutorial)
        {
            case Tutorial.MoveToTarget:
                MoveToTarget();
                break;
            case Tutorial.Step:
                Step();
                break;
            case Tutorial.Jump:
                Jump();
                break;
            case Tutorial.N_Weak:
                N_Weak();
                break;
            case Tutorial.N_Middle:
                N_Middle();
                break;
            case Tutorial.N_Strong:
                N_Strong();
                break;
            case Tutorial.Crouching:
                Crouching();
                break;
            case Tutorial.JumpAttack:
                JumpAttack();
                break;
            case Tutorial.TutorialFinish:
                TutorialFinish();
                break;
        }
    }

    // 所定の位置まで歩く
    private void MoveToTarget()
    {
        // キャラクターが目標地点に到達したかを確認する
        if (Mathf.Abs(player.controller.transform.position.x - targetsPos[currentIndex].position.x) < 2f)
        {
            // ステップが完了した場合の処理
            Debug.Log("Step 1 completed!");
            UpdateTutorialStep();
            // 次のステップに移行する
            tutorial = Tutorial.Step;
        }
    }

    // 相手に向かってステップ
    private void Step()
    {
        bool stepR = anim.GetBool("StepR");
        if (stepR)
        {
            // ステップが完了した場合の処理
            Debug.Log("Step 2 completed!");
            UpdateTutorialStep();
            // 次のステップに移行
            tutorial = Tutorial.Jump;
        }
    }

    // ジャンプ
    private void Jump()
    {
        if (Mathf.Abs(player.controller.transform.position.y - targetsPos[currentIndex].position.y) < 0.1f)
        {
            isJump = true;
        }

        if (!isJump)
        {
            return;
        }

        if (player.GetIsGround())
        {
            // ステップが完了した場合の処理
            Debug.Log("Step 3 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.N_Weak;
        }
    }

    // 相手に対して弱攻撃
    private void N_Weak()
    {
        if (sandbag.GetIsWeak())
        {
            // ステップが完了した場合の処理
            Debug.Log("Step 4 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.N_Middle;
        }
    }

    // 相手に対して中攻撃
    private void N_Middle()
    {
        if (sandbag.GetIsMiddle())
        {
            // ステップが完了した場合の処理
            Debug.Log("Step 5 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.N_Strong;
        }
    }

    // 相手に対して強攻撃
    private void N_Strong()
    {
        if (sandbag.GetIsStrong())
        {
            // ステップが完了した場合の処理
            Debug.Log("Step 6 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.Crouching;
        }
    }

    // しゃがみ
    private void Crouching()
    {
        bool isCrouching = anim.GetBool("Crouching");
        if (isCrouching)
        {
            // ステップが完了した場合の処理
            Debug.Log("Step 7 completed!");
            UpdateTutorialStep();
            tutorial = Tutorial.JumpAttack;
        }
    }

    // 相手に対してジャンプ攻撃
    private void JumpAttack()
    {
        if (sandbag.GetIsJumpAttack())
        {
            // ステップが完了した場合の処理
            Debug.Log("Step 8 completed!");
            UpdateTutorialStep();
            tuto.SetDefaultPos();
            tutorial = Tutorial.TutorialFinish;
        }
    }

    // チュートリアル終了処理
    private void TutorialFinish()
    {
        // チュートリアル開始の処理
        UpdateTutorialStep();
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("SelectModeScene");
        }
    }

    // チュートリアルをクリアしたときの処理
    private void UpdateTutorialStep()
    {
        StartCoroutine(NextTutorial());
        player.animState.SetAnimFalse("Crouching");
        // チュートリアルをクリアしたら、次のチュートリアルが始まるまで動けないようにする
        player.State = PlayerAction.MyState.Freeze;
        // チュートリアル開始の処理
        StartCoroutine(WarpAndToggleObjects());
    }

    // プレイヤーをデフォルトの位置にワープし、目標オブジェクトを切り替える
    IEnumerator WarpAndToggleObjects()
    {
        yield return new WaitForSeconds(1.0f);
        // プレイヤーをデフォルトの位置にワープ
        player.SetDefaultPos();
        player.animState.ResetAnim();
        player.State = PlayerAction.MyState.Game;
        // 今の目標オブジェクトをfalseにして、次の目標オブジェクトをtrueにする
        targetObj[currentIndex].SetActive(false);
        targetObj[currentIndex + 1].SetActive(true);
        targetText[currentIndex].SetActive(false);
        targetText[currentIndex + 1].SetActive(true);
        currentIndex += 1;
    }

    // 次のチュートリアルステップに進む
    private IEnumerator NextTutorial()
    {
        yield return FadeController.Instance.StartCoroutine(FadeController.Instance.FadeOut());
        FadeController.Instance.StartCoroutine(FadeController.Instance.FadeIn());
    }
}
