using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("LeftHandCollider")) // �v���C���[1�̍���R���C�_�[�̃^�O���`�F�b�N
        {
            Debug.Log("�v���C���[2���v���C���[1�̃p���`���󂯂܂����I");
        }
    }
}