using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hadoken : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.0f;
    float moveDistance;

    // �����������ϐ�
    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private string hitPlayer;

    private void Start()
    {
    }

    // ���������\�b�h
    public void Initialize(Vector3 direction,string hitPlayer)
    {
        this.direction = direction; // �����𐳋K�����邱�ƂŁA���x��ێ�����
        this.hitPlayer = hitPlayer;
        Debug.LogWarning("������ׂ������" + this.hitPlayer);
    }

    private void FixedUpdate()
    {
        moveDistance = speed * Time.deltaTime;
        HadokenMove();
    }

    public void HadokenMove()
    {
        // �����ɉ����Ĉړ�
        transform.Translate(direction * moveDistance);
    }

    private  void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Attack" || other.tag == "Car" || other.tag == "Edge" || other.tag == hitPlayer)
        {
            Debug.Log("���炠���ŉΏ�"+other.name);

            Destroy(this.gameObject);
        }
    }
}
