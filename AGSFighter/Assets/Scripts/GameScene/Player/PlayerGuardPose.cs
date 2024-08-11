using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGuardPose : MonoBehaviour
{
    [SerializeField]
    private ColAnimationEvent enemyColAnim; //�G�̍U���A�j���[�V�����C�x���g
    private bool isGuard = false; // �K�[�h��Ԃ������t���O
    private float guard = 0.0f; // �K�[�h�̎�������

    private void Update()
    {
        //�t���[�����[�g�Ɉˑ����Ȃ��悤��Time.deltaTime���g�p���ăK�[�h���Ԃ𑝉�
        if (isGuard)
        {
            guard += Time.deltaTime; 
        }
        //�K�[�h���Ԃ����𒴂����烊�Z�b�g
        if (guard >= 50.0f)
        {
            ResetGuard(); 
        }

        //�\�����[�h�ł̓K�[�h�͎g��Ȃ�
        if (SceneManager.GetActiveScene().name != "SoloGameScene")
        {
            enemyColAnim.OnAttackEnded += HandleAttackEnded; // �U���I���C�x���g�̃n���h����ݒ�
            enemyColAnim.OnAttackEnded += get_flag =>
            {
                if (get_flag)
                {

                }
                else
                {
                    isGuard = false;
                }
            };
        }
    }

    // �U�����I��������K�[�h������
    private void HandleAttackEnded(bool get_flag)
    {
        if (!get_flag)
        {
            isGuard = false; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Attack")
        {
            // ��ʊK�w��k����ColAnimationEvent��T��
            Transform currentTransform = other.transform;
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

            if (attackerEvent != null && attackerEvent == enemyColAnim)
            {
                isGuard = true;
            }
        }
        else if (other.tag == "Projectile")
        {
            IsGuard = false;
        }
    }

    //�K�[�h��Ԃ���������
    private void ResetGuard()
    {
        guard = 0.0f;
        isGuard = false;
    }

    public bool IsGuard
    {
        get { return isGuard; }
        set { isGuard = value; }
    }
}
