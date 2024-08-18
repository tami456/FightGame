using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class TitleScene : MonoBehaviour
    {
        // スタート時にフェードインを開始
        private void Start()
        {
            StartCoroutine(FadeController.Instance.FadeIn());
        }

        // 毎フレーム呼び出される
        public void Update()
        {
            // 何かキーが押された場合
            if (Input.anyKeyDown)
            {
                // 効果音を再生
                SoundManager.Instance.PlayUIClip("ガラスが割れる1");
                // チュートリアルシーンへ移動
                SceneManager.LoadScene("TutorialScene");
            }
        }
    }
}
