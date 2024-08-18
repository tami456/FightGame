using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OptionButton : MonoBehaviour
{
    // オプションメニュー関連のUI要素
    [SerializeField]
    private GameObject option, optionBG, audioSource, commandList, commandSP, key;
    [SerializeField]
    private List<GameObject> commandImage;
    [SerializeField]
    private Button audioButton;
    [SerializeField]
    private Button commandButton;
    [SerializeField]
    private Button keyButton;
    [SerializeField]
    private Button titleButton;
    [SerializeField]
    private Button closeButton;

    // オプションメニューの状態を管理するフラグ
    private bool optionActive = false;

    // 初期化処理
    void Start()
    {
        SetupMenuUIEvent();
    }

    // 毎フレーム呼び出される更新処理
    private void Update()
    {
        HandleOptionClose();
    }

    // オプションメニューを閉じる処理
    private void HandleOptionClose()
    {
        if (OptionActiveFalse(audioSource.activeSelf))
        {
            CloseOptionMenu(audioSource);
        }
        else if (OptionActiveFalse(commandList.activeSelf))
        {
            CloseOptionMenu(commandList);
            foreach (GameObject command in commandImage)
            {
                command.SetActive(false);
            }
        }
        else if (OptionActiveFalse(key.activeSelf))
        {
            CloseOptionMenu(key);
        }
    }

    // オプションメニューを閉じる条件をチェック
    private bool OptionActiveFalse(bool optionActive)
    {
        return optionActive && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Joystick1Button2));
    }

    // ポーズメニューの入力処理
    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        ToggleOptionMenu();
    }

    // オプションメニューの表示/非表示を切り替える
    private void ToggleOptionMenu()
    {
        optionActive = !optionActive;
        Time.timeScale = optionActive ? 0f : 1f;
        option.SetActive(optionActive);
        if (optionActive)
        {
            audioButton.Select();
        }
        else
        {
            CloseAllOptionMenus();
        }
    }

    // 全てのオプションメニューを閉じる
    private void CloseAllOptionMenus()
    {
        option.SetActive(false);
        audioSource.SetActive(false);
        commandList.SetActive(false);
        foreach (GameObject command in commandImage)
        {
            command.SetActive(false);
        }
        key.SetActive(false);
    }

    // メニューUIのイベント設定
    private void SetupMenuUIEvent()
    {
        audioButton.onClick.AddListener(() => OpenOptionMenu(audioSource));
        commandButton.onClick.AddListener(() => OpenOptionMenu(commandList, commandSP));
        keyButton.onClick.AddListener(() => OpenOptionMenu(key));
        titleButton.onClick.AddListener(LoadTitleScene);
        closeButton.onClick.AddListener(CloseOptionMenu);
    }

    // オプションメニューを開く処理
    private void OpenOptionMenu(GameObject menu, GameObject additionalMenu = null)
    {
        option.SetActive(false);
        optionBG.SetActive(true);
        menu.SetActive(true);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }
    }

    // オプションメニューを閉じる処理
    private void CloseOptionMenu(GameObject menu)
    {
        option.SetActive(true);
        optionBG.SetActive(false);
        menu.SetActive(false);
    }

    // タイトルシーンを読み込む処理
    private void LoadTitleScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
        SoundManager.Instance.StopBGM();
    }

    // オプションメニューを閉じる処理
    private void CloseOptionMenu()
    {
        Time.timeScale = 1f;
        option.SetActive(false);
        optionActive = false;
    }
}
