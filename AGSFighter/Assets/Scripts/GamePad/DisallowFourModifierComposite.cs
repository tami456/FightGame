using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

[DisplayName("Disallow Four Modifier")]
public class DisallowFourModifierComposite : InputBindingComposite<float>
{
    // ���̃L�[��������Ă��Ȃ����button��Action�����s����
    [InputControl(layout = "Button")] public int modifier;
    [InputControl(layout = "Button")] public int modifier2;
    [InputControl(layout = "Button")] public int modifier3;
    [InputControl(layout = "Button")] public int modifier4;

    // �r������Ώۂ̃{�^��
    [InputControl(layout = "Button")] public int button;

    /// <summary>
    /// ������
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
    private static void Initialize()
    {
        // �����CompositeBinding��o�^����K�v������
        InputSystem.RegisterBindingComposite(typeof(DisallowFourModifierComposite), "DisallowFourModifierComposite");
    }

    /// <summary>
    /// ����̃{�^����������Ă��Ȃ��������l��Ԃ�
    /// </summary>
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        // modifier�̃{�^����������Ă��Ȃ�������button�̓��͂�ʂ�
        if (!context.ReadValueAsButton(modifier) && !context.ReadValueAsButton(modifier2)
            && !context.ReadValueAsButton(modifier3) && !context.ReadValueAsButton(modifier4))
        {
            return context.ReadValue<float>(button);
        }

        return default;
    }

    /// <summary>
    /// ���͒l�̑傫�����擾����
    /// modifier���͂̉�������iPress Point�Ƃ�臒l����j�̂��߂Ɏ����K�{
    /// </summary>
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context);
    }
}
