using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipPanel : MonoBehaviour
{
    // スキップ確認パネル
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Button leftButton;
    [SerializeField]
    private Button rightButton;

    // チュートリアルを受けるかどうかのテキスト
    [SerializeField]
    private GameObject tutoText;
    [SerializeField]
    private GameObject tutoTextSkip;

    // チュートリアルをスキップするかどうかのテキスト
    [SerializeField]
    private GameObject skipText;
    [SerializeField]
    private GameObject skipTextNo;

    // 最初のチュートリアル
    [SerializeField]
    private GameObject tuto1;

    // チュートリアルを集めた親
    [SerializeField]
    private GameObject tuto;

    // プレイヤーのState変更のため
    [SerializeField]
    private GameObject player;
    private PlayerAction playerState;
    private bool tutorialSkipped = false;

    // 初期化処理
    private void Start()
    {
        StartCoroutine(FadeController.Instance.FadeIn());
        playerState = player.GetComponent<PlayerAction>();
        playerState.State = PlayerAction.MyState.Freeze;
        SetUpMenuUIEvent();
    }

    // 有効化時の処理
    private void OnEnable()
    {
        leftButton.Select();
    }

    // ポーズメニューの入力処理
    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        // チュートリアルの途中でスキップパネルを出す
        if (playerState.State == PlayerAction.MyState.Game)
        {
            SkipTutorial();
            leftButton.Select();
        }
    }

    // チュートリアルをスキップ
    private void SkipTutorial()
    {
        SetTutorialActive(false,false);
        panel.SetActive(true);
        SetSkipTextActive(true);
        playerState.State = PlayerAction.MyState.Freeze;
        tutorialSkipped = true;
    }

    // シーン読み込み
    private void LoadSelectModeScene()
    {
        SceneManager.LoadScene("SelectModeScene");
    }

    // メニューUIのイベント設定
    private void SetUpMenuUIEvent()
    {
        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);
    }

    // 左のボタンがクリックされたときの処理
    private void OnLeftButtonClick()
    {
        SetTutorialActive(false,true);
        panel.SetActive(false);
        SetSkipTextActive(false);

        // チュートリアルの途中でスキップパネルが表示されていたらスキップする用ボタンになる
        if (!tutorialSkipped)
        {
            tuto1.SetActive(true);
        }
        else
        {
            LoadSelectModeScene();
        }

        playerState.State = PlayerAction.MyState.Game;
    }

    // 右のボタンがクリックされたときの処理
    private void OnRightButtonClick()
    {
        if (!tutorialSkipped)
        {
            LoadSelectModeScene();
        }
        else
        {
            SetTutorialActive(false,true);
            SetSkipTextActive(false);
            panel.SetActive(false);
            playerState.State = PlayerAction.MyState.Game;
        }
    }

    // チュートリアルの表示/非表示を設定
    private void SetTutorialActive(bool panelTextActive,bool isActive)
    {
        tuto.SetActive(isActive);
        tutoText.SetActive(panelTextActive);
        tutoTextSkip.SetActive(isActive);
    }

    // スキップテキストの表示/非表示を設定
    private void SetSkipTextActive(bool isActive)
    {
        skipText.SetActive(isActive);
        skipTextNo.SetActive(isActive);
    }
}
