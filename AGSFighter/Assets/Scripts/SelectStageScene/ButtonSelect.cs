using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public GameObject[] image;
    public GameObject[] searchImage;

    private Image currentImage;
    private bool[] buttonHovered;

    void Start()
    {
        currentImage = GetComponent<Image>();
        buttonHovered = new bool[searchImage.Length];
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // ���̉摜�̍��W���Ƃɏ�̉摜��؂�ւ���
        for (int i = 0; i < searchImage.Length; i++)
        {
            if (searchImage[i] != null)
            {
                // ���̉摜��RectTransform���擾
                RectTransform rectTransform = searchImage[i].GetComponent<RectTransform>();

                // �}�E�X�J�[�\�����{�^���̏�ɂ��邩�ǂ������m�F
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
                {
                    if (!buttonHovered[i])
                    {
                        buttonHovered[i] = true;
                        SoundManager.Instance.PlayUIClip("�J�[�\���ړ�5");
                    }
                    SwitchTopImage(i);
                }
                else
                {
                    buttonHovered[i] = false;
                }
            }
        }
    }

    // ��̉摜��؂�ւ��郁�\�b�h
    void SwitchTopImage(int index)
    {
        for (int i = 0; i < image.Length; i++)
        {
            if (image[i] != null)
            {
                // ���ׂĂ̏�̉摜���\���ɂ���
                image[i].SetActive(false);
            }
        }
        // �Ή������̉摜��\������
        if (index >= 0 && index < image.Length && image[index] != null)
        {
            image[index].SetActive(true);
        }
    }
}
