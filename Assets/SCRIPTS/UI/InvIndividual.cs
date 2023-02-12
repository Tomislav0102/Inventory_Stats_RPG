using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK.Enums;
using TK;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InvIndividual : TotalItem, IPointerEnterHandler, IPointerExitHandler
{
    GameManager gm;
    bool durabilitySpentWhileWalking;
    SO_items _item;
    public override SO_items Item
    {
        get => _item;
        set
        {
            _item = value;
            if (_item == null)
            {
                if (slotType == InvSlotType.Equipment)
                {
                    mainImg.sprite = gm.EquipmentSprites[(int)equipment];
                }
                else
                {
                    mainImg.sprite = gm.Empty;
                }
                StackNum = 0;
            }
            else
            {
                _inheritedDistance = gm.PlayerMove.disTotal;
                _durabilitySwitch = true;
                mainImg.sprite = _item.Sprite;
                if (!Item.stackable)
                {
                    StackNum = 1;
                }
            }
        }
    }
    int _stackNum;
    public override int StackNum
    {
        get => _stackNum;
        set
        {
            _stackNum = value;
            stackDisplay.text = string.Empty;
            if (Item == null)
            {
                _stackNum = 0;
                return;
            }

            if (_stackNum == 0)
            {
                Item = null;
                return;
            }

            if (Item.stackable)
            {
                if (Item.stackMax == 999)
                {
                    stackDisplay.text = _stackNum.ToString();
                }
                else
                {
                    stackDisplay.text = _stackNum.ToString() + "/" + Item.stackMax.ToString();
                }
            }
            else if (_stackNum > 1)
            {
                _stackNum = 1;
            }

        }
    }
    int _currentDurability;
    public override int CurrentDurability
    {
        get => _currentDurability;
        set
        {
            _currentDurability = value;
            if (_durabilitySwitch)
            {
                _inheritedDurability = _currentDurability;
                _durabilitySwitch = false;
            }
            if (_currentDurability <= 0 && Item != null)
            {
                gm.DisplayMainInfo(null, Item.Name + " has broken from too much use.");
                Item = null;
            } 
        }
    }

    [SerializeField] TextMeshProUGUI stackDisplay;
    [SerializeField] Image mainImg, hlImg;
    public InvSlotType slotType;
    [HideInInspector] public EquipmentType equipment;

    float _inheritedDistance;
    int _inheritedDurability;
    bool _durabilitySwitch;

    void Awake()
    {
        gm = GameManager.gm;
        durabilitySpentWhileWalking = gm.durabilitySpentWhileWalking;
    }
    void Start()
    {
        Item = null;
        if (slotType == InvSlotType.Inventory && Random.value < 0.3f)
        {
           // Item = gm.AllItems[Random.Range(0, 8)];
           // Item = gm.AllItems[Random.Range(0, gm.AllItems.Length)];
             Item = gm.AllItems[2];
            CurrentDurability = Item.durablity;
            StackNum = 20;
        }
    }

    void Update()
    {
        if (!durabilitySpentWhileWalking || Item == null || slotType == InvSlotType.Inventory) return;

        CurrentDurability = _inheritedDurability - (int)(gm.PlayerMove.disTotal - _inheritedDistance);
    }



    #region//UNITY.EVENTSYSTEM
    public void OnPointerEnter(PointerEventData eventData)
    {
        hlImg.enabled = true;
        gm.invManager.HoverSl = this;
        if (Item != null)
        {
            if (slotType == InvSlotType.Inventory)
            {
                gm.DisplayInv("Name: " + Item.Name + "\n" + "Type: " + Item.itemType.ToString() + "\n" + Item.Description);
            }

            gm.DisplayEquipment(this, Item.itemType == ItemType.Wearable ? Item : null);
        }
        else
        {
            gm.DisplayEquipment(this, null);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hlImg.enabled = false;
        gm.invManager.HoverSl = null;
        gm.DisplayEquipment(this, null);
        gm.DisplayInv("");
    }


    #endregion
}
