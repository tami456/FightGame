using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // UI要素
    [SerializeField]
    private Text score; // 現在のスコアを表示するテキスト
    [SerializeField]
    private Text highScore; // ハイスコアを表示するテキスト
    [SerializeField]
    private Text timeScoreText; // タイムスコアを表示するテキスト
    [SerializeField]
    private Text perfectScoreText; // パーフェクトスコアを表示するテキスト
    [SerializeField]
    private Text totalScoreText; // トータルスコアを表示するテキスト
    [SerializeField]
    private Text scoreText; // 最終スコアを表示するテキスト

    // スコア関連の変数
    private int scoreIndex; // 現在のスコアインデックス
    private int scoreCurrentIndex; // 現在のスコア
    private int timeScore; // タイムスコア
    private int carBreakScore; // 車の破壊スコア
    private int totalScore; // トータルスコア
    private int finishScore; // 最終スコア

    // 他のコンポーネント
    [SerializeField]
    private AttackManager attackManager; // 攻撃マネージャー
    [SerializeField]
    private SoloModeCountDown countDown; // カウントダウン
    [SerializeField]
    private UIAnimation uiAnim; // UIアニメーション

    // 初期化処理
    void Start()
    {
        InitializeHighScore();
    }

    // ハイスコアの初期化
    private void InitializeHighScore()
    {
        // スコアリセット
        // PlayerPrefs.SetInt("HighScore", 0);
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    // 毎フレーム呼び出される更新処理
    void Update()
    {
        CheckCountDown();
    }

    // カウントダウンのチェック
    private void CheckCountDown()
    {
        if (countDown.GetTime() < 0)
        {
            uiAnim.SceneAnimFalse();
        }
    }

    // トリガーに衝突したときの処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            ColAnimationEvent attackerEvent = GetAttackerEvent(other.transform);
            if (attackerEvent != null)
            {
                AddScore(attackerEvent);
            }
        }
    }

    // 攻撃者のイベントを取得
    private ColAnimationEvent GetAttackerEvent(Transform currentTransform)
    {
        while (currentTransform != null)
        {
            ColAnimationEvent attackerEvent = currentTransform.GetComponent<ColAnimationEvent>();
            if (attackerEvent != null)
            {
                return attackerEvent;
            }
            currentTransform = currentTransform.parent;
        }
        return null;
    }

    // スコアを追加
    public void AddScore(ColAnimationEvent attacker)
    {
        if (attacker.anim != null)
        {
            foreach (string key in attackManager.attackInfo.Keys)
            {
                if (attacker.anim.GetBool(key))
                {
                    scoreIndex = attackManager.GetAttackDetails(key).damage;
                    scoreCurrentIndex += scoreIndex;
                }
            }
            score.text = scoreCurrentIndex.ToString();
        }
    }

    // プロジェクタイルのスコアを追加
    public void AddProjectileScore(int damage)
    {
        scoreIndex = damage;
        scoreCurrentIndex += scoreIndex;
        score.text = scoreCurrentIndex.ToString();
    }

    // トータルスコアを計算
    public void TotalScore(int carBreakScore)
    {
        timeScore = countDown.GetTime() * 1000;
        this.carBreakScore = carBreakScore;
        totalScore = timeScore + this.carBreakScore;
        UpdateScoreTexts();
        finishScore = totalScore + scoreCurrentIndex + scoreIndex;
        scoreText.text = finishScore.ToString();

        UpdateHighScore();
    }

    // スコアテキストを更新
    private void UpdateScoreTexts()
    {
        timeScoreText.text = timeScore.ToString();
        perfectScoreText.text = carBreakScore.ToString();
        totalScoreText.text = totalScore.ToString();
    }

    // ハイスコアを更新
    private void UpdateHighScore()
    {
        if (PlayerPrefs.GetInt("HighScore") < finishScore)
        {
            PlayerPrefs.SetInt("HighScore", finishScore);
        }
    }
}
