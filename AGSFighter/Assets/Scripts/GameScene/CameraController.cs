using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] players; // �v���C���[��Transform
    public float minDistance = 5f; // �J�������v���C���[�ɍł��߂Â�����
    public float moveSpeed = 10f; // �J�����̈ړ����x
    public float maxDistance = 20f; // �J�������v���C���[����ł��������鋗��
    public float distanceThreshold = 1f; // �v���C���[�Ԃ̋���������臒l�𒴂�����J�����������o��

    private Vector3 defaultPosition; // �J�����̃f�t�H���g�̈ʒu

    void Start()
    {
        // �J�����̏����ʒu���L��
        defaultPosition = transform.position;
    }

    void LateUpdate()
    {
        // �v���C���[�Ԃ̋������v�Z
        float distance = Vector3.Distance(players[0].position, players[1].position);

        // �v���C���[�Ԃ̋�����臒l�𒴂�����J�������ړ�
        if (Mathf.Abs(distance - minDistance) > distanceThreshold)
        {
            // �v���C���[�̒��S�ʒu���v�Z
            Vector3 centerPosition = (players[0].position + players[1].position) / 2f;

            // �f�t�H���g�ʒu����̃I�t�Z�b�g���v�Z
            Vector3 offset = transform.position - defaultPosition;

            // �v���C���[�Ԃ̋����Ɋ�Â��ăJ�����̈ʒu��ݒ�
            float clampedDistance = Mathf.Clamp(distance, minDistance, maxDistance);
            Vector3 targetPosition = centerPosition - transform.forward * clampedDistance + offset;

            // �J�����ʒu���X���[�Y�Ɉړ�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
