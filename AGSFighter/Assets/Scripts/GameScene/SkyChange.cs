using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyChange : MonoBehaviour
{
    [SerializeField] private Material[] sky; // �X�J�C�{�b�N�X�̃}�e���A���z��
    [SerializeField] private Light directionalLight; // �Ƃ炷���C�g
    [SerializeField] private Vector3[] lightRotations; // ���C�g�̕���
    private int num;

    void Start()
    {
        // �ۑ�����Ă���J�E���g���擾
        num = PlayerPrefs.GetInt("DayNightCycle", 0);

        // �V�[�����[�h���ɒ����ݒ肷��
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetSkyboxAndLight();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SelectStage2")
        {
            SetSkyboxAndLight();
        }
    }

    void SetSkyboxAndLight()
    {
        // �J�E���g���C���N�������g���A�X�J�C�{�b�N�X�ƃ��C�g�̐ݒ�
        num = (num + 1) % sky.Length;
        PlayerPrefs.SetInt("DayNightCycle", num);
       
        RenderSettings.skybox = sky[num];

        directionalLight.transform.rotation = Quaternion.Euler(lightRotations[num]);

        // ���C�g�̉�]�p�x�ƌ��݂̃X�J�C�{�b�N�X�̔ԍ������O�ɏo��
        Debug.Log("Current Skybox Index: " + num);
        Debug.Log("Current Light Rotation: " + directionalLight.transform.rotation.eulerAngles);

        // PlayerPrefs�̕ύX��ۑ�
        PlayerPrefs.Save();
    }
}
