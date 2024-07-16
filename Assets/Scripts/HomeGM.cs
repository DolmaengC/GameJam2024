using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public class HomeGM : MonoBehaviour
{
    // 3 parts
    public GameObject homeParts;
    public GameObject settingParts;
    public GameObject enhanceParts;

    // core
    public Button homeButton;
    public Button settingButton;
    public Button enhanceButton;
    public Button exitButton;

    // for Home Parts
    public Button playButton;
    public Button nextStageButton;
    public Button beforeStageButton;
    public TMP_Text stageText;
    private int currentStage = 1;

    // for Setting Parts
    public Button playerSettingButton;
    public Button towerSettingButton;
    public GameObject playerSettingParts;
    public GameObject towerSettingParts;
    

    void Start()
    {
        playButton.onClick.AddListener(OnplayButtonClicked);

        homeButton.onClick.AddListener(OnHomeButtonClicked);
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        enhanceButton.onClick.AddListener(OnEnhanceButtonClicked);
        
        nextStageButton.onClick.AddListener(OnNextStageButtonClicked);
        beforeStageButton.onClick.AddListener(OnBeforeStageButtonClicked);

        playerSettingButton.onClick.AddListener(OnPlayerSettingButtonClicked);
        towerSettingButton.onClick.AddListener(OnTowerSettingButtonClicked);

        UpdateStageText();
        
    }


    // core
    void OnHomeButtonClicked()
    {
        homeParts.SetActive(true);
        settingParts.SetActive(false);
        enhanceParts.SetActive(false);
    }
    void OnSettingButtonClicked()
    {
        // Setting 버튼 클릭 시 동작 정의
        Debug.Log("Setting Button Clicked");
        homeParts.SetActive(false);
        settingParts.SetActive(true);
        enhanceParts.SetActive(false);
    }
    void OnEnhanceButtonClicked()
    {
        // Enhance 버튼 클릭 시 동작 정의
        Debug.Log("Enhance Button Clicked");
        homeParts.SetActive(false);
        settingParts.SetActive(false);
        enhanceParts.SetActive(true);
    }
    void OnExitButtonClicked()
    {
        Debug.Log("Exit Button Clicked");
        // Application.Quit();
    }

    // for Home parts
    void OnplayButtonClicked()
    {
        // 예시로 'GameScene'이라는 이름의 씬을 로드합니다.
        Debug.Log("Play Button Clicked");
        // SceneManager.LoadScene("SampleScene"); 
        SceneManager.LoadScene("Stage" + currentStage.ToString()); 
    }
    void OnNextStageButtonClicked()
    {
        currentStage++;
        UpdateStageText();
    }

    void OnBeforeStageButtonClicked()
    {
        if (currentStage > 1)
        {
            currentStage--;
            UpdateStageText();
        }
    }

    void UpdateStageText()
    {
        stageText.text = "Stage " + currentStage;
    }

     // for Setting parts
    void OnPlayerSettingButtonClicked()
    {
        Debug.Log("Player Setting Button Clicked");
        playerSettingParts.SetActive(true);
        towerSettingParts.SetActive(false);    
    }
    void OnTowerSettingButtonClicked()
    {
        Debug.Log("Tower Setting Button Clicked");
        playerSettingParts.SetActive(false);
        towerSettingParts.SetActive(true);
    }
    
}
