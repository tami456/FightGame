using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGuard : MonoBehaviour
{
    [SerializeField]
    private ColAnimationEvent enemyColAnim; // 敵の攻撃アニメーションイベント
    [SerializeField]
    private ColAnimationEvent colAnimationEvent; // 自分の攻撃アニメーションイベント

    private bool isGuard = false; // ガード状態を示すフラグ
    private float guard = 0.0f; // ガードの持続時間

    [SerializeField]
    private GameObject guardEffect; // ガードエフェクトのプレハブ
    private bool canInstantiateEffect = true; // エフェクト生成の制御フラグ

    private Collider col; // 衝突したコライダー

    private void Update()
    {
        //フレームレートに依存しないようにTime.deltaTimeを使用してガード時間を増加
        if (isGuard)
        {
            guard += Time.deltaTime; 
        }
        //ガード時間が一定を超えたらリセット
        if (guard >= 50.0f)
        {
            ResetGuard(); 
        }

        //ソロモードではガードは使わない
        if (SceneManager.GetActiveScene().name != "SoloGameScene")
        {
            enemyColAnim.OnAttackEnded += HandleAttackEnded; // 攻撃終了イベントのハンドラを設定
            enemyColAnim.OnAttackEnded += get_flag =>
            {
                if (get_flag)
                {

                }
                else
                {
                    isGuard = false;
                }
            };
        }
    }

    /// <summary>
    /// 攻撃が終了したらガードを解除
    /// </summary>
    /// <param name="get_flag"></param>
    private void HandleAttackEnded(bool get_flag)
    {
        if (!get_flag)
        {
            isGuard = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack") || other.CompareTag("Projectile"))
        {
            col = other; // 衝突したコライダーを保存
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Attack")
        {
            // 上位階層を遡ってColAnimationEventを探す
            Transform currentTransform = other.transform;
            ColAnimationEvent attackerEvent = null;

            while (currentTransform != null)
            {
                attackerEvent = currentTransform.GetComponent<ColAnimationEvent>();
                if (attackerEvent != null)
                {
                    break;
                }
                currentTransform = currentTransform.parent;
            }

            if (attackerEvent != null)
            {
                // 自分自身の攻撃なら無視する
                if (attackerEvent == colAnimationEvent)
                {
                    Debug.Log("自分自身の攻撃なので無視します。");
                    return;
                }
            }

            if (attackerEvent != null && attackerEvent == enemyColAnim)
            {
                isGuard = true;
            }
        }
        else if (other.tag == "Projectile")
        {
            isGuard = true;
        }
    }

    /// <summary>
    /// ガードエフェクトを生成
    /// </summary>
    public void InstantiateGuardEffect()
    {
        if (canInstantiateEffect)
        {
            Vector3 position = col.ClosestPoint(transform.position); // 衝突した場所の近くの点を取得
            Instantiate(guardEffect, position, Quaternion.identity); // ガードエフェクトを生成
            canInstantiateEffect = false; // エフェクト生成を一時的に無効にする
            StartCoroutine(ResetEffectInstantiation()); // 一定時間後にエフェクト生成を再度有効にする
        }
    }

    /// <summary>
    /// エフェクト生成を再度有効にする
    /// </summary>
    private IEnumerator ResetEffectInstantiation()
    {
        yield return new WaitForSeconds(0.1f);
        canInstantiateEffect = true; 
    }

    /// <summary>
    /// ガード状態を強制解除
    /// </summary>
    private void ResetGuard()
    {
        guard = 0.0f;
        isGuard = false;
    }

    public bool IsGuard
    {
        get { return isGuard; }
        set { isGuard = value; }
    }
}
