using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGuardPose : MonoBehaviour
{
    [SerializeField]
    private ColAnimationEvent enemyColAnim; //敵の攻撃アニメーションイベント
    private bool isGuard = false; // ガード状態を示すフラグ
    private float guard = 0.0f; // ガードの持続時間

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

    // 攻撃が終了したらガードを解除
    private void HandleAttackEnded(bool get_flag)
    {
        if (!get_flag)
        {
            isGuard = false; 
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

            if (attackerEvent != null && attackerEvent == enemyColAnim)
            {
                isGuard = true;
            }
        }
        else if (other.tag == "Projectile")
        {
            IsGuard = false;
        }
    }

    //ガード状態を強制解除
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
