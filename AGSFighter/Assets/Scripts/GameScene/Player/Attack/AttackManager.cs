using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
public Dictionary<string, (int damage, string sound, 
    AttackLevel attackLevel,AttackType attackType)> attackInfo = 
        new Dictionary<string, (int, string, AttackLevel,AttackType)>
    {
        ///{"�U���A�j���[�V������bool","SE�̃t�@�C����",
        ///"�㒆���i�̎���"}
        {"N_Weak",(300, "�y���p���`1",AttackLevel.High,AttackType.Weak)},
        { "C_Weak", (200, "�y���L�b�N1",AttackLevel.Low,AttackType.Weak) },
        //{ "NA_Weak", 350 },
        { "N_Mid", (600,"�y���p���`2",AttackLevel.High,AttackType.Middle)},
        { "C_Mid", (500,"�y���L�b�N1",AttackLevel.Low,AttackType.Middle) },
        //{ "NA_Mid", 600 },
        { "N_Stro", (800,"�d���p���`2",AttackLevel.High,AttackType.Strong) },
        { "Ashibarai", (900,"�y���L�b�N1",AttackLevel.Low,AttackType.Huttobi) },
        { "JumpKick", (300,"�y���L�b�N1",AttackLevel.Mid,AttackType.Weak) },
        { "JumpKick2", (500,"�y���L�b�N1",AttackLevel.Mid,AttackType.Middle) },
        { "JumpPunch", (800,"�y���L�b�N1",AttackLevel.Mid,AttackType.Strong) },
        { "Sakotsuwari", (600,"�d���p���`2",
                AttackLevel.Mid,AttackType.Middle) },
        //{ "NA_Stro", 900 },
        { "Syoryuken", (1400,"������",
                AttackLevel.High,AttackType.Huttobi) },
        { "Tatsumaki", (900,"�y���L�b�N1",
            AttackLevel.High,AttackType.Weak) },
        { "Throw", (1200,"�y���L�b�N1",
            AttackLevel.High,AttackType.Strong) },
        { "Hasyogeki", (800,"�g�Ռ�",
            AttackLevel.High,AttackType.Strong) },
    };


    // �U���̎�ނ�ݒ肵�A���̃_���[�W��Ԃ�
    public (int damage, string sound, AttackLevel attackLevel,AttackType attackType) GetAttackDetails(string attackType)
    {
        if (attackType != null && attackInfo.ContainsKey(attackType))
        {
            return attackInfo[attackType];
        }
        else
        {
            if (attackType != null)
            {
                Debug.LogWarning($"�U���^�C�v {attackType} �ɑΉ�����ڍׂ�������܂���ł����B");
            }
            else
            {
                Debug.LogWarning("�U���^�C�v��null�ł��B");
            }

            return (0, null, AttackLevel.NullLevel, AttackType.NullLevel);
        }
    }
}
