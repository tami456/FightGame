using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoloModeCountDown : MonoBehaviour
{
    // カウントダウンが有効かどうかのフラグ
    [SerializeField]
    private bool isCount = false;
    // 残り時間
    private int remainingTime;
    // カウントダウンタイム
    [SerializeField]
    private float countDownTime;
    // 表示用テキストUI
    [SerializeField]
    private Text textCountDown;
    // プレイヤーのアクションを管理するクラス
    [SerializeField]
    private PlayerAction player;
    // スコアを管理するクラス
    [SerializeField]
    private Score score;
    // 車のダメージを管理するクラス
    [SerializeField]
    private CarDamage carDamage;
    // UIアニメーションを管理するクラス
    private UIAnimation uiAnim;

    // 残り時間を取得するプロパティ
    public int GetTime() => remainingTime;

    // カウントダウンの開始/停止を設定するメソッド
    public void StartStopTime(bool isCount)
    {
        this.isCount = isCount;
    }

    // 初期化処理
    private void Start()
    {
        uiAnim = GetComponent<UIAnimation>();
    }

    // 毎フレーム呼び出される更新処理
    private void Update()
    {
        if (!isCount)
        {
            UpdateCountDown();
        }
    }

    // カウントダウンの更新処理
    private void UpdateCountDown()
    {
        // カウントダウンタイムを整形して表示
        textCountDown.text = string.Format("{0:00}", countDownTime);
        if (countDownTime > 0)
        {
            // 経過時刻を引いていく
            countDownTime -= Time.deltaTime;
            remainingTime = Mathf.RoundToInt(countDownTime);
        }
        else
        {
            EndCountDown();
        }
    }

    // カウントダウン終了時の処理
    private void EndCountDown()
    {
        player.State = PlayerAction.MyState.Freeze;
        uiAnim.SceneAnimFalse();
        score.TotalScore(carDamage.GetCarScore());
        SoundManager.Instance.StopBGM();
        Debug.Log("Finish!!");
    }
}
