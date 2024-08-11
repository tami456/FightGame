using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class PlayerHit : MonoBehaviour
{
    private int test = 0;
    private AttackManager attackManager;

    [SerializeField]
    private PlayerAction player;
    //�q�b�g�X�g�b�v�ϐ�
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private int frameCount;
    private bool isCountingFrames;

    [SerializeField]
    private PlayerInputSystem inputSystem;
    [SerializeField] public HPGauge hpGauge; // ����v���C���[��HP���A�^�b�`����
    [SerializeField] private ColAnimationEvent colAnimationEvent; // ������ColAnimationEvent���A�^�b�`����
    [SerializeField]
    private GameObject camEdge, camEdge2;
    Collider attackCol;
    [SerializeField] private GameObject initializePosition;

        [SerializeField]
    GameObject impactEffect;

    // ProjectileDamageDictionary�ւ̎Q��
    [SerializeField] private ProjectileDamageDictionary projectileDamageDictionary;
    (int, string, AttackLevel, AttackType) damages = (0, "", AttackLevel.NullLevel, AttackType.NullLevel); // �����l�Ƃ��ă_���[�W�A�����̖��O�A�U�����x���̃^�v����ݒ肵�܂�

    public (int, string, AttackLevel,AttackType) Damage
    {
        get { return this.damages; }  // �擾�p
        private set { this.damages = value; } // �l���͗p
    }

    // �N�[���_�E�����ԁi�b�j
    public float hitCooldown = 1.0f;
    public float hitDuration = 0.5f;
    private bool canBeHit = true;

    private bool hasHit = false; // ���łɓ����蔻�肪�������ꂽ���ǂ������Ǘ�����t���O

    [SerializeField] private bool isDefeated = false; // �v���C���[���|���ꂽ���ǂ����������t���O



    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "TutorialScene"
            && SceneManager.GetActiveScene().name != "SoloGameScene")
        {
            RoundManager.Instance.Create();

            attackManager = FindObjectOfType<AttackManager>(); // AttackManager��������
            if (attackManager == null)
            {
                Debug.LogError("AttackManager��������܂���ł����B");
            }

            if (colAnimationEvent == null)
            {
                Debug.LogError("ColAnimationEvent��������܂���ł����B");
            }
        }
    }

    private void Update()
    {
        // �t���[���J�E���g���J�n����Ă���ꍇ
        if (isCountingFrames)
        {
            // �t���[���J�E���g���C���N�������g
            frameCount++;

            // 10�t���[���ɓ��B������
            if (frameCount >= 10)
            {
                // Animator���ĊJ
                playerAnimator.enabled = true;

                // �t���[���J�E���g�����Z�b�g
                frameCount = 0;

                // �t���[���J�E���g���~
                isCountingFrames = false;
            }
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (isDefeated) return; // �ǉ�: �|���ꂽ�ꍇ�͓����蔻��𖳎�

        //���������������ăK�[�h�o���Ă��Ă��_���[�W��H����Ă��܂�
        // Attack�^�O�̃I�u�W�F�N�g�ɂ̂ݔ���
        if (other.CompareTag("Attack") && canBeHit && !hasHit)
        {
            await UniTask.Delay(50);

            if (!player.playerInput.guardSFlag && !player.playerInput.guardCFlag)
            {
                NormalAttack(other);
                hasHit = true;

                // 1�t���[�����hasHit��false�ɂ���
                StartCoroutine(ResetHasHit());
            }
        }
        else if (other.CompareTag("Projectile") && canBeHit && !hasHit)
        {
            //�������U��
            if (!player.playerInput.guardSFlag && !player.playerInput.guardCFlag)
            {
                if(other!=null)
                {
                    ProjectileAttack(other);
                    
                    hasHit = true;
                    // 1�t���[�����hasHit��false�ɂ���
                    StartCoroutine(ResetHasHit());
                }
                else
                {
                    Debug.LogError("�����Ă˂����E����");
                }
            }
        }
    }

    private IEnumerator ResetHasHit()
    {
        yield return null; // 1�t���[���ҋ@

        hasHit = false;
    }

    //Damage�̏�����āA������������
    private bool AttackLevelFlag()
    {
        // ���ʉ��E�A�j���[�V�������Đ�
        Dictionary<AttackLevel, (int, bool)> attackLevelToTestValue = new Dictionary<AttackLevel, (int, bool)>()
    {
        { AttackLevel.High, player.IsCrouching ? (21, false) : (11, true) },
        { AttackLevel.Mid, player.IsCrouching ? (22, true) : (12, true) },
        { AttackLevel.Low, player.IsCrouching ? (23, true) : (13, true) }
    };

        var (testValue, shouldApplyDamage) = attackLevelToTestValue[Damage.Item3];
        test = testValue;

        if ((Damage.Item3 == AttackLevel.Low || Damage.Item3 == AttackLevel.Mid || Damage.Item3 == AttackLevel.High))
        {
            player.animState.ResetDamageAnim();
            if(Damage.Item4 == AttackType.Strong)
            {
                ExecuteHit("SStrongHit");
            }
            else if(Damage.Item4 == AttackType.Middle)
            {
                ExecuteHit("SMidHit");
            }
            else if(Damage.Item4 == AttackType.Weak)
            {
                ExecuteHit("SWeakHit");
            }
            else if(Damage.Item4 == AttackType.Huttobi)
            {
                ExecuteHit("Huttobi");
            }
        }
        return shouldApplyDamage;
    }
    private void ExecuteHit(string attackName)
    {
        if (player.anim.GetBool(attackName))
        {
            player.animState.SetAnimFalse(attackName);
            StartCoroutine(DelayAnim(attackName));
            return;
        }

        player.animState.ResetDamageAnim();

        player.animState.SetAnimTrue(attackName);
        player.playerInput.AttackFlag = true;
    }
    IEnumerator DelayAnim(string attackName)
    {
        yield return new WaitForSeconds(0.001f);

        player.animState.SetAnimTrue(attackName);
        player.playerInput.AttackFlag = true;
    }

    private void NormalAttack(Collider other)
    {
        //Debug.Log("Attack�^�O�̃I�u�W�F�N�g�����o����܂���: " + other.gameObject.name);

        // ��ʊK�w��k����ColAnimationEvent��T��
        Transform currentTransform = other.transform;
        ColAnimationEvent attackerEvent = null;

        attackCol = other;
        while (currentTransform != null)
        {
            attackerEvent = currentTransform.GetComponent<ColAnimationEvent>();
            if (attackerEvent != null)
            {
                break;
            }
            currentTransform = currentTransform.parent;
        }

        if (attackerEvent != null)//�ǂ����������g��������������null�ɂȂ�
        {
            Debug.Log("ColAnimationEvent�R���|�[�l���g��������܂����B");
            // �������g�̍U���Ȃ疳������
            if (attackerEvent == colAnimationEvent)
            {
                Debug.Log("�������g�̍U���Ȃ̂Ŗ������܂��B");
                return;
            }
            Debug.Log("�U�����q�b�g���܂����B");
            // �U���҂̏����擾
            TakeDamage(attackerEvent);
        }
        else
        {
            //TakeDamage(colAnimationEvent);
            Debug.LogWarning("ColAnimationEvent�R���|�[�l���g��������܂���ł����B");
        }
    }

    private void ProjectileAttack(Collider other)
    {
        //Debug.Log("Projectile�^�O�̃I�u�W�F�N�g�����o����܂���: " + other.gameObject.name);
        string projectileName = other.gameObject.name.Replace("(Clone)", "").Trim();
        Debug.LogWarning("�������̃l�[��"+ projectileName);
        attackCol = other;
        if (projectileDamageDictionary.projectileInfo.TryGetValue(
            projectileName, out ProjectileDamageDictionary.ProjectileInfo damage))
        {
            Damage = (damage.damage, damage.soundEffect,damage.attackLevel, damage.attackType);
            // �Փ˂����ꏊ�ɃG�t�F�N�g�𐶐�
            Vector3 position = other.ClosestPoint(transform.position);
            Instantiate(impactEffect, position, Quaternion.identity);

            // �_���[�W����
            TakeDamageProjectile(Damage);

        }
        else
        {
            Debug.LogWarning("ProjectileDamageDictionary�ɑ��݂��Ȃ���ѓ���ł�: " + other.gameObject.name+"\n");
        }
    }

    // �_���[�W���󂯂�
    public void TakeDamage(ColAnimationEvent attacker)
    {
        //Debug.Log("TakeDamage���Ă΂�܂����B");

        if (attacker.anim != null)
        {
            foreach (string key in 
                attackManager.attackInfo.Keys)
            {
                if(attacker.anim.GetBool(key))
                { // AttackManager����^�v�����擾���AProjectileDamageDictionary.ProjectileInfo�ɕϊ�
                
                    Damage = attackManager.GetAttackDetails(key);
                    Debug.Log(Damage);
                    SoundManager.Instance.PlaySFX(Damage.Item2);
                }
            }
        }
        else
        {
            Debug.LogWarning("�U���A�j���[�^�[��������܂���ł����B");
            return;
        }

        //Damage�̏������
        if (!AttackLevelFlag()) return;

        // �Փ˂����ꏊ�ɃG�t�F�N�g�𐶐�
        Vector3 position = attackCol.ClosestPoint(transform.position); // �Փ˂����ꏊ�̋߂��̓_���擾
        Instantiate(impactEffect, position, Quaternion.identity);


        //HP��������
        if (hpGauge != null)
        {
            hpGauge.BeInjured(Damage.Item1);
        }
        else
        {
            Debug.LogWarning("opponentHpGauge��������܂���ł����B");
        }

        //�q�b�g�X�g�b�v
        // Animator���~
        playerAnimator.enabled = false;

        // �t���[���J�E���g�����Z�b�g
        frameCount = 0;

        // �t���[���J�E���g���J�n
        isCountingFrames = true;

        if (RoundManager.Instance.isRoundOver) return; // �ǉ�
        PlayerDead();

        
        Debug.Log("�����HP��" + Damage + "�������܂����B");
    }

    public void TakeDamageProjectile((int,string,AttackLevel,AttackType) damage)
    {
        Debug.Log("TakeDamageProjectile���Ă΂�܂����B");

        //projectileDamageDictionary[]
        Damage =  damage;

        if (hpGauge != null)
        {
            hpGauge.BeInjured(Damage.Item1);
            SoundManager.Instance.PlaySFX(Damage.Item2);
        }
        else
        {
            Debug.LogWarning("hpGauge��������܂���ł����B");
        }

        if (RoundManager.Instance.isRoundOver) return;

        PlayerDead();

        Debug.Log("�����HP��" + Damage + "�������܂����B");
    }



    private IEnumerator ResetDefeatedFlag()
    {
        yield return new WaitForSeconds(3f); // 3�b�̗P�\��u��
    }

    public void ResetPlayer()
    {
        ResetPlayerCoroutine();

    }

    private void PlayerDead()
    {
        if (hpGauge._currentHP <= 0 && !isDefeated)
        {
            Debug.LogWarning("���ɂ܂���");
            isDefeated = true;
            hpGauge._currentHP = 0;
            RoundManager.Instance.NotifyPlayerDefeated(gameObject);
            StartCoroutine(ResetDefeatedFlag());
        }
    }

    private void ResetPlayerCoroutine()
    {
        ResetCamera(false);
        player.SetDefaultPos();

        if (hpGauge != null)
        {
            hpGauge.ResetHP();

            isDefeated = false; // �t���O�����Z�b�g
        }
        // �v���C���[�̈ʒu�₻�̑��̃X�e�[�^�X�����Z�b�g���鏈����ǉ�
        StartCoroutine(Cam());
    }

    public void ResetCamera(bool flag)
    {
        camEdge.SetActive(flag);
        camEdge2.SetActive(flag);
    }

    public IEnumerator Cam()
    {
        yield return new WaitForSeconds(0.5f);
        ResetCamera(true);
    }
}
