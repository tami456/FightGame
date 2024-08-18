using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour
{
    // 点滅スピード
    [SerializeField]
    private float speed = 1.0f; // 点滅の速さを調整するための変数
    private Text text; // Textコンポーネントを保持する変数
    private float time; // 時間を追跡するための変数

    // Awakeはスクリプトインスタンスがロードされたときに呼び出される
    void Awake()
    {
        // Textコンポーネントを取得してキャッシュする
        text = GetComponent<Text>();
    }

    // Updateは毎フレーム呼び出される
    void Update()
    {
        // 新しい色を計算し、変更があれば適用する
        Color newColor = GetAlphaColor(text.color);
        if (text.color != newColor)
        {
            text.color = newColor;
        }
    }

    // 文字の点滅効果を計算する
    Color GetAlphaColor(Color color)
    {
        // 時間を更新し、PingPong関数を使ってアルファ値を計算する
        time += Time.deltaTime * speed;
        color.a = Mathf.PingPong(time, 1.0f);

        return color;
    }
}
