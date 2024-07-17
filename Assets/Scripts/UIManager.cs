using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject[] items; // 상점 아이템 프리팹들
    private GameObject selectedItem;
    private int selectedItemIdx = -1;  // 선택된 아이템 인덱스를 저장할 변수
    private bool isPlacingItem = false;
    public float coin;
    public TMP_Text coinText;
    public GameObject storeMenuBtns;
    public Button CreateTowerViewBtn;
    public Button EnhanceTowerViewBtn;
    public Button EnhancePlayerViewBtn;

    public GameObject createTowerBtns;
    public Button BackBtn_0;
    public Button[] createTowerButtons;
    public GameObject[] createTowerImg;
    public TMP_Text[] createTowerBtnTexts;

    public GameObject EnhanceTowerBtns;
    public Button BackBtn_1;
    public Button[] enhanceTowerButtons; // 배열로 변경
    public TMP_Text[] enhanceTowerBtnTexts;
    public GameObject[] enhanceTowerImg;
    public GameObject EnhancePlayerBtns;
    public Button BackBtn_2;
    private Camera mainCamera;

    private CapsuleCollider2D selectedCapsuleCollider2D;
    private Scanner selectedScanner;
    private Canvas selectedCanvas;

    void Start()
    {
        CreateTowerViewBtn.onClick.AddListener(OnCreateTowerViewBtnClicked);
        EnhanceTowerViewBtn.onClick.AddListener(OnEnhanceTowerViewBtnClicked);
        EnhancePlayerViewBtn.onClick.AddListener(OnEnhancePlayerViewBtnClicked);
        BackBtn_0.onClick.AddListener(OnBackBtnClicked);
        BackBtn_1.onClick.AddListener(OnBackBtnClicked);
        BackBtn_2.onClick.AddListener(OnBackBtnClicked);

        // CreateTower
        for (int i = 0; i < createTowerButtons.Length; i++)
        {
            int index = i; // 로컬 복사본 생성
            createTowerButtons[i].onClick.AddListener(() => SelectItem(index));
        }

        // EnhanceTower 버튼에 각각 리스너 추가
        for (int i = 0; i < enhanceTowerButtons.Length; i++)
        {
            int index = i; // 로컬 복사본 생성
            enhanceTowerButtons[i].onClick.AddListener(() => OnEnhanceTowerButtonClicked(index));
        }


        mainCamera = Camera.main;
        coin = 0f;
        UpdateCoin();
        LoadSelectedTowers();
        SetTowerImgs();
        UpdateCreateTowerButtonsText();
        UpdateEnhanceTowerButtonsText();
    }

    void LoadSelectedTowers()
    {
        items = GameData.instance.selectedTowers.ToArray();
    }
    void SetTowerImgs() {
        for (int i = 0; i < items.Length; i++)
        {
            if (i < createTowerImg.Length)
            {
                GameObject item = items[i];
                TowerManager towerManager = item.GetComponent<TowerManager>();

                if (towerManager != null)
                {
                    createTowerImg[i].GetComponent<Image>().sprite = towerManager.towerImg;
                    enhanceTowerImg[i].GetComponent<Image>().sprite = towerManager.towerImg;
                }
            }
        }
    }

    void UpdateCreateTowerButtonsText()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (i < createTowerBtnTexts.Length)
            {
                GameObject item = items[i];
                TowerManager towerManager = item.GetComponent<TowerManager>();

                if (towerManager != null)
                {
                    int buildCost = towerManager.buildCost;
                    createTowerBtnTexts[i].text = item.name + ": " + buildCost.ToString();
                }
            }
        }
    }

    void UpdateEnhanceTowerButtonsText()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (i < enhanceTowerBtnTexts.Length)
            {
                GameObject item = items[i];
                TowerManager towerManager = item.GetComponent<TowerManager>();

                if (towerManager != null)
                {
                    int buildCost = towerManager.enhanceCost;
                    int state = TowerManager.towerStates.ContainsKey(item.name) ? TowerManager.towerStates[item.name] : 0;
                    enhanceTowerBtnTexts[i].text = item.name + "(" + state +  "): " + buildCost.ToString();
                }
            }
        }
    }

    void OnCreateTowerViewBtnClicked()
    {
        createTowerBtns.SetActive(true);
        storeMenuBtns.SetActive(false);
    }

    void OnEnhanceTowerViewBtnClicked()
    {
        UpdateEnhanceTowerButtonsText(); // 강화 버튼 텍스트 업데이트
        EnhanceTowerBtns.SetActive(true);
        storeMenuBtns.SetActive(false);
    }

    void OnEnhancePlayerViewBtnClicked()
    {
        EnhancePlayerBtns.SetActive(true);
        storeMenuBtns.SetActive(false);
    }

    void OnBackBtnClicked()
    {
        createTowerBtns.SetActive(false);
        EnhanceTowerBtns.SetActive(false);
        EnhancePlayerBtns.SetActive(false);
        storeMenuBtns.SetActive(true);
    }

    void Update()
    {
        if (isPlacingItem && selectedItem != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);  // 카메라의 z 위치를 고려하여 조정

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0;  // 여기서 z를 0으로 설정합니다.
            selectedItem.transform.position = worldPosition;

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PlaceItem(worldPosition);
            }
        }
    }

    void FixedUpdate()
    {
        coin += 0.05f;
        UpdateCoin();
    }

    public void UpdateCoin()
    {
        coinText.text = Mathf.FloorToInt(coin).ToString();
    }

    public void SelectItem(int itemIndex)
    {
        GameObject item = items[itemIndex];
        TowerManager towerManager = item.GetComponent<TowerManager>();

        if (towerManager != null)
        {
            int buildCost = towerManager.buildCost;

            // 코인이 충분한지 확인합니다.
            if (coin >= buildCost)
            {
                if (selectedItem != null)
                {
                    Destroy(selectedItem);
                }

                selectedItem = Instantiate(items[itemIndex]);
                selectedItem.GetComponent<TowerManager>().UpdateHP();
                selectedItemIdx = itemIndex;
                isPlacingItem = true;

                // CapsuleCollider2D와 Scanner 비활성화
                selectedCapsuleCollider2D = selectedItem.GetComponent<CapsuleCollider2D>();
                selectedScanner = selectedItem.GetComponent<Scanner>();
                selectedCanvas = selectedItem.GetComponentInChildren<Canvas>();
                
                if (selectedCapsuleCollider2D != null)
                {
                    selectedCapsuleCollider2D.enabled = false;
                }

                if (selectedScanner != null)
                {
                    selectedScanner.enabled = false;
                }

                if (selectedCanvas != null)
                {
                    selectedCanvas.enabled = false;
                }
            }
            else
            {
                Debug.Log("Not enough coins to build this item.");
            }
        }
    }

    void PlaceItem(Vector3 position)
    {
        GameObject item = Instantiate(items[selectedItemIdx], position, Quaternion.identity);
        TowerManager towerManager = item.GetComponent<TowerManager>();

        if (towerManager != null)
        {
            int buildCost = towerManager.buildCost;

            // 코인 차감
            coin -= buildCost;
            UpdateCoin();
        }

        // CapsuleCollider2D, Scanner 및 Canvas 다시 활성화
        CapsuleCollider2D itemCapsuleCollider2D = item.GetComponent<CapsuleCollider2D>();
        Scanner scanner = item.GetComponent<Scanner>();
        Canvas canvas = item.GetComponentInChildren<Canvas>();

        if (itemCapsuleCollider2D != null)
        {
            itemCapsuleCollider2D.enabled = true;
        }

        if (scanner != null)
        {
            scanner.enabled = true;
        }

        if (canvas != null)
        {
            canvas.enabled = true;
        }

        // 초기화
        Destroy(selectedItem);
        selectedItem = null;
        selectedItemIdx = -1;
        isPlacingItem = false;
    }

    // 상태를 증가시키는 버튼 클릭 시 호출되는 메서드
    void OnEnhanceTowerButtonClicked(int index)
    {
        if (index >= 0 && index < items.Length)
        {
            GameObject item = items[index];
            TowerManager towerManager = item.GetComponent<TowerManager>();

            if (towerManager != null)
            {
                int enhanceCost = towerManager.enhanceCost;

                // 코인이 충분한지 확인합니다.
                if (coin >= enhanceCost)
                {
                    TowerManager.IncreaseState(item.name);
                    coin -= enhanceCost;
                    UpdateCoin();
                    UpdateEnhanceTowerButtonsText(); // 상태 변경 후 버튼 텍스트 업데이트
                }
                else
                {
                    Debug.Log("Not enough coins to enhance this item.");
                }
            }
        }
    }

}
