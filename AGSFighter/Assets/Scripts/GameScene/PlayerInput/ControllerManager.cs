using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class ControllerManager : SingletonBehaviour<ControllerManager>
{
    // �R���g���[�����X�g: �ǉ����ɓo�^
    private List<GameObject> m_controllers = new List<GameObject>();

    // �R���g���[�������m�F
    public bool IsController(int controller_id)
    {
        if (controller_id >= m_controllers.Count) { return false; }
        return true;
    }

    // �R���g���[�����擾
    public ControllerBehaviour GetController(int controller_id)
    {
        return m_controllers[controller_id].GetComponent<ControllerBehaviour>();
    }

    // �R���g���[���ǉ�����
    public void OnPlayerJoined(PlayerInput player_input)
    {
        var controller_obj = player_input.gameObject;

        controller_obj.transform.parent = this.transform;
        m_controllers.Add(controller_obj);
    }
}