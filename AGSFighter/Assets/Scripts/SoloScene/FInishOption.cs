using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FInishOption : MonoBehaviour
{
    // ボタンの参照
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button modeButton;
    [SerializeField]
    private Button titleButton;

    // 初期化処理
    void Start()
    {
        SetupMenuUIEvent();
    }

    // ボタンを選択する処理
    public void ButtonSelect()
    {
        restartButton.Select();
    }

    // メニューUIのイベント設定
    private void SetupMenuUIEvent()
    {
        restartButton.onClick.AddListener(RestartGame);
        modeButton.onClick.AddListener(LoadSelectModeScene);
        titleButton.onClick.AddListener(LoadTitleScene);
    }

    // ゲームを再スタートする処理
    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SoloGameScene");
    }

    // モード選択シーンを読み込む処理
    private void LoadSelectModeScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SelectModeScene");
    }

    // タイトルシーンを読み込む処理
    private void LoadTitleScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }
}
