using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private CanvasGroup fadeCanvasGroup;
    private GameObject fade;
    public float fadeDuration = 1.0f;

    public enum GameMode
    {
        Solo,
        VS
    }

    public GameMode SelectedGameMode { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fade = GameObject.FindGameObjectWithTag("Fade");
        fadeCanvasGroup = fade.GetComponent<CanvasGroup>();
        if (fadeCanvasGroup != null)
        {
            // �Q�[���J�n���Ƀt�F�[�h�C��
            FadeController.Instance.StartCoroutine(FadeController.Instance.FadeIn());
        }
    }

    // ���E���h�̊J�n����
    public void StartRound()
    {
        // ���E���h�J�n���̏����������ɋL�q
    }

    // ���E���h�I�����̏���
    public void EndRound()
    {
        // ���E���h�I�����Ƀt�F�[�h�A�E�g���A���̃��E���h�̏���
        StartCoroutine(EndRoundCoroutine());
    }

    private IEnumerator EndRoundCoroutine()
    {
        yield return StartCoroutine(FadeOut());
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        fadeCanvasGroup.gameObject.SetActive(true); // CanvasGroup��\��
        fadeCanvasGroup.alpha = 0;
        while (fadeCanvasGroup.alpha < 1.0f)
        {
            fadeCanvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        fadeCanvasGroup.alpha = 1;
        while (fadeCanvasGroup.alpha > 0.0f)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.gameObject.SetActive(false); // CanvasGroup���\��
    }

    public void SetGameMode(GameMode mode)
    {
        SelectedGameMode = mode;
    }

}
