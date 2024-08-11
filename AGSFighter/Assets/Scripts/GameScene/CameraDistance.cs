using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistance : MonoBehaviour
{
    public Transform player1; // �v���C���[1��Transform
    public Transform player2; // �v���C���[2��Transform
    public float minDistance = 5f; // �J�������v���C���[�ɍł��߂Â�����
    public float maxDistance = 20f; // �J�������v���C���[����ł��������鋗��
    public float minZOffset = -10f; // �f�t�H���g��Z�ʒu����̍ŏ��I�t�Z�b�g
    public float maxZOffset = -30f; // �f�t�H���g��Z�ʒu����̍ő�I�t�Z�b�g
    public float moveSpeed = 10f; // �J�����̈ړ����x
    public float distanceThreshold = 1f; // �v���C���[�Ԃ̋���������臒l�𒴂�����J�����������o��
    private float defaultZ = -60f; // �J�����̃f�t�H���g��Z���W
    private Vector3 previousTargetPosition;

    public float minFOV = 60f; // �v���C���[���߂Â����Ƃ��̍ŏ�FOV
    public float maxFOV = 70f; // �v���C���[�����ꂽ�Ƃ��̍ő�FOV

    void Start()
    {
        // �J�����̏����ʒu��ݒ�
        previousTargetPosition = new Vector3(transform.position.x, transform.position.y, defaultZ);
    }

    void LateUpdate()
    {
        // �v���C���[�Ԃ̋������v�Z
        float distance = Vector3.Distance(player1.position, player2.position);

        // �v���C���[�̒��S�ʒu���v�Z
        Vector3 centerPosition = (player1.position + player2.position) / 2f;

        // �v���C���[�Ԃ̋�����臒l�𒴂�����J�������ړ�
        if (Mathf.Abs(distance - minDistance) > distanceThreshold)
        {
            // �����Ɋ�Â��ăJ������Z���W�I�t�Z�b�g���v�Z
            float clampedDistance = Mathf.Clamp(distance, minDistance, maxDistance);
            float targetZOffset = Mathf.Lerp(minZOffset, maxZOffset, (clampedDistance - minDistance) / (maxDistance - minDistance));

            // �f�t�H���g��Z���W�ɃI�t�Z�b�g���������^�[�Q�b�gZ���W���v�Z
            float targetZ = defaultZ + targetZOffset;

            // �^�[�Q�b�g�ʒu���v�Z�i���S�ʒu�ƃ^�[�Q�b�gZ���W���g�p�j
            Vector3 targetPosition = new Vector3(centerPosition.x, transform.position.y, targetZ);

            // �J�����ʒu���X���[�Y�Ɉړ�
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // FOV�������ɉ����Ē����i�c�����̕ω����ŏ����Ɂj
            float targetFOV = Mathf.Lerp(minFOV, maxFOV, (clampedDistance - minDistance) / (maxDistance - minDistance));
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, targetFOV, moveSpeed * Time.deltaTime);

            // �O��̃^�[�Q�b�g�ʒu���X�V
            previousTargetPosition = targetPosition;
        }
    }
}