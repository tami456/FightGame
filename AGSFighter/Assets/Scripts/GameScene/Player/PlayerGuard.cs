using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGuard : MonoBehaviour
{
    [SerializeField]
    private ColAnimationEvent enemyColAnim; // �G�̍U���A�j���[�V�����C�x���g
    [SerializeField]
    private ColAnimationEvent colAnimationEvent; // �����̍U���A�j���[�V�����C�x���g

    private bool isGuard = false; // �K�[�h��Ԃ������t���O
    private float guard = 0.0f; // �K�[�h�̎�������

    [SerializeField]
    private GameObject guardEffect; // �K�[�h�G�t�F�N�g�̃v���n�u
    private bool canInstantiateEffect = true; // �G�t�F�N�g�����̐���t���O

    private Collider col; // �Փ˂����R���C�_�[

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

    /// <summary>
    /// �U�����I��������K�[�h������
    /// </summary>
    /// <param name="get_flag"></param>
    private void HandleAttackEnded(bool get_flag)
    {
        if (!get_flag)
        {
            isGuard = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack") || other.CompareTag("Projectile"))
        {
            col = other; // �Փ˂����R���C�_�[��ۑ�
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

            if (attackerEvent != null)
            {
                // �������g�̍U���Ȃ疳������
                if (attackerEvent == colAnimationEvent)
                {
                    Debug.Log("�������g�̍U���Ȃ̂Ŗ������܂��B");
                    return;
                }
            }

            if (attackerEvent != null && attackerEvent == enemyColAnim)
            {
                isGuard = true;
            }
        }
        else if (other.tag == "Projectile")
        {
            isGuard = true;
        }
    }

    /// <summary>
    /// �K�[�h�G�t�F�N�g�𐶐�
    /// </summary>
    public void InstantiateGuardEffect()
    {
        if (canInstantiateEffect)
        {
            Vector3 position = col.ClosestPoint(transform.position); // �Փ˂����ꏊ�̋߂��̓_���擾
            Instantiate(guardEffect, position, Quaternion.identity); // �K�[�h�G�t�F�N�g�𐶐�
            canInstantiateEffect = false; // �G�t�F�N�g�������ꎞ�I�ɖ����ɂ���
            StartCoroutine(ResetEffectInstantiation()); // ��莞�Ԍ�ɃG�t�F�N�g�������ēx�L���ɂ���
        }
    }

    /// <summary>
    /// �G�t�F�N�g�������ēx�L���ɂ���
    /// </summary>
    private IEnumerator ResetEffectInstantiation()
    {
        yield return new WaitForSeconds(0.1f);
        canInstantiateEffect = true; 
    }

    /// <summary>
    /// �K�[�h��Ԃ���������
    /// </summary>
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
