using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckInversion : MonoBehaviour
{
    // �v���C���[�̃A�N�V�������Ǘ�����N���X
    private PlayerAction player;

    // Ray�̒���
    [SerializeField]
    private float rayLength = 0.1f;

    // Ray���ǂꂭ�炢�̂ɂ߂荞�܂��邩
    [SerializeField]
    private float rayOffset;

    // Ray�̔���ɗp����Layer
    [SerializeField]
    private LayerMask layerMask = default;

    // ���]��Ԃ̃t���O
    [SerializeField]
    private bool isInversion = false;

    // Ray�̕���
    private Vector3 vec = Vector3.right;
    private Ray ray;

    // ����������
    void Start()
    {
        player = GetComponent<PlayerAction>();
    }

    // ���t���[���Ăяo�����X�V����
    void Update()
    {
        UpdateRayDirection();
    }

    // �Œ�t���[�����[�g�ŌĂяo�����X�V����
    private void FixedUpdate()
    {
        isInversion = CheckInversion();
    }

    // ���]��Ԃ��擾���郁�\�b�h
    public bool GetIsInversion()
    {
        return isInversion;
    }

    // ���]��Ԃ��`�F�b�N���郁�\�b�h
    private bool CheckInversion()
    {
        if (player.GetIsGround())
        {
            // �������̏����ʒu�Ǝp��
            ray = new Ray(transform.position + Vector3.up * rayOffset, vec);
        }
        // Raycast��hit���邩�ǂ����Ŕ���
        return Physics.Raycast(ray, rayLength, layerMask);
    }

    // Ray�̕������X�V���郁�\�b�h
    private void UpdateRayDirection()
    {
        if (!isInversion)
        {
            vec *= -1;
        }
    }

    // Gizmos��`�悷�郁�\�b�h
    private void OnDrawGizmos()
    {
        Gizmos.color = isInversion ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, vec);
    }
}
