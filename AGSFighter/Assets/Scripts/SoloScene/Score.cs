using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // UI�v�f
    [SerializeField]
    private Text score; // ���݂̃X�R�A��\������e�L�X�g
    [SerializeField]
    private Text highScore; // �n�C�X�R�A��\������e�L�X�g
    [SerializeField]
    private Text timeScoreText; // �^�C���X�R�A��\������e�L�X�g
    [SerializeField]
    private Text perfectScoreText; // �p�[�t�F�N�g�X�R�A��\������e�L�X�g
    [SerializeField]
    private Text totalScoreText; // �g�[�^���X�R�A��\������e�L�X�g
    [SerializeField]
    private Text scoreText; // �ŏI�X�R�A��\������e�L�X�g

    // �X�R�A�֘A�̕ϐ�
    private int scoreIndex; // ���݂̃X�R�A�C���f�b�N�X
    private int scoreCurrentIndex; // ���݂̃X�R�A
    private int timeScore; // �^�C���X�R�A
    private int carBreakScore; // �Ԃ̔j��X�R�A
    private int totalScore; // �g�[�^���X�R�A
    private int finishScore; // �ŏI�X�R�A

    // ���̃R���|�[�l���g
    [SerializeField]
    private AttackManager attackManager; // �U���}�l�[�W���[
    [SerializeField]
    private SoloModeCountDown countDown; // �J�E���g�_�E��
    [SerializeField]
    private UIAnimation uiAnim; // UI�A�j���[�V����

    // ����������
    void Start()
    {
        InitializeHighScore();
    }

    // �n�C�X�R�A�̏�����
    private void InitializeHighScore()
    {
        // �X�R�A���Z�b�g
        // PlayerPrefs.SetInt("HighScore", 0);
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    // ���t���[���Ăяo�����X�V����
    void Update()
    {
        CheckCountDown();
    }

    // �J�E���g�_�E���̃`�F�b�N
    private void CheckCountDown()
    {
        if (countDown.GetTime() < 0)
        {
            uiAnim.SceneAnimFalse();
        }
    }

    // �g���K�[�ɏՓ˂����Ƃ��̏���
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            ColAnimationEvent attackerEvent = GetAttackerEvent(other.transform);
            if (attackerEvent != null)
            {
                AddScore(attackerEvent);
            }
        }
    }

    // �U���҂̃C�x���g���擾
    private ColAnimationEvent GetAttackerEvent(Transform currentTransform)
    {
        while (currentTransform != null)
        {
            ColAnimationEvent attackerEvent = currentTransform.GetComponent<ColAnimationEvent>();
            if (attackerEvent != null)
            {
                return attackerEvent;
            }
            currentTransform = currentTransform.parent;
        }
        return null;
    }

    // �X�R�A��ǉ�
    public void AddScore(ColAnimationEvent attacker)
    {
        if (attacker.anim != null)
        {
            foreach (string key in attackManager.attackInfo.Keys)
            {
                if (attacker.anim.GetBool(key))
                {
                    scoreIndex = attackManager.GetAttackDetails(key).damage;
                    scoreCurrentIndex += scoreIndex;
                }
            }
            score.text = scoreCurrentIndex.ToString();
        }
    }

    // �v���W�F�N�^�C���̃X�R�A��ǉ�
    public void AddProjectileScore(int damage)
    {
        scoreIndex = damage;
        scoreCurrentIndex += scoreIndex;
        score.text = scoreCurrentIndex.ToString();
    }

    // �g�[�^���X�R�A���v�Z
    public void TotalScore(int carBreakScore)
    {
        timeScore = countDown.GetTime() * 1000;
        this.carBreakScore = carBreakScore;
        totalScore = timeScore + this.carBreakScore;
        UpdateScoreTexts();
        finishScore = totalScore + scoreCurrentIndex + scoreIndex;
        scoreText.text = finishScore.ToString();

        UpdateHighScore();
    }

    // �X�R�A�e�L�X�g���X�V
    private void UpdateScoreTexts()
    {
        timeScoreText.text = timeScore.ToString();
        perfectScoreText.text = carBreakScore.ToString();
        totalScoreText.text = totalScore.ToString();
    }

    // �n�C�X�R�A���X�V
    private void UpdateHighScore()
    {
        if (PlayerPrefs.GetInt("HighScore") < finishScore)
        {
            PlayerPrefs.SetInt("HighScore", finishScore);
        }
    }
}
