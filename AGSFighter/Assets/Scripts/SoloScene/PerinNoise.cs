using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerinNoise : MonoBehaviour
{
    //�U��
    private bool isVibrating = false;
    private float vibrationDuration = 0.3f;
    private float vibrationTimer = 0f;
    [SerializeField]
    PerlinNoiseShaker noiseShaker;
    [SerializeField]
    GameObject impactEffect;

    [SerializeField]
    Animator playerAnim;
    [SerializeField]
    private int frameCount;
    private bool isCountingFrames;

    [SerializeField]
    CarDamage cardamage;
    [SerializeField]
    Score score;
    // ProjectileDamageDictionary�ւ̎Q��
    [SerializeField] private ProjectileDamageDictionary projectileDamageDictionary;
    (int, string, AttackLevel) damages = (0, ""
         , AttackLevel.NullLevel); // �����l�Ƃ��ă_���[�W�A�����̖��O�A�U�����x���̃^�v����ݒ肵�܂�
    public (int, string, AttackLevel) Damage
    {
        get { return this.damages; }  // �擾�p
        private set { this.damages = value; } // �l���͗p
    }
    private void Start()
    {
    }

    public void OnTriggerEnter(Collider col)
	{
        if (col.tag == "Attack")
        {
            Transform currentTransform = col.transform;
            ColAnimationEvent attackerEvent = null;
            while (currentTransform != null)
            {
                attackerEvent = currentTransform.GetComponent<ColAnimationEvent>();
                if (attackerEvent != null)
                {
                    break;
                }
                currentTransform = currentTransform.parent;
            }

            isVibrating = true;
            vibrationTimer = 0f;
            Debug.Log("�������Ă�");
            // �Փ˂����ꏊ�ɃG�t�F�N�g�𐶐�
            Vector3 position = col.ClosestPoint(transform.position); // �Փ˂����ꏊ�̋߂��̓_���擾
            Instantiate(impactEffect, position, Quaternion.identity);

            // Animator���~
            playerAnim.enabled = false;

            // �t���[���J�E���g�����Z�b�g
            frameCount = 0;

            // �t���[���J�E���g���J�n
            isCountingFrames = true;

            if (SceneManager.GetActiveScene().name == "SoloGameScene")
            {
                cardamage.ApplyDamage(attackerEvent);
                score.AddScore(attackerEvent);
            }
        }
        else if (col.CompareTag("Projectile"))
        {
            ProjectileAttack(col);
        }
    }

    private void ProjectileAttack(Collider other)
    {
        string projectileName = other.gameObject.name.Replace("(Clone)", "").Trim();

        if (projectileDamageDictionary.projectileInfo.TryGetValue(
            projectileName, out ProjectileDamageDictionary.ProjectileInfo damage))
        {

            Damage = (damage.damage, damage.soundEffect, damage.attackLevel);
            // �Փ˂����ꏊ�ɃG�t�F�N�g�𐶐�
            Vector3 position = other.ClosestPoint(transform.position);
            Instantiate(impactEffect, position, Quaternion.identity);

            //�U������
            isVibrating = true;
            vibrationTimer = 0f;

            // �_���[�W����
            TakeDamageProjectile(Damage);

            // ��ѓ�����폜
            Destroy(other.gameObject);
        }
    }

    public void TakeDamageProjectile((int, string, AttackLevel) damage)
    {
        if (SceneManager.GetActiveScene().name == "SoloGameScene")
        {
            cardamage.ApplyProjectileDamage(damage.Item1);
            score.AddProjectileScore(damage.Item1);
        }
    }

    private void Update()
    {
        //�U��������
        if (isVibrating)
        {
            if(vibrationTimer < vibrationDuration)
            {
                vibrationTimer += Time.deltaTime;
                noiseShaker.StartNoise(vibrationTimer);
            }
            else
            {
                isVibrating = false;
            }
        }
        
        // �t���[���J�E���g���J�n����Ă���ꍇ
        if (isCountingFrames)
        {
            // �t���[���J�E���g���C���N�������g
            frameCount++;

            // 10�t���[���ɓ��B������
            if (frameCount >= 10)
            {
                // Animator���ĊJ
                playerAnim.enabled = true;

                // �t���[���J�E���g�����Z�b�g
                frameCount = 0;

                // �t���[���J�E���g���~
                isCountingFrames = false;
            }
        }
    }
}
