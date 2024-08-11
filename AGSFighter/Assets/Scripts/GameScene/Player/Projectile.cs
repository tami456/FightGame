using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float impactEffectDuration = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            // �v���C���[�Ƀ_���[�W��^���鏈��
            PlayerHit playerHit = other.GetComponent<PlayerHit>();
            if (playerHit != null)
            {
                playerHit.TakeDamageProjectile((damageAmount, "�Ή����@1(�g����)", AttackLevel.Mid, AttackType.Middle));
            }

            // �G�t�F�N�g�𐶐����Ĉ�莞�Ԍ�ɏ���
            Vector3 position = other.ClosestPoint(transform.position);
            Instantiate(impactEffect, position, Quaternion.identity);
            Destroy(impactEffect, impactEffectDuration);

            // ��ѓ������������
            Destroy(gameObject);
        }
    }
}
