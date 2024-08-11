using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ColAnimationEvent : MonoBehaviour
{
    //攻撃終了イベント
    public event Action<bool> OnAttackEnded;

    //当たり判定
    //右手、左手
    [SerializeField]
    private CapsuleCollider rightArmCapsuleCol, leftArmCapsuleCol;

    //太もも、ふくらはぎ
    [SerializeField]
    private CapsuleCollider rightLegUpCapsuleCol, rightLegCapsuleCol;
    [SerializeField]
    private CapsuleCollider leftLegUpCapsuleCol, leftLegCapsuleCol;

    //お尻
    [SerializeField]
    private BoxCollider rightHipCapsuleCol, leftHipCapsuleCol;

    //波衝撃
    [SerializeField]
    private CapsuleCollider rightHasyoCol, leftHasyoCol;

    //昇竜拳
    [SerializeField]
    private CapsuleCollider rightSyoryuCol, leftSyoryuCol;

    private Dictionary<string, Collider> colliders;

    [SerializeField]
    private bool mirror;

    [NonSerialized]
    public Animator anim;

    private void Start()
    {
        // アニメーターコンポーネントの取得とヌルチェック
        if (!TryGetComponent(out anim))
        {
            Debug.LogError("Animator コンポーネントが見つかりません。");
            return;
        }

        // コライダーを辞書に登録
        colliders = new Dictionary<string, Collider>()
        {
            { nameof(rightArmCapsuleCol), rightArmCapsuleCol },
            { nameof(leftArmCapsuleCol), leftArmCapsuleCol },
            { nameof(rightLegUpCapsuleCol), rightLegUpCapsuleCol },
            { nameof(rightLegCapsuleCol), rightLegCapsuleCol },
            { nameof(leftLegUpCapsuleCol), leftLegUpCapsuleCol },
            { nameof(leftLegCapsuleCol), leftLegCapsuleCol },
            { nameof(rightHipCapsuleCol), rightHipCapsuleCol },
            { nameof(leftHipCapsuleCol), leftHipCapsuleCol },
            { nameof(rightHasyoCol), rightHasyoCol },
            { nameof(leftHasyoCol), leftHasyoCol },
            { nameof(rightSyoryuCol), rightSyoryuCol },
            { nameof(leftSyoryuCol), leftSyoryuCol },
        };

        // コライダーが設定されているかチェック
        foreach (var collider in colliders.Values)
        {
            if (collider == null)
            {
                Debug.LogError("カプセルコライダーが設定されていない箇所があります。");
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        // ミラーリングフラグの更新
        if (anim != null)
        {
            mirror = anim.GetBool("Mirror");
        }
    }

    /// <summary>
    /// コライダーの有効/無効を設定
    /// </summary>
    /// <param name="colliderName">コライダーの名前</param>
    /// <param name="enable">有効/無効フラグ</param>
    private void EnableCollider(string colliderName, bool enable)
    {
        if (colliders.TryGetValue(colliderName, out Collider collider))
        {
            collider.enabled = enable;
        }
        else
        {
            Debug.LogError($"'{colliderName}' のコライダーが見つかりません。");
        }
    }

    /// <summary>
    /// 攻撃処理をハンドル
    /// </summary>
    /// <param name="colliderName">コライダーの名前</param>
    private void HandleAttack(string colliderName)
    {
        EnableCollider(mirror ? GetMirrorColliderName(colliderName) : colliderName, true);
    }

    /// <summary>
    /// ミラーリング時のコライダー名を取得
    /// </summary>
    /// <param name="colliderName">元のコライダー名</param>
    /// <returns>ミラーリング時のコライダー名</returns>
    private string GetMirrorColliderName(string colliderName)
    {
        return colliderName switch
        {
            nameof(rightArmCapsuleCol) => nameof(leftArmCapsuleCol),
            nameof(leftArmCapsuleCol) => nameof(rightArmCapsuleCol),
            nameof(rightLegUpCapsuleCol) => nameof(leftLegUpCapsuleCol),
            nameof(leftLegUpCapsuleCol) => nameof(rightLegUpCapsuleCol),
            nameof(rightLegCapsuleCol) => nameof(leftLegCapsuleCol),
            nameof(leftLegCapsuleCol) => nameof(rightLegCapsuleCol),
            nameof(rightHipCapsuleCol) => nameof(leftHipCapsuleCol),
            nameof(leftHipCapsuleCol) => nameof(rightHipCapsuleCol),
            nameof(rightHasyoCol) => nameof(leftHasyoCol),
            nameof(leftHasyoCol) => nameof(rightHasyoCol),
            nameof(rightSyoryuCol) => nameof(leftSyoryuCol),
            nameof(leftSyoryuCol) => nameof(rightSyoryuCol),
            _ => colliderName
        };
    }

    // 各攻撃メソッド
    void RightArmAttack() => HandleAttack(nameof(rightArmCapsuleCol));
    void LeftArmAttack() => HandleAttack(nameof(leftArmCapsuleCol));
    void RightLegUpAttack() => HandleAttack(nameof(rightLegUpCapsuleCol));
    void RightLegAttack() => HandleAttack(nameof(rightLegCapsuleCol));
    void LeftLegUpAttack() => HandleAttack(nameof(leftLegUpCapsuleCol));
    void LeftLegAttack() => HandleAttack(nameof(leftLegCapsuleCol));
    void LeftHipAttack() => HandleAttack(nameof(leftHipCapsuleCol));
    void Hasyogeki() => HandleAttack(nameof(rightHasyoCol));
    void SyoryuAttack() => HandleAttack(nameof(rightSyoryuCol));

    /// <summary>
    /// 攻撃終了処理
    /// </summary>
    void AttackFinish()
    {
        foreach (var collider in colliders.Values)
        {
            collider.enabled = false;
        }
        OnAttackEnded?.Invoke(false);
    }

    /// <summary>
    /// 攻撃終了を外部から呼び出す
    /// </summary>
    public void Finish()
    {
        AttackFinish();
    }
}