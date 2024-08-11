using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

[DisplayName("Disallow One Modifier")]
public class DisallowOneModifierComposite : InputBindingComposite<float>
{
    // ���̃L�[��������Ă��Ȃ����button��Action�����s����
    [InputControl(layout = "Button")] public int modifier;

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
        InputSystem.RegisterBindingComposite(typeof(DisallowOneModifierComposite), "DisallowOneModifierComposite");
    }

    /// <summary>
    /// ����̃{�^����������Ă��Ȃ��������l��Ԃ�
    /// </summary>
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        // modifier�̃{�^����������Ă��Ȃ�������button�̓��͂�ʂ�
        if (!context.ReadValueAsButton(modifier))
            return context.ReadValue<float>(button);

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
