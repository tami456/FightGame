using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckInversion : MonoBehaviour
{
    // プレイヤーのアクションを管理するクラス
    private PlayerAction player;

    // Rayの長さ
    [SerializeField]
    private float rayLength = 0.1f;

    // Rayをどれくらい体にめり込ませるか
    [SerializeField]
    private float rayOffset;

    // Rayの判定に用いるLayer
    [SerializeField]
    private LayerMask layerMask = default;

    // 反転状態のフラグ
    [SerializeField]
    private bool isInversion = false;

    // Rayの方向
    private Vector3 vec = Vector3.right;
    private Ray ray;

    // 初期化処理
    void Start()
    {
        player = GetComponent<PlayerAction>();
    }

    // 毎フレーム呼び出される更新処理
    void Update()
    {
        UpdateRayDirection();
    }

    // 固定フレームレートで呼び出される更新処理
    private void FixedUpdate()
    {
        isInversion = CheckInversion();
    }

    // 反転状態を取得するメソッド
    public bool GetIsInversion()
    {
        return isInversion;
    }

    // 反転状態をチェックするメソッド
    private bool CheckInversion()
    {
        if (player.GetIsGround())
        {
            // 放つ光線の初期位置と姿勢
            ray = new Ray(transform.position + Vector3.up * rayOffset, vec);
        }
        // Raycastがhitするかどうかで判定
        return Physics.Raycast(ray, rayLength, layerMask);
    }

    // Rayの方向を更新するメソッド
    private void UpdateRayDirection()
    {
        if (!isInversion)
        {
            vec *= -1;
        }
    }

    // Gizmosを描画するメソッド
    private void OnDrawGizmos()
    {
        Gizmos.color = isInversion ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, vec);
    }
}
