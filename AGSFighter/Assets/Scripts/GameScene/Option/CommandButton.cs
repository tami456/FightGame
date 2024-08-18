using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    // ボタンの参照
    [SerializeField]
    private Button SPButton;
    [SerializeField]
    private Button SPMButton;
    [SerializeField]
    private Button SystemButton;

    // コマンドの参照
    [SerializeField]
    private GameObject SPCom;
    [SerializeField]
    private GameObject SPMCom;
    [SerializeField]
    private GameObject SystemCom;
    [SerializeField]
    private GameObject SelectCom;

    // 初期化処理
    void Start()
    {
        SetupMenuUIEvent();
    }

    // 有効化時の処理
    private void OnEnable()
    {
        SPButton.Select();
    }

    // メニューUIのイベント設定
    private void SetupMenuUIEvent()
    {
        SPButton.onClick.AddListener(() => ShowCommand(SPCom));
        SPMButton.onClick.AddListener(() => ShowCommand(SPMCom));
        SystemButton.onClick.AddListener(() => ShowCommand(SystemCom));
    }

    // コマンドを表示する処理
    private void ShowCommand(GameObject activeCommand)
    {
        SPCom.SetActive(activeCommand == SPCom);
        SPMCom.SetActive(activeCommand == SPMCom);
        SystemCom.SetActive(activeCommand == SystemCom);
    }
}
