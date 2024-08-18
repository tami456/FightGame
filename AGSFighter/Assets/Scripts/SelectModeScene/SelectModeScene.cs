using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class SelectModeScene : MonoBehaviour
    {
        // 初期化処理
        private void Start()
        {
            // 必要な初期化処理があればここに追加
        }

        // VSシーンに移動する処理
        public void GoToVSScene()
        {
            SceneManager.LoadScene("SelectStageScene");
        }

        // ソロシーンに移動する処理
        public void GoToSoloScene()
        {
            SceneManager.LoadScene("SoloGameScene");
            SoundManager.Instance.PlayBGM("wanderer");
        }
    }
}
