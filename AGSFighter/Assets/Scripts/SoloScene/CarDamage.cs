using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour
{
    // �v���C���[�̃A�N�V�������Ǘ�����N���X
    [SerializeField]
    private PlayerAction player;

    // �j���臒l
    private int destructionThreshold = 1000;

    // �~�ς��ꂽ�_���[�W
    private int accumulatedDamage = 0;
    private int damage = 0;

    // ���ł�����
    [SerializeField]
    private float flyingForce = 0.0f;

    // ���ł����Ԃ̃p�[�c
    [SerializeField]
    private GameObject[] carParts;
    private int partsIndex = 0;

    // �U���}�l�[�W���[
    [SerializeField]
    private AttackManager attackManager;

    // �J�E���g�_�E��
    [SerializeField]
    private SoloModeCountDown countDown;

    // �X�R�A
    private Score score;
    private int carBreakScore = 0;

    // UI�̃A�j���[�V����
    [SerializeField]
    private UIAnimation uiAnim;

    // SE�ABGM
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip se1;
    [SerializeField]
    private AudioClip se2;

    // ����������
    void Start()
    {
        score = GetComponent<Score>();
    }

    // �_���[�W��^���鏈��
    public void ApplyDamage(ColAnimationEvent attacker)
    {
        if (attacker.anim != null)
        {
            foreach (string key in attackManager.attackInfo.Keys)
            {
                if (attacker.anim.GetBool(key))
                {
                    damage = attackManager.GetAttackDetails(key).damage;
                }
            }
        }

        PlayDamageSound();
        AccumulateDamage(damage);
    }

    // �v���W�F�N�^�C���̃_���[�W��^���鏈��
    public void ApplyProjectileDamage(int damage)
    {
        PlayDamageSound();
        AccumulateDamage(damage);
    }

    // �_���[�W�����Đ����鏈��
    private void PlayDamageSound()
    {
        audioSource.PlayOneShot(se1);
    }

    // �_���[�W��~�ς��鏈��
    private void AccumulateDamage(int damage)
    {
        accumulatedDamage += damage;

        if (accumulatedDamage >= destructionThreshold)
        {
            ShatterScatterPart();
            accumulatedDamage -= destructionThreshold;
            Debug.Log(accumulatedDamage);
        }
    }

    // �Ԃ̃p�[�c���΂�����
    private void ShatterScatterPart()
    {
        audioSource.PlayOneShot(se2);

        if (carParts[partsIndex] == null)
        {
            HandleAllPartsDestroyed();
            return;
        }

        if (carParts.Length > 0)
        {
            LaunchCarPart();
            carBreakScore = 0;
            partsIndex++;
        }
    }

    // �S�Ẵp�[�c���j�󂳂ꂽ�Ƃ��̏���
    private void HandleAllPartsDestroyed()
    {
        Debug.Log("�����p�[�c�͂���܂���");
        carBreakScore = 30000;
        player.State = PlayerAction.MyState.Freeze;
        countDown.StartStopTime(true);
        score.TotalScore(carBreakScore);
        uiAnim.SceneAnimFalse();
    }

    // �Ԃ̃p�[�c���΂�����
    private void LaunchCarPart()
    {
        GameObject randomPart = carParts[partsIndex];
        Rigidbody partRb = randomPart.GetComponent<Rigidbody>();

        if (partRb != null)
        {
            partRb.useGravity = true;
            partRb.isKinematic = false;
            partRb.AddForce(Random.onUnitSphere * flyingForce, ForceMode.Impulse); // �����_���ȕ����ɗ͂�������
        }
    }

    // �Ԃ̃X�R�A���擾���鏈��
    public int GetCarScore()
    {
        return carBreakScore;
    }
}
