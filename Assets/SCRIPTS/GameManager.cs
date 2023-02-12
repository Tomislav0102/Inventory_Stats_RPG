
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK;
using TK.Enums;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public Transform PlTransform;
    [HideInInspector] public PlayerMove PlayerMove;
    [HideInInspector] public StatManager StatManager;
    public BoxCollider2D[] PlayerColliders;
    public PickUpTool PlayerPickUpTool;
    [HideInInspector] public InvManager invManager;
    [HideInInspector] public SpawnItemsManager SpawnItemsManager;
    public Sprite Empty;
    public Sprite[] EquipmentSprites;
    public Camera MainCam;
    public LayerMask L_spawnArea;
    public LayerMask L_pickups;
    [HideInInspector] public SO_items[] AllItems;

    public bool durabilitySpentWhileWalking;

    [SerializeField] Canvas _canMainMenu;
    [SerializeField] TextMeshProUGUI _displayInfoEQUIP;
    [SerializeField] TextMeshProUGUI _displayInfoINV;
    [SerializeField] TextMeshProUGUI _displayCoinINV;
    [SerializeField] TextMeshProUGUI _displayMain;
    const string CONSTANT_INV = "Left click to choose/place items, right click finds a place for item automatically, right click + left shift drops item on the ground, middle click uses consumable and left click + left control splits stack.";
    const string CONSTANT_MAIN = "Press 'Space' to spawn items and 'Esc' to show main menu.";
    int _currentGold;
    [HideInInspector] public bool _mainMenuOpen;
    private void Awake()
    {
        gm = this;
        PlayerMove = PlTransform.GetComponent<PlayerMove>();
        StatManager = GetComponent<StatManager>();
        SpawnItemsManager = GetComponent<SpawnItemsManager>();
        AllItems = Resources.LoadAll<SO_items>("Items");
        invManager = GetComponent<InvManager>();
    }
    private void Start()
    {
        Analytics.CustomEvent("Build Startup", new Dictionary<string, object> { { Application.platform.ToString(), System.DateTime.Now } });
       
        _displayInfoEQUIP.text = string.Empty;
        _displayInfoINV.text = CONSTANT_INV;
        DisplayGold(0);
        _displayMain.text = CONSTANT_MAIN;
    }

    private void OnEnable()
    {
        InventoryUtilities.MainMenuOpen += Method_MainMenuOpen;
    }
    private void OnDisable()
    {
        InventoryUtilities.MainMenuOpen -= Method_MainMenuOpen;
    }
    void Method_MainMenuOpen(bool b)
    {
        _mainMenuOpen = b;
        _canMainMenu.enabled = _mainMenuOpen;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _mainMenuOpen = !_mainMenuOpen;
            InventoryUtilities.MainMenuOpen?.Invoke(_mainMenuOpen);
        }

    }

    #region //DISPLAYS
    public void DisplayInv(string tekst)
    {
        _displayInfoINV.text = (tekst == "") ? CONSTANT_INV : tekst;
    }
    public void DisplayEquipment(TotalItem totalItem, SO_items item)
    {
        if(item == null || item.itemType != ItemType.Wearable)
        {
            _displayInfoEQUIP.text = string.Empty;
            return;
        }

        string textToShow;
        textToShow = item.Name + "\n" + "\n";
        switch (item.equipType)
        {
            case EquipmentType.Weapon:
                textToShow += "Damage: " + item.damage.ToString() + "\n";
                break;
            default:
                textToShow += "Armor Class: " + item.ACbonus.ToString() + "\n";
                break;
        }
        foreach (SO_items.ExtraBonus bonus in item.extraBonuses)
        {
            textToShow += "Bonus: " + bonus.statsBonus.ToString() + " " + bonus.amount.ToString() + "\n";
        }
        if (item.itemType == ItemType.Wearable) textToShow += "Durablitiy: " + totalItem.CurrentDurability.ToString() + "/" + item.durablity.ToString();
        _displayInfoEQUIP.text = textToShow;
    }
    public void DisplayGold(int gold)
    {
        _currentGold += gold;
        _displayCoinINV.text = _currentGold.ToString();
    }
    public void DisplayMainInfo(SO_items item, string tekst)
    {
        StopAllCoroutines();
        if (item == null)
        {
            _displayMain.text = tekst;
        }
        else
        {
            _displayMain.text = "You've used " + item.Name;
        }
        print(_displayMain.text);
        StartCoroutine(ReturnDisplayToDefaultState());
    }
    IEnumerator ReturnDisplayToDefaultState()
    {
        yield return new WaitForSeconds(5f);
        _displayMain.text = CONSTANT_MAIN;
    }
    #endregion
    #region//BUTTONS
    public void Btn_Restart()
    {
        SceneManager.LoadScene(gameObject.scene.name);
    }
    public void Btn_ReturnToGame()
    {
        InventoryUtilities.MainMenuOpen?.Invoke(false);
    }
    public void Btn_QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
