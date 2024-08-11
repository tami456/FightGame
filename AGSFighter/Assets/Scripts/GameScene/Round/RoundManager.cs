using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    public GameObject player1;
    public GameObject player2;


    public int player1Wins = 0;
    public int player2Wins = 0;
    public int currentRound = 1;
    public int totalRounds = 3;
    [SerializeField] string selectedStage; // �I�����ꂽ�X�e�[�W����ۑ�����ϐ�

    public bool isRoundOver = false; // �ǉ�

    [SerializeField]private RentalUIAnimation testUI;


    // CountDown�X�N���v�g�̎Q��
    [SerializeField] private CountDown countDown;


    [SerializeField] private WinStarsUI player1WinStarsUI;
    [SerializeField] private WinStarsUI player2WinStarsUI;

    private Dictionary<string, string> stageBGMMapping;


    void Awake()
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
        InitializeStageBGMMapping();
    }

    private void Start()
    {
    
    }

    private void InitializeStageBGMMapping()
    {
        stageBGMMapping = new Dictionary<string, string>
        {
            { "SelectStage1", "�g���[�j���O���j���[" },
            { "SelectStage2", "���ēS��" },
            { "SelectStage3", "��R�S�H��" },
            // ���̃X�e�[�W-BGM�̃y�A��ǉ�
        };
    }

    public void Create()
    {
        player1 = GameObject.Find("yuji");
        player2 = GameObject.Find("yuji2P");
        // CountDown�X�N���v�g�̎Q�Ƃ��擾
        countDown = FindObjectOfType<CountDown>();
        player1WinStarsUI =
            GameObject.Find(
                "HP&SP").transform.Find(
                "Player1WinStars").GetComponent<WinStarsUI>();
        player2WinStarsUI =
            GameObject.Find(
                "HP&SP").transform.Find(
                "Player2WinStars").GetComponent<WinStarsUI>();
    }

    public void Player1Wins()
    {
        if (isRoundOver) return; // �ǉ�
        Debug.LogWarning("�v���C���[1�̏���");
        
        isRoundOver = true; // �ǉ�
        player1Wins++;
        UpdateWinStarsUI();
        CheckGameOver();
    }

    public void Player2Wins()
    {
        if (isRoundOver) return; // �ǉ�
        Debug.LogWarning("�v���C���[2�̏���");
        
        isRoundOver = true; // �ǉ�
        player2Wins++;
        UpdateWinStarsUI();
        CheckGameOver();
    }

    public void PlayerDraw()
    {
        if (isRoundOver) return; // �ǉ�]
        Debug.LogWarning("���������I");

        isRoundOver = true; // �ǉ�
        player1Wins++;
        player2Wins++;
        UpdateWinStarsUI();
        CheckGameOver();
    }

    void CheckGameOver()
    {
        isRoundOver = false; // �ǉ�
        Debug.Log("CheckGameOver called");
        if (player1Wins == player2Wins && 
            player1Wins
            >= totalRounds / 2 + 1)
        {
            // �v���C���[1������
            Debug.Log("Draw Game");
            EndGame("Draw�c");
        }
        else if (player1Wins >= totalRounds / 2 + 1)
        {
            // �v���C���[1������
            Debug.Log("Player 1 Wins the Game!");
            EndGame("Player 1 Wins the Game!");
        }
        else if (player2Wins >= totalRounds / 2 + 1)
        {
            // �v���C���[2������
            Debug.Log("Player 2 Wins the Game!");
            EndGame("Player 2 Wins the Game!");
        }
        else
        {
            // ���̃��E���h��
            currentRound++;
            if (currentRound > totalRounds)
            {
                // ���̏������ɒB���Ă��Ȃ��ꍇ�A��������SA�Q�[�W�����Z�b�g���A���E���h�����X�^�[�g
                ResetScoresAndSA();
                StartCoroutine(EndRoundCoroutine());
            }
            else
            {
                StartCoroutine(StartRound());
            }
        }
    }

    private IEnumerator StartRound()
    {
        Debug.Log("StartRound called");

        yield return FadeController.Instance.StartCoroutine(FadeController.Instance.FadeOut());
        FadeController.Instance.StartCoroutine(FadeController.Instance.FadeIn());
       


        if (player1 != null && player2 != null)
        {

            player1.GetComponent<PlayerHit>().ResetPlayer();
            player2.GetComponent<PlayerHit>().ResetPlayer();

            testUI.Start();
            GameObject.Find("RoundCount").transform.GetComponent<RoundCount>().RoundText();
        }

        else
        {
            Debug.LogError("�v���C���[��������܂���B");
        }

    }

    void ResetScoresAndSA()
    {
        player1Wins = 0;
        player2Wins = 0;
        Debug.Log("Scores and SA reset");

        // ���̃��Z�b�g������ǉ�����ꍇ�͂����ɋL�q
    }
   

    private IEnumerator EndRoundCoroutine()
    {

        yield return new WaitForSeconds(1f); // �����̗P�\��u��                                    // �V�[�������[�h
        ReloadScene();
         yield return new WaitForSeconds(1f); // �V�[�������[�h��̗P�\
    }

    private void ReloadScene()
    {
        if (!string.IsNullOrEmpty(selectedStage))
        {
            SceneManager.LoadScene(selectedStage);
        }
        else
        {
            Debug.LogError("selectedStage�ϐ��ɃV�[�������ݒ肳��Ă��܂���B");
        }
    }

    void EndGame(string winnerMessage)
    {
        Debug.Log("EndGame called with message: " + winnerMessage);
        GameResult.winnerMessage = winnerMessage;
        SceneManager.LoadScene("EndGameScene");
        SoundManager.Instance.StopBGM();
    }

    public void SetSelectedStage(string stageName)
    {
        selectedStage = stageName;
        PlayStageBGM(stageName);
    }

    private void PlayStageBGM(string stageName)
    {
        if (stageBGMMapping.TryGetValue(stageName, out string bgmName))
        {
            Debug.Log($"�X�e�[�W: {stageName} ��BGM: {bgmName} ���Đ�");
            SoundManager.Instance.PlayBGM(bgmName);
        }
        else
        {
            Debug.LogError($"�X�e�[�W: {stageName} �ɑΉ�����BGM��������܂���");
        }
    }

    public void NotifyPlayerDefeated(GameObject player)
    {
        if (isRoundOver) return; // �ǉ�
        countDown.ResetCountDown();
        Debug.LogWarning("�ʒm������[");
        Debug.LogWarning("�������E��ꂽ: " + player.name);

        if (player == player1)
        {
            Debug.LogWarning(player.name + "�̎��S���m�F (player1)");
            Player2Wins();
        }
        else if (player == player2)
        {
            Debug.LogWarning(player.name + "�̎��S���m�F (player2)");
            Player1Wins();
        }
        else
        {
            Debug.LogError("�Q�Ƃ���v���܂���B");
            Debug.LogError("player" + player.GetInstanceID());
            Debug.LogError("player1" + player1.GetInstanceID());
            Debug.LogError("player2" + player2.GetInstanceID());
        }
    }

    public void TimeOut()
    {
        if (player1 != null && player2 != null)
        {
            if (player1.GetComponent<PlayerHit>().hpGauge._currentHP > player2.GetComponent<PlayerHit>().hpGauge._currentHP)
            {
                Player1Wins();
            }
            else if (player1.GetComponent<PlayerHit>().hpGauge._currentHP < player2.GetComponent<PlayerHit>().hpGauge._currentHP)
            {
                Player2Wins();
            }
            else
            {
                // �̗͂������ꍇ�̏����i�������������ɂ���Ȃǁj
                Debug.Log("Draw! Both players have the same health.");
                PlayerDraw();

            }
        }
        else
        {
            Debug.LogError("�v���C���[��������܂���B");
        }
    }


    private void UpdateWinStarsUI()
    {
        player1WinStarsUI.UpdateWinStars(player1Wins);
        player2WinStarsUI.UpdateWinStars(player2Wins);
    }

    private void ResetWinStarsUI()
    {
        player1WinStarsUI.ResetWinStars();
        player2WinStarsUI.ResetWinStars();
    }

    public void ResetRoundCount()
    {
        player1Wins = 0;
        player2Wins = 0;
        currentRound = 1;
    }
}
