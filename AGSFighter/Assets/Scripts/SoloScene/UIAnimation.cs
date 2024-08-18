using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    // プレイヤーのアクションを管理するクラス
    [SerializeField]
    private PlayerAction player;

    // UI要素
    [SerializeField]
    private GameObject ready;
    [SerializeField]
    private GameObject go;
    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private GameObject finishOption;
    [SerializeField]
    private FInishOption finishPanel;

    // オーディオ関連
    [SerializeField]
    private AudioSource readyAudioSource;
    [SerializeField]
    private AudioClip readyAudio;

    // アニメーターとカウントダウン
    private Animator anim;
    private SoloModeCountDown countDown;

    // 初期化処理
    private void Start()
    {
        InitializeComponents();
        StartCountdown();
        TriggerReadyAnimation();
        FreezePlayer();
    }

    // コンポーネントの初期化
    private void InitializeComponents()
    {
        countDown = GetComponent<SoloModeCountDown>();
        anim = GetComponent<Animator>();
    }

    // カウントダウンの開始
    private void StartCountdown()
    {
        countDown.StartStopTime(true);
    }

    // "Ready"アニメーションのトリガー
    private void TriggerReadyAnimation()
    {
        anim.SetTrigger("Ready");
    }

    // プレイヤーをフリーズ状態にする
    private void FreezePlayer()
    {
        player.State = PlayerAction.MyState.Freeze;
    }

    // スコアが有効になったときの処理
    private void ScoreTrue()
    {
        anim.SetTrigger("ScoreTrue");
        ui.SetActive(false);
    }

    // ゲーム終了アニメーションのトリガー
    public void SceneAnimFalse()
    {
        anim.SetTrigger("GameFinish");
    }

    // "Ready"オーディオの再生
    private void ReadyAudio()
    {
        readyAudioSource.PlayOneShot(readyAudio);
    }

    // "GO"テキストの表示とアニメーションのトリガー
    private void GOTextStart()
    {
        ready.SetActive(false);
        go.SetActive(true);
        anim.SetTrigger("GO");
        SoundManager.Instance.PlayUIClip("男衆「始めいッ！」");
    }

    // "GO"テキストの非表示とカウントダウンの再開
    private void GOFalse()
    {
        go.SetActive(false);
        countDown.StartStopTime(false);
        player.State = PlayerAction.MyState.Game;
    }

    // 終了オプションの表示
    private void FinishOptionTrue()
    {
        finishOption.SetActive(true);
        finishPanel.ButtonSelect();
    }
}
