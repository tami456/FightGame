using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandBag : MonoBehaviour
{
    [SerializeField]
    private TutorialCollider tuto; // チュートリアル用のコライダー
    [SerializeField]
    private Animator anim; // アニメーターコンポーネント
    private AnimationState state; // アニメーションステート
    private bool isWeak = false; // 弱攻撃フラグ
    private bool isMiddle = false; // 中攻撃フラグ
    private bool isStrong = false; // 強攻撃フラグ
    private bool isJumpAttack = false; // ジャンプ攻撃フラグ
    private Animator animator; // アニメーター
    [SerializeField]
    private GameObject impactEffect; // 衝突エフェクト
    [SerializeField]
    private GameObject time; // 時間オブジェクト（用途不明）

    // 各攻撃フラグのゲッター
    public bool GetIsWeak() => isWeak;
    public bool GetIsMiddle() => isMiddle;
    public bool GetIsStrong() => isStrong;
    public bool GetIsJumpAttack() => isJumpAttack;

    // Awakeはスクリプトインスタンスがロードされたときに呼び出される
    void Awake()
    {
        state = GetComponent<AnimationState>();
        animator = GetComponent<Animator>();
        animator.SetBool("Mirror", true);
    }

    // トリガーに衝突したときに呼び出される
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            // 衝突した場所にエフェクトを生成
            Vector3 position = other.ClosestPoint(transform.position); // 衝突した場所の近くの点を取得
            Instantiate(impactEffect, position, Quaternion.identity);

            // 攻撃の種類に応じてフラグを設定
            isWeak = anim.GetBool("N_Weak");
            isMiddle = anim.GetBool("N_Mid");
            isStrong = anim.GetBool("N_Stro");
            isJumpAttack = anim.GetBool("JumpKick") || anim.GetBool("JumpKick2") || anim.GetBool("JumpPunch");
        }
    }

}
