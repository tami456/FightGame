using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour
{

    [SerializeField]
    PlayerAction player;
    //�j���臒l
    private int destructionThreshold = 1000;
    // �~�ς��ꂽ�_���[�W
    private int accumulatedDamage = 0;
    private int damage = 0;

    //���ł�����
    [SerializeField]
    private float flyingForce = 0.0f;

    //���ł����Ԃ̃p�[�c
    [SerializeField]
    private GameObject[] carParts;
    private int partsIndex = 0;

    [SerializeField]
    AttackManager attackManager;

    [SerializeField]
    SoloModeCountDown countDown;
    Score score;
    private int carBreakScore = 0;

    //UI�̃A�j���[�V����
    [SerializeField]
    UIAnimation uiAnim;

    //SE�ABGM
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip se1;
    [SerializeField]
    AudioClip se2;

    void Start()
    {
        score = GetComponent<Score>();
    }

    private void Update()
    {
        
    }

    //�_���[�W��^����
    public void ApplyDamage(ColAnimationEvent attacker)
    {
        //�U�����Ă�������̃A�j���[�V�������擾
        if (attacker.anim != null)
        {
            foreach (string key in
                attackManager.attackInfo.Keys)
            {
                if (attacker.anim.GetBool(key))
                {
                    damage = attackManager.GetAttackDetails(key).damage;
                }
            }
        }

        //�ԉ���
        audioSource.PlayOneShot(se1);

        //�_���[�W�~��
        accumulatedDamage += damage;

        //���ʒ������烊�Z�b�g
        if (accumulatedDamage >= destructionThreshold)
        {
            ShatterScatterPart();
            accumulatedDamage = accumulatedDamage - destructionThreshold;
            Debug.Log(accumulatedDamage);
        }
    }

    public void ApplyProjectileDamage(int damage)
    {
        //�ԉ���
        audioSource.PlayOneShot(se1);

        //�_���[�W�~��
        accumulatedDamage += damage;

        //���ʒ������烊�Z�b�g
        if (accumulatedDamage >= destructionThreshold)
        {
            ShatterScatterPart();
            accumulatedDamage = accumulatedDamage - destructionThreshold;
            Debug.Log(accumulatedDamage);
        }
    }

    //�Ԃ̃p�[�c���΂�
    void ShatterScatterPart()
    {
        //�Ԕj��
        audioSource.PlayOneShot(se2);

        if(carParts[partsIndex] == null)
        {
            Debug.Log("�����p�[�c�͂���܂���");
            carBreakScore = 30000;
            player.State = PlayerAction.MyState.Freeze;
            countDown.StartStopTime(true);
            score.TotalScore(carBreakScore);
            uiAnim.SceneAnimFalse();
            return;
        }
        if (carParts.Length > 0)
        {
            GameObject randomPart = carParts[partsIndex];

            Rigidbody partRb = randomPart.GetComponent<Rigidbody>();
            if (partRb != null)
            {
                partRb.useGravity = true;
                partRb.isKinematic = false;
                partRb.AddForce(Random.onUnitSphere * flyingForce, ForceMode.Impulse); // �����_���ȕ����ɗ͂�������
            }
            carBreakScore = 0;
            partsIndex++;
        }
    }

    public int GetCarScore()
    {
        return carBreakScore;
    }
}
