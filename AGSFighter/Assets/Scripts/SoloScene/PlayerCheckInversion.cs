using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckInversion : MonoBehaviour
{
    private PlayerAction player;
    //�ڒn����
    //Ray�̒���
    [SerializeField]
    private float rayLength = 0.1f;

    //Ray���ǂꂭ�炢�̂ɂ߂荞�܂��邩
    [SerializeField]
    private float rayOffset;

    //Ray�̔���ɗp����Layer
    [SerializeField]
    private LayerMask layerMask = default;
    [SerializeField]
    private bool isInversion = false;
    Vector3 vec = Vector3.right;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void Update()
    {
        //���C���t�����ɔ�΂�
        if(!isInversion)
        {
            vec *= -1;
        }
    }

    private void FixedUpdate()
    {
        //�~���[�p�̃I�u�W�F�N�g�ɒ����Ă��邩
        isInversion = CheckInversion();
    }

    public bool GetIsInversion()
    {
        return isInversion;
    }

    private bool CheckInversion()
    {
        if(player.GetIsGround())
        {
            //�������̏����ʒu�Ǝp��
            //�኱�g�̂ɂ߂荞�܂����ꂩ�甭�˂��Ȃ��Ɛ���������ł��Ȃ���������
            ray = new Ray(origin: transform.position + Vector3.up * rayOffset, direction: vec);
        }
        //Raycast��hit���邩�ǂ����Ŕ���
        //���C���̎w���Y�ꂸ��
        return Physics.Raycast(ray, rayLength, layerMask);
    }

    private void OnDrawGizmos()
    {
        //��{�I�Ƀ��C���������Ă����ԂȂ̂Ŋm�F���Ȃ��Ă�����
        Gizmos.color = isInversion ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, vec);
    }
}
