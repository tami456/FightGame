using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundScript : MonoBehaviour
{
    void Update()
    {
        // Y���𒆐S�ɂP�b�ɂ��|12�x��]����
        transform.Rotate(new Vector3(0, -5) * Time.deltaTime);
    }
}
