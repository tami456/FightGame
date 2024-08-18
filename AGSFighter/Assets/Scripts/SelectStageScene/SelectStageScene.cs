using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class SelectStageScene : MonoBehaviour
    {
        // ローディングスピナーの参照
        public GameObject loadingSpinner;

        // 初期化処理
        private void Start()
        {
            InitializeLoadingSpinner();
            CheckRoundManagerInstance();
        }

        // ローディングスピナーを初期状態で非表示に設定
        private void InitializeLoadingSpinner()
        {
            loadingSpinner.SetActive(false);
        }

        // RoundManagerのインスタンスをチェック
        private void CheckRoundManagerInstance()
        {
            if (RoundManager.Instance == null)
            {
                Debug.LogError("RoundManagerのインスタンスが見つかりません。シーンに配置されていることを確認してください。");
            }
        }

        // 他のシーンに移動する処理
        public void GoToOtherScene(string stage)
        {
            SaveSelectedStage(stage);
            StartCoroutine(LoadSceneAsync(stage));
        }

        // 選択されたステージをRoundManagerに保存
        private void SaveSelectedStage(string stage)
        {
            if (RoundManager.Instance != null)
            {
                RoundManager.Instance.SetSelectedStage(stage);
            }
            else
            {
                Debug.LogError("RoundManagerのインスタンスがありません");
            }
        }

        // シーンの非同期読み込み処理
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            ShowLoadingSpinner();

            // シーンの非同期読み込みを開始
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            // 読み込みが完了するまで待つ
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            HideLoadingSpinner();
        }

        // ローディングスピナーを表示
        private void ShowLoadingSpinner()
        {
            loadingSpinner.SetActive(true);
        }

        // ローディングスピナーを非表示
        private void HideLoadingSpinner()
        {
            loadingSpinner.SetActive(false);
        }
    }
}
