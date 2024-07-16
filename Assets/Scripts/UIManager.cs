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
    public Button CreateTowerBtn;
    public Button EnhanceTowerBtn;
    public Button EnhancePlayerBtn;

    public GameObject createTowerBtns;
    public Button BackBtn_0;
    public TMP_Text[] createTowerBtnTexts;

    public GameObject EnhanceTowerBtns;
    public Button BackBtn_1;
    public GameObject EnhancePlayerBtns;
    public Button BackBtn_2;

    private Camera mainCamera;

    void Start()
    {
        CreateTowerBtn.onClick.AddListener(OnCreateTowerBtnClicked);
        EnhanceTowerBtn.onClick.AddListener(OnEnhanceTowerButtonClicked);
        EnhancePlayerBtn.onClick.AddListener(OnEnhancePlayerButtonClicked);
        BackBtn_0.onClick.AddListener(OnBackButtonClicked);
        BackBtn_1.onClick.AddListener(OnBackButtonClicked);
        BackBtn_2.onClick.AddListener(OnBackButtonClicked);

        

        mainCamera = Camera.main;
        coin = 0f;
        UpdateCoin();
        LoadSelectedTowers();
        InitializeTowerButtons();
    }
    void LoadSelectedTowers()
    {
        items = GameData.instance.selectedTowers.ToArray();
        InitializeTowerButtons();
    }

    void InitializeTowerButtons()
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

    void OnCreateTowerBtnClicked()
    {
        createTowerBtns.SetActive(true);
        storeMenuBtns.SetActive(false);
    }

    void OnEnhanceTowerButtonClicked()
    {
        EnhanceTowerBtns.SetActive(true);
        storeMenuBtns.SetActive(false);
    }

    void OnEnhancePlayerButtonClicked()
    {
        EnhancePlayerBtns.SetActive(true);
        storeMenuBtns.SetActive(false);
    }

    void OnBackButtonClicked()
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
                selectedItemIdx = itemIndex;
                isPlacingItem = true;
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

        // 초기화
        Destroy(selectedItem);
        selectedItem = null;
        selectedItemIdx = -1;
        isPlacingItem = false;
    }
}
