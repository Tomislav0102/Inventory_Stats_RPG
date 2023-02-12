using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpIndividual :  TotalItem
{
    GameManager gm;
    SO_items _item;
    public override SO_items Item
    {
        get => _item;
        set
        {
            _item = value;
            if (_item == null) DestroyedObject("");
        }
    }
    int _stackNum;
    public override int StackNum
    {
        get => _stackNum;
        set
        {
            _stackNum = value;
            if (_stackNum <= 0) DestroyedObject("");
        }
    }
    int _currentDurability;
    public override int CurrentDurability
    {
        get => _currentDurability;
        set
        {
            _currentDurability = value;
            if (_currentDurability <= 0) DestroyedObject(Item != null ? Item.Name + " has been destroyed!" : "");
        }
    }
    [SerializeField] SpriteRenderer _sprite;
    [HideInInspector] public bool CanBePickedUp;
    Rigidbody2D _r2D;
    BoxCollider2D _box2D;

    private void Awake()
    {
        gm = GameManager.gm;
        _r2D = GetComponent<Rigidbody2D>();
        _box2D = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        if (Item != null)
        {
            _sprite.sprite = Item.Sprite;
        }

        if (!CanBePickedUp)
        {
            StartCoroutine(KickedOut());
        }
    }

    void DestroyedObject(string description)
    {
        if (description != "") gm.DisplayMainInfo(null, description);
        gameObject.SetActive(false);
    }
    IEnumerator KickedOut()
    {
        for (int i = 0; i < 2; i++)
        {
            Physics2D.IgnoreCollision(_box2D, gm.PlayerColliders[i], true);
        }
        _box2D.isTrigger = false;
        _r2D.isKinematic = false;
        Vector2 direction = (UnityEngine.Random.insideUnitCircle).normalized;
        _r2D.AddRelativeForce(4f * direction, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 2; i++)
        {
            Physics2D.IgnoreCollision(_box2D, gm.PlayerColliders[i], false);
        }
        _box2D.isTrigger = true;
        _r2D.velocity = Vector2.zero;
        _r2D.isKinematic = true;
        CanBePickedUp = true;
    }
}
