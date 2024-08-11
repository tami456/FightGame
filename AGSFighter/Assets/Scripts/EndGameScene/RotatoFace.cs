using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatoFace : MonoBehaviour
{
    public Transform neckBone;
    private GameObject watchTarget;
    private void Start()
    {
        watchTarget = GameObject.Find("Main Camera");
    }

    protected virtual void LateUpdate()
    {
        if (neckBone != null && watchTarget != null)
        {
            Vector3 targetPosition = watchTarget.transform.position;
            Vector3 selfPosition = transform.position;
            Vector3 direction = targetPosition - selfPosition;

            // Y���̉�]�p�x���v�Z
            float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // X���̉�]�p�x���v�Z�i�㉺�̎����ړ��j
            float angleX = Mathf.Atan2(direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;

            // �l�b�N�{�[���̉�]�����Z�b�g
            neckBone.rotation = Quaternion.identity;

            // Y����X���̉�]��K�p
            neckBone.Rotate(-angleX, angleY, 0f);
        }
    }
}
