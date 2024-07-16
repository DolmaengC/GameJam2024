using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollViewManager : MonoBehaviour
{
    public GameObject buttonPrefab; // 버튼 프리팹
    public Transform content; // ScrollView의 Content 트랜스폼
    public Button[] towerSettingButtons; // TowerSetting 안에 있는 버튼들
    public GameObject[] towerList;

    private HashSet<string> activeButtonTexts = new HashSet<string>();

    void Start()
    {
        if (buttonPrefab == null)
        {
            Debug.LogError("Button Prefab is not assigned.");
            return;
        }

        if (content == null)
        {
            Debug.LogError("Content Transform is not assigned.");
            return;
        }

        if (towerSettingButtons == null || towerSettingButtons.Length == 0)
        {
            Debug.LogError("TowerSetting Buttons are not assigned.");
            return;
        }

        AddButtonsToScrollView();

        // 모든 TowerSetting 버튼들을 비활성화
        foreach (var button in towerSettingButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.AddListener(() => OnTowerSettingButtonClick(button));
        }
    }

    void AddButtonsToScrollView()
    {
        for (int i = 0; i < towerList.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, content);
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = towerList[i].name;
            }
            else
            {
                Debug.LogError("TMP_Text component not found in button prefab.");
            }

            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                int index = i; // Capture the current value of i
                buttonComponent.onClick.AddListener(() => OnScrollViewButtonClick(buttonText.text, towerList[index]));
            }
            else
            {
                Debug.LogError("Button component not found in button prefab.");
            }
        }

        // Content Size Fitter를 갱신하여 모든 버튼이 올바르게 표시되도록 합니다.
        StartCoroutine(RefreshContentSizeFitter());
    }

    IEnumerator RefreshContentSizeFitter()
    {
        yield return null; // 한 프레임 대기
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
    }

    void OnScrollViewButtonClick(string buttonText, GameObject tower)
    {
        if (activeButtonTexts.Contains(buttonText))
        {
            Debug.LogWarning("Button with text " + buttonText + " is already active.");
            return;
        }

        Button towerButton = FindInactiveTowerButton();
        if (towerButton != null)
        {
            towerButton.gameObject.SetActive(true);
            TMP_Text towerButtonText = towerButton.GetComponentInChildren<TMP_Text>();
            if (towerButtonText != null)
            {
                towerButtonText.text = buttonText;
                activeButtonTexts.Add(buttonText); // 중복 방지를 위해 텍스트 추가
                GameData.instance.selectedTowers.Add(tower); // 선택된 타워를 GameData에 추가
            }
        }
        else
        {
            Debug.LogWarning("No inactive TowerSetting button available.");
        }
    }

    Button FindInactiveTowerButton()
    {
        foreach (var button in towerSettingButtons)
        {
            if (!button.gameObject.activeSelf)
            {
                return button;
            }
        }
        return null;
    }

    void OnTowerSettingButtonClick(Button button)
    {
        button.gameObject.SetActive(false);
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            activeButtonTexts.Remove(buttonText.text); // 활성화된 버튼 텍스트 제거
            // 선택된 타워를 GameData에서 제거
            GameData.instance.selectedTowers.RemoveAll(tower => tower.name == buttonText.text);
        }
    }
}
