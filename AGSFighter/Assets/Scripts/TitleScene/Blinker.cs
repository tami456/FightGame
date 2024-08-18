using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour
{
    // �_�ŃX�s�[�h
    [SerializeField]
    private float speed = 1.0f; // �_�ł̑����𒲐����邽�߂̕ϐ�
    private Text text; // Text�R���|�[�l���g��ێ�����ϐ�
    private float time; // ���Ԃ�ǐՂ��邽�߂̕ϐ�

    // Awake�̓X�N���v�g�C���X�^���X�����[�h���ꂽ�Ƃ��ɌĂяo�����
    void Awake()
    {
        // Text�R���|�[�l���g���擾���ăL���b�V������
        text = GetComponent<Text>();
    }

    // Update�͖��t���[���Ăяo�����
    void Update()
    {
        // �V�����F���v�Z���A�ύX������ΓK�p����
        Color newColor = GetAlphaColor(text.color);
        if (text.color != newColor)
        {
            text.color = newColor;
        }
    }

    // �����̓_�Ō��ʂ��v�Z����
    Color GetAlphaColor(Color color)
    {
        // ���Ԃ��X�V���APingPong�֐����g���ăA���t�@�l���v�Z����
        time += Time.deltaTime * speed;
        color.a = Mathf.PingPong(time, 1.0f);

        return color;
    }
}
