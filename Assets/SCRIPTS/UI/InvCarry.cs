using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TK;

public class InvCarry : TotalItem
{
    GameManager gm;
    private Image _image;
    SO_items _item;
    public override SO_items Item
    {
        get => _item;
        set
        {
            _item = value;
            if (_item != null) _image.sprite = Item.Sprite;
            else gameObject.SetActive(false);
        }
    }
    int _stackNum;
    public override int StackNum
    {
        get => _stackNum;
        set
        {
            _stackNum = value;
            if (_stackNum <= 0) Item = null;
        }
    }
    
    void Awake()
    {
        gm = GameManager.gm;
        _image = GetComponent<Image>();
    }
    private void Start()
    {
        Item = null;
    }


    void Update()
    {
        transform.position = Input.mousePosition;
    }

}
