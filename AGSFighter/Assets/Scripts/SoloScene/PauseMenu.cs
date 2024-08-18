using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    // ボタンの参照
    [SerializeField]
    private Button backBattleButton;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button modeButton;
    [SerializeField]
    private Button titleButton;

    // メニューUIと背景
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private GameObject menuBG;

    // ポーズメニューの状態を管理するフラグ
    private bool isPaused = false;

    // 初期化処理
    void Start()
    {
        SetupMenuUIEvent();
    }

    // 有効化時の処理
    private void OnEnable()
    {
        backBattleButton.Select();
    }

    // ポーズメニューの入力処理
    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        TogglePauseMenu();
    }

    // ポーズメニューの表示/非表示を切り替える
    private void TogglePauseMenu()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        menuUI.SetActive(isPaused);
        menuBG.SetActive(isPaused);
    }

    // メニューUIのイベント設定
    private void SetupMenuUIEvent()
    {
        backBattleButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        modeButton.onClick.AddListener(LoadSelectModeScene);
        titleButton.onClick.AddListener(LoadTitleScene);
    }

    // ゲームを再開する処理
    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        menuUI.SetActive(false);
        menuBG.SetActive(false);
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
