using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TK;
using TK.Enums;
using TMPro;
using UnityEngine.EventSystems;


public class InvManager : MonoBehaviour
{
    GameManager gm;
    [HideInInspector] public InvIndividual HoverSl;
    public InvCarry carryScript;
    public TotalItem buffer;

    bool _aktiv = true;
    [SerializeField] Canvas[] _canvases;

    //split
    [SerializeField] GameObject splitScreen;
    [SerializeField] Image splitImage;
    [SerializeField] Slider splitSlider;
    [SerializeField] TextMeshProUGUI[] splitTexts;
    InvIndividual[] invsToBeSplit = new InvIndividual[2];
    int[] _splitNumber = new int[2];
    float prevValue;


    [SerializeField] GameObject[] _buttonsMain;

    [SerializeField] Transform _parInventory, _parEquip;
    InvIndividual[] _inventorySlots;
    InvIndividual[] _equipInvs;
    List<InvIndividual> AllSlotsWithItem(SO_items item)
    {
        List<InvIndividual> list = new List<InvIndividual>();
        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (_inventorySlots[i].Item == item)
            {
                if (item == null ||
                    !item.stackable ||
                    item.stackMax == 999 ||
                    _inventorySlots[i].StackNum < item.stackMax)
                {
                    list.Add(_inventorySlots[i]);
                }
            }
        }
        return list;
    }
    public bool OpenWindows()
    {
        return _canvases[1].enabled || _canvases[2].enabled;
    }

    private void Awake()
    {
        gm = GameManager.gm;
        _inventorySlots = new InvIndividual[_parInventory.childCount];
        for (int i = 0; i < _parInventory.childCount; i++)
        {
            _inventorySlots[i] = _parInventory.GetChild(i).GetChild(0).GetComponent<InvIndividual>();
        }
        _equipInvs = new InvIndividual[_parEquip.childCount];
        for (int i = 0; i < _parEquip.childCount; i++)
        {
            _equipInvs[i] = _parEquip.GetChild(i).GetChild(0).GetComponent<InvIndividual>();
        }
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            _canvases[i].enabled = false;
        }
        splitScreen.SetActive(false);
       // _canvases[0].enabled = true;
    }
    private void OnEnable()
    {
        InventoryUtilities.MainMenuOpen += Method_Aktiv;
    }
    private void OnDisable()
    {
        InventoryUtilities.MainMenuOpen -= Method_Aktiv;
    }
    void Method_Aktiv(bool b)
    {
        _aktiv = !b;
    }

    private void RefreshStats()
    {
        gm.StatManager.ResetValues(ValueResetType.MagicalItems);

        for (int i = 0; i < 7; i++) 
        {
            if (_equipInvs[i].Item != null)
            {
                SO_items it = _equipInvs[i].Item;
                StatManager.Derivatives[Stats.Damage].magicalItemValue += it.damage;
                StatManager.Derivatives[Stats.ArmorClass].magicalItemValue += it.ACbonus;

                foreach (SO_items.ExtraBonus bonus in it.extraBonuses)
                {
                    if ((int)bonus.statsBonus < 5)
                    {
                        StatManager.Abilities[bonus.statsBonus].magicalItemValue += bonus.amount;
                    }
                    else
                    {
                        StatManager.Derivatives[bonus.statsBonus].magicalItemValue += bonus.amount;
                    }
                }

            }
        }

        gm.StatManager.RefreshDisplays();
    }
    public void FindSpaceForItem(PickUpIndividual pu)
    {
        SO_items item = pu.Item;
        if (pu.StackNum == 0 || item == null) return;

        switch (item.itemType)
        {
            case ItemType.Wearable:
                if (item.equipType == EquipmentType.Rings)
                {
                    if (_equipInvs[5].Item == null)
                    {
                        _equipInvs[5].TakeItem(pu);
                        gm.DisplayMainInfo(null, "You equip the " + item.Name);
                        pu.gameObject.SetActive(false);
                    }
                    else if (_equipInvs[6].Item == null)
                    {
                        _equipInvs[6].TakeItem(pu);
                        gm.DisplayMainInfo(null, "You equip the " + item.Name);
                        pu.gameObject.SetActive(false);
                    }
                    else SimpleFind(pu);
                }
                else if (_equipInvs[(int)item.equipType].Item == null)
                {
                    _equipInvs[(int)item.equipType].TakeItem(pu);
                    gm.DisplayMainInfo(null, "You equip the " + item.Name);
                    pu.gameObject.SetActive(false);
                }
                else SimpleFind(pu);
                RefreshStats();
                break;

            default:
                if (item.stackable)
                {
                    List<InvIndividual> lis = AllSlotsWithItem(item);
                    if (lis.Count == 0) SimpleFind(pu);
                    else
                    {
                        foreach (InvIndividual slot in lis)
                        {
                            if (pu.Item == null) return;

                            if (slot.Item.stackMax == 999 ||
                                pu.StackNum + slot.StackNum <= item.stackMax)
                            {
                                slot.StackNum += pu.StackNum;
                                pu.gameObject.SetActive(false);
                                gm.DisplayMainInfo(null, "You've picked up a " + item.Name);
                                return;
                            }
                            else
                            {
                                int razlika = pu.StackNum - (item.stackMax - slot.StackNum);
                                slot.StackNum = item.stackMax;
                                pu.StackNum = razlika;
                            }
                        }
                    }
                }
                else SimpleFind(pu);
                break;
        }
    }
    private void SimpleFind(PickUpIndividual pu)
    {
        if (AllSlotsWithItem(null).Count > 0)
        {
            AllSlotsWithItem(null)[0].TakeItem(pu);
            gm.DisplayMainInfo(null, "You've picked up a " + pu.Item.Name);
            pu.gameObject.SetActive(false);
        }
        else gm.DisplayMainInfo(null, "No room in inventory");
    }

    void SimpleExchange(TotalItem male, TotalItem female)
    {
        buffer.TakeItem(female);
        female.TakeItem(male);
        male.TakeItem(buffer);
    }

    private void Update()
    {
        #region//STANDARD
        if (!_aktiv)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!_canvases[0].enabled) Btn_OpenWindows(0);
            else Btn_CloseWindow(0);
        }
        else if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (!_canvases[1].enabled) Btn_OpenWindows(1);
            else Btn_CloseWindow(1);
        }
        else if (Input.GetKeyDown(KeyCode.I)) 
        {
            if (!_canvases[2].enabled) Btn_OpenWindows(2);
            else Btn_CloseWindow(2);
        }

        if (!OpenWindows() || splitScreen.activeInHierarchy) 
        {
            return;
        }
        #endregion

        if (Input.GetMouseButtonDown(0))
        {
            if (carryScript.gameObject.activeInHierarchy)
            {
                if (!EventSystem.current.IsPointerOverGameObject()) //drop carried item
                {
                    gm.SpawnItemsManager.DropItem("", carryScript);
                    carryScript.Item = null;
                }
                else if (HoverSl != null) //switch item between carry and hover
                {
                    switch (HoverSl.slotType)
                    {
                        case InvSlotType.Inventory:
                            if (HoverSl.Item != null && HoverSl.Item.stackable && //STACKABLES
                                HoverSl.Item == carryScript.Item)
                            {
                                if (HoverSl.Item.stackMax == 999)
                                {
                                    HoverSl.StackNum += carryScript.StackNum;
                                    carryScript.StackNum = 0; //carry bi trebao nestati s ovim
                                }
                                else
                                {
                                    if (HoverSl.StackNum + carryScript.StackNum <= HoverSl.Item.stackMax)
                                    {
                                        HoverSl.StackNum += carryScript.StackNum;
                                        carryScript.StackNum = 0; //carry bi trebao nestati s ovim
                                    }
                                    else
                                    {
                                        carryScript.StackNum -=  HoverSl.Item.stackMax - HoverSl.StackNum;
                                        HoverSl.StackNum = HoverSl.Item.stackMax;
                                    }
                                }
                            }
                            else SimpleExchange(carryScript, HoverSl);
                            break;

                        case InvSlotType.Equipment:
                            if (carryScript.Item.itemType == ItemType.Wearable &&
                                carryScript.Item.equipType == HoverSl.equipment)
                            {
                                SimpleExchange(carryScript, HoverSl);
                            }
                            else
                            {
                                gm.DisplayInv(carryScript.Item.Name + " doesn't fit here.");
                            }
                            break;
                    }
                }
            }
            else if (HoverSl != null && HoverSl.Item != null) 
            {
                if (Input.GetKey(KeyCode.LeftControl) && HoverSl.StackNum > 1) //split stack
                {
                    splitScreen.SetActive(true);
                    invsToBeSplit[0] = HoverSl;
                    splitImage.sprite = invsToBeSplit[0].Item.Sprite;
                    _splitNumber[0] = invsToBeSplit[0].StackNum;
                    _splitNumber[1] = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        splitTexts[i].text = _splitNumber[i].ToString();
                    }

                    splitSlider.value = 0f;
                    splitSlider.maxValue = invsToBeSplit[0].StackNum;
                    prevValue = 0f;
                }
                else //pick item from inventory or equipment
                {
                    carryScript.TakeItem(HoverSl);
                    carryScript.gameObject.SetActive(true);
                    HoverSl.Item = null;
                }
            }
            RefreshStats();
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (carryScript.gameObject.activeInHierarchy ||
            HoverSl == null || HoverSl.Item == null)
            {
                return;
            }

            switch (HoverSl.slotType)
            {
                case InvSlotType.Inventory: //move item from inv to equipment. if equipment has item, put that item in inv slot where hover is
                    if (HoverSl.Item.itemType == ItemType.Wearable)
                    {
                        if (_equipInvs[(int)HoverSl.Item.equipType].Item == null)
                        {
                            _equipInvs[(int)HoverSl.Item.equipType].TakeItem(HoverSl);
                            HoverSl.Item = null;
                        }
                        else SimpleExchange(HoverSl, _equipInvs[(int)HoverSl.Item.equipType]);
                    }
                    else gm.DisplayInv(HoverSl.Item.Name + " can't be equiped.");

                    break;
                case InvSlotType.Equipment: //remove item from equipment to inventory. if inv is full than drop to ground
                    if (AllSlotsWithItem(null).Count <= 0) gm.SpawnItemsManager.DropItem("No room in inventory. ", HoverSl);
                    else AllSlotsWithItem(null)[0].TakeItem(HoverSl);
                    HoverSl.Item = null;
                    break;
            }
            RefreshStats();
            return;
        }

        if (Input.GetMouseButtonDown(2)) //use consumables from inventory
        {
            if (carryScript.gameObject.activeInHierarchy || 
                HoverSl == null || HoverSl.Item == null || 
                HoverSl.Item.itemType != ItemType.Consumable)
            {
                return;
            }

            gm.StatManager.UsableItemsUsage(HoverSl.Item);
            gm.DisplayInv("You've used " + HoverSl.Item.Name);

            HoverSl.StackNum--;
            if (HoverSl.StackNum <= 0)
            {
                HoverSl.Item = null;
            }

            RefreshStats();
            return;
        }

    }

    #region//MISC
    public void RemoveItemFromInventory(InvIndividual inv)
    {
        gm.SpawnItemsManager.DropItem("", inv);
        inv.StackNum--;
        if (inv.StackNum <= 0)
        {
            inv.Item = null;
        }
        RefreshStats();
    }

    public void Btn_Split_OK()
    {
        if(!(splitSlider.value == 0 || AllSlotsWithItem(null).Count == 0))
        {
            invsToBeSplit[1] = AllSlotsWithItem(null)[0];
            invsToBeSplit[1].Item = invsToBeSplit[0].Item;
            invsToBeSplit[1].StackNum = _splitNumber[1];
            invsToBeSplit[0].StackNum = _splitNumber[0];
        }
        Btn_Split_Cancel();
    }
    public void Btn_Split_Cancel()
    {
        splitScreen.SetActive(false);
    }
    public void Slider_Split()
    {
        if (prevValue < splitSlider.value)
        {
            // print("porast");
            _splitNumber[0]--;
            _splitNumber[1]++;
            for (int i = 0; i < 2; i++)
            {
                _splitNumber[i] = Mathf.Clamp(_splitNumber[i], 0, invsToBeSplit[0].StackNum);
                splitTexts[i].text = _splitNumber[i].ToString();
            }

        }
        else if (prevValue > splitSlider.value)
        {
            // print("smanjenje");
            _splitNumber[0]++;
            _splitNumber[1]--;
            for (int i = 0; i < 2; i++)
            {
                _splitNumber[i] = Mathf.Clamp(_splitNumber[i], 0, invsToBeSplit[0].StackNum);
                splitTexts[i].text = _splitNumber[i].ToString();
            }


        }

        prevValue = splitSlider.value;
    }

    public void Btn_CloseWindow(int a)
    {
        _canvases[a].enabled = false;
        _buttonsMain[a].SetActive(true);
        FinalWindowButton();
    }

    public void Btn_OpenWindows(int a)
    {
        _canvases[a].enabled = true;
        _buttonsMain[a].SetActive(false);
        FinalWindowButton();
    }
    
    private void FinalWindowButton()
    {
        if (!OpenWindows())
        {
            // InventoryUtilities.FinishInvExchanges?.Invoke();
            RefreshStats();
        }
    }
    #endregion

}

