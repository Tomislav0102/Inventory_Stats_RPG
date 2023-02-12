using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK;
using TK.Enums;
using TMPro;


public class PickUpTool : MonoBehaviour
{
    GameManager gm;
    Transform _myTranform;
    [SerializeField] TMP_Dropdown _dropdown;
    [SerializeField] BoxCollider2D _boxCol;
    PickUpType _pUtype;
    PickUpType PickUpType
    {
        get
        {
            return _pUtype;
        }
        set
        {
            _pUtype = value;
            switch (_pUtype)
            {
                case PickUpType.MouseClick:
                    _boxCol.enabled = false;
                    break;
                case PickUpType.WalkOver:
                    _boxCol.enabled = true;
                    break;
                case PickUpType.WalkNear:
                    _boxCol.enabled = false;
                    break;
                case PickUpType.WalkNearRange:
                    _boxCol.enabled = false;
                    break;
            }
        }
    }
    Vector2 MousePos()
    {
        return gm.MainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    Vector2 MoveDirectionPos()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(hor, ver);
        dir.Normalize();
        dir *= 1.5f;
        return new Vector2(_myTranform.position.x + dir.x, _myTranform.position.y + dir.y);
    }
    Collider2D _pickUpCollider;
    bool _aktiv = true;

    private void Awake()
    {
        gm = GameManager.gm;
        _myTranform = transform;
    }
    private void Start()
    {
        DropDwn_ChoosePickUpType();
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

    private void Update()
    {
        if (!_aktiv || gm.invManager.OpenWindows())
        {
            return;
        }

        switch (PickUpType)
        {
            case PickUpType.MouseClick:
                if (Input.GetMouseButtonDown(0))
                {
                    _pickUpCollider = Physics2D.OverlapPoint(MousePos(), gm.L_pickups);
                    PickUpAction(_pickUpCollider);
                }
                break;
        }

    }
    private void FixedUpdate()
    {
        if (!_aktiv)
        {
            return;
        }


        switch (PickUpType)
        {
            case PickUpType.WalkNear:
                _pickUpCollider = Physics2D.OverlapCircle(_myTranform.position, 1.5f, gm.L_pickups);
                PickUpAction(_pickUpCollider);
                break;
            case PickUpType.WalkNearRange:
                _pickUpCollider = Physics2D.OverlapCircle(MoveDirectionPos(), 1.5f, gm.L_pickups);
                PickUpAction(_pickUpCollider);
                break;
        }

    }
    void PickUpAction(Collider2D collider2D)
    {
        if (collider2D == null)
        {
            return;
        }
        if (collider2D.TryGetComponent(out PickUpIndividual pick))
        {
            if (pick.CanBePickedUp) PickUpMain(pick);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PickUpType != PickUpType.MouseClick)
        {
            if (collision.TryGetComponent(out PickUpIndividual pu))
            {
                if (pu.CanBePickedUp) PickUpMain(pu);
            }

        }
    }

    public void DropDwn_ChoosePickUpType()
    {
        PickUpType = (PickUpType)_dropdown.value;
    }

    void PickUpMain(PickUpIndividual pu)
    {
        if (pu.Item == null)
        {
            return;
        }

        if (pu.Item.itemType == ItemType.PermanentUsage)
        {
            gm.StatManager.UsableItemsUsage(pu.Item);
            gm.DisplayMainInfo(pu.Item, string.Empty);
            pu.gameObject.SetActive(false);
        }
        else
        {
            gm.invManager.FindSpaceForItem(pu);
        }
    }



}
