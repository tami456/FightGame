using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour
{
    // プレイヤーのアクションを管理するクラス
    [SerializeField]
    private PlayerAction player;

    // 破壊の閾値
    private int destructionThreshold = 1000;

    // 蓄積されたダメージ
    private int accumulatedDamage = 0;
    private int damage = 0;

    // 飛んでいく力
    [SerializeField]
    private float flyingForce = 0.0f;

    // 飛んでいく車のパーツ
    [SerializeField]
    private GameObject[] carParts;
    private int partsIndex = 0;

    // 攻撃マネージャー
    [SerializeField]
    private AttackManager attackManager;

    // カウントダウン
    [SerializeField]
    private SoloModeCountDown countDown;

    // スコア
    private Score score;
    private int carBreakScore = 0;

    // UIのアニメーション
    [SerializeField]
    private UIAnimation uiAnim;

    // SE、BGM
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip se1;
    [SerializeField]
    private AudioClip se2;

    // 初期化処理
    void Start()
    {
        score = GetComponent<Score>();
    }

    // ダメージを与える処理
    public void ApplyDamage(ColAnimationEvent attacker)
    {
        if (attacker.anim != null)
        {
            foreach (string key in attackManager.attackInfo.Keys)
            {
                if (attacker.anim.GetBool(key))
                {
                    damage = attackManager.GetAttackDetails(key).damage;
                }
            }
        }

        PlayDamageSound();
        AccumulateDamage(damage);
    }

    // プロジェクタイルのダメージを与える処理
    public void ApplyProjectileDamage(int damage)
    {
        PlayDamageSound();
        AccumulateDamage(damage);
    }

    // ダメージ音を再生する処理
    private void PlayDamageSound()
    {
        audioSource.PlayOneShot(se1);
    }

    // ダメージを蓄積する処理
    private void AccumulateDamage(int damage)
    {
        accumulatedDamage += damage;

        if (accumulatedDamage >= destructionThreshold)
        {
            ShatterScatterPart();
            accumulatedDamage -= destructionThreshold;
            Debug.Log(accumulatedDamage);
        }
    }

    // 車のパーツを飛ばす処理
    private void ShatterScatterPart()
    {
        audioSource.PlayOneShot(se2);

        if (carParts[partsIndex] == null)
        {
            HandleAllPartsDestroyed();
            return;
        }

        if (carParts.Length > 0)
        {
            LaunchCarPart();
            carBreakScore = 0;
            partsIndex++;
        }
    }

    // 全てのパーツが破壊されたときの処理
    private void HandleAllPartsDestroyed()
    {
        Debug.Log("もうパーツはありません");
        carBreakScore = 30000;
        player.State = PlayerAction.MyState.Freeze;
        countDown.StartStopTime(true);
        score.TotalScore(carBreakScore);
        uiAnim.SceneAnimFalse();
    }

    // 車のパーツを飛ばす処理
    private void LaunchCarPart()
    {
        GameObject randomPart = carParts[partsIndex];
        Rigidbody partRb = randomPart.GetComponent<Rigidbody>();

        if (partRb != null)
        {
            partRb.useGravity = true;
            partRb.isKinematic = false;
            partRb.AddForce(Random.onUnitSphere * flyingForce, ForceMode.Impulse); // ランダムな方向に力を加える
        }
    }

    // 車のスコアを取得する処理
    public int GetCarScore()
    {
        return carBreakScore;
    }
}
