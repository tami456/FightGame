using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinStarsUI : MonoBehaviour
{
    public List<Image> winStars; // ��������\������Image�R���|�[�l���g�̃��X�g
    public Sprite defaultSprite; // �����̉摜
    public Sprite winSprite; // ���������l�����ꂽ�Ƃ��̉摜

    private void Start()
    {
        ResetWinStars();
    }

    // ���������X�V���郁�\�b�h
    public void UpdateWinStars(int wins)
    {
        for (int i = 0; i < winStars.Count; i++)
        {
            if (i < wins)
            {
                winStars[i].sprite = winSprite;
            }
            else
            {
                winStars[i].sprite = defaultSprite;
            }
        }
    }

    // ��������������ԂɃ��Z�b�g���郁�\�b�h
    public void ResetWinStars()
    {
        foreach (var star in winStars)
        {
            star.sprite = defaultSprite;
        }
    }

}
