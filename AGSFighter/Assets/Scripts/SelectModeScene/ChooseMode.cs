using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChooseMode : MonoBehaviour
{

    // 初期化処理
    private void Start()
    {
        // 必要な初期化処理があればここに追加
    }

    // キャラクターを選択した時に実行しキャラクターデータをMyGameManagerDataにセット
    public void OnSelectMode()
    {
        // コントローラー使用の場合はここを消す
        // ボタンの選択状態を解除して選択したボタンのハイライト表示を可能にする為に実行
        EventSystem.current.SetSelectedGameObject(null);
    }

    // キャラクターを選択した時に背景をオンにする
    public void SwitchButtonBackground(int buttonNumber)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SetButtonBackground(i, buttonNumber - 1);
        }
    }

    // ボタンの背景を設定する処理
    private void SetButtonBackground(int index, int selectedButtonIndex)
    {
        bool isActive = (index == selectedButtonIndex);
        transform.GetChild(index).Find("Background").gameObject.SetActive(isActive);
    }
}

