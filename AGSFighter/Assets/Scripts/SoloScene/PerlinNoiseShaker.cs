using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseShaker : MonoBehaviour
{
    // 単一のパーリンノイズ情報を格納する構造体
    [Serializable]
    private struct NoiseParam
    {
        // 振幅
        [SerializeField]
        private float amplitude;

        // 振動の速さ
        [SerializeField]
        private float speed;

        // パーリンノイズのオフセット
        private float offset;

        // 乱数のオフセット値を指定する
        public void SetRandomOffset()
        {
            offset = UnityEngine.Random.Range(0f, 256f);
        }

        // 指定時刻のパーリンノイズ値を取得する
        public float GetValue(float time)
        {
            // ノイズ位置を計算
            var noisePos = speed * time + offset;

            // -1〜1の範囲のノイズ値を取得
            var noiseValue = 2 * (Mathf.PerlinNoise(noisePos, 0) - 0.5f);

            // 振幅を掛けた値を返す
            return amplitude * noiseValue;
        }
    }

    // パーリンノイズのXYZ情報
    [Serializable]
    private struct NoiseTransform
    {
        public NoiseParam x, y, z;

        // xyz成分に乱数のオフセット値を指定する
        public void SetRandomOffset()
        {
            x.SetRandomOffset();
            y.SetRandomOffset();
            z.SetRandomOffset();
        }

        // 指定時刻のパーリンノイズ値を取得する
        public Vector3 GetValue(float time)
        {
            return new Vector3(
                x.GetValue(time),
                y.GetValue(time),
                z.GetValue(time)
            );
        }
    }

    // 位置の揺れ情報
    [SerializeField] private NoiseTransform _noisePosition;

    // 回転の揺れ情報
    [SerializeField] private NoiseTransform _noiseRotation;

    //車のtransform
    private Transform _transform;

    // Transformの初期状態
    private Vector3 _initLocalPosition;
    private Quaternion _initLocalQuaternion;

    // 初期化
    private void Awake()
    {
        _transform = transform;

        // Transformの初期値を保持
        _initLocalPosition = _transform.localPosition;
        _initLocalQuaternion = _transform.localRotation;

        // パーリンノイズのオフセット初期化
        _noisePosition.SetRandomOffset();
        _noiseRotation.SetRandomOffset();
    }

    public void StartNoise(float timeN)
    {
        // パーリンノイズの値を時刻から取得
        var noisePos = _noisePosition.GetValue(timeN);
        var noiseRot = _noiseRotation.GetValue(timeN);

        // 各Transformにパーリンノイズの値を加算
        _transform.localPosition = _initLocalPosition + noisePos;
        _transform.localRotation = Quaternion.Euler(noiseRot) * _initLocalQuaternion;
    }
}
