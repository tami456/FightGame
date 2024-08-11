using UnityEngine;
using Cinemachine;

public class DynamicCameraController : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineTargetGroup targetGroup;

    public float minDistance = 5f; // �J�������v���C���[�ɍł��߂Â�����
    public float maxDistance = 20f; // �J�������v���C���[����ł��������鋗��
    public float minFOV = 40f; // �ŏ�FOV
    public float maxFOV = 60f; // �ő�FOV
    public float moveSpeed = 10f; // �J�����̈ړ����x

    private void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        if (targetGroup == null)
        {
            targetGroup = GetComponent<CinemachineTargetGroup>();
        }

        // �v���C���[���^�[�Q�b�g�O���[�v�ɒǉ�
        targetGroup.AddMember(player1, 1f, 0f);
        targetGroup.AddMember(player2, 1f, 0f);
    }

    private void LateUpdate()
    {
        // �v���C���[�Ԃ̋������v�Z
        float distance = Vector3.Distance(player1.position, player2.position);

        // �����Ɋ�Â��ăJ������FOV�𒲐�
        float clampedDistance = Mathf.Clamp(distance, minDistance, maxDistance);
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, (clampedDistance - minDistance) / (maxDistance - minDistance));
        virtualCamera.m_Lens.FieldOfView = targetFOV;

        // �^�[�Q�b�g�O���[�v�̒��S�ʒu���v�Z
        Vector3 centerPosition = (player1.position + player2.position) / 2f;
        centerPosition.z = virtualCamera.transform.position.z; // Z���͌Œ�

        // �J�����ʒu���X���[�Y�Ɉړ�
        virtualCamera.transform.position = Vector3.MoveTowards(virtualCamera.transform.position, centerPosition, moveSpeed * Time.deltaTime);
    }
}
