using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoloModeCountDown : MonoBehaviour
{
    [SerializeField]
    private bool isCount = false;
    private int remainingTime;
    [SerializeField]
    private float countDownTime; // �J�E���g�_�E���^�C��
    [SerializeField]
    private Text textCountDown; // �\���p�e�L�X�gUI
    [SerializeField]
    private PlayerAction player;
    [SerializeField]
    Score score;
    [SerializeField]
    CarDamage carDamage;
    UIAnimation uiAnim;

    private void Start()
    {
        uiAnim = GetComponent<UIAnimation>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isCount)
        {
            // �J�E���g�_�E���^�C���𐮌`���ĕ\��
            textCountDown.text = string.Format("{0:00}", countDownTime);
            if (countDownTime > 0)
            {
                // �o�ߎ����������Ă���
                countDownTime -= Time.deltaTime;
                remainingTime = Mathf.RoundToInt(countDownTime);
            }
            else
            {
                player.State = PlayerAction.MyState.Freeze;
                uiAnim.SceneAnimFalse();
                score.TotalScore(carDamage.GetCarScore());
                SoundManager.Instance.StopBGM();
                Debug.Log("Finish!!");
            }
        }
    }
    public int GetTime()
    {
        return remainingTime;
    }

    public void StartStopTime(bool isCount)
    {
        this.isCount = isCount;
    }
}
