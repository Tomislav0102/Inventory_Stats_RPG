using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK;
using TK.Enums;

public class SpawnItemsManager : MonoBehaviour
{
    GameManager gm;
    readonly Vector4 _spawnArea = new Vector4(-8f, 26f, -4f, 26f);
    [SerializeField] Transform _parPoolItems;
    Transform[] _pickUpsPool;
    int _counterItems;
    int _needSpawnNum = 100;
    int _currSpawnNum;

    private void Awake()
    {
        gm = GameManager.gm;
    }

    private void Start()
    {
        _pickUpsPool = InventoryUtilities.GetChildByType<Transform>(_parPoolItems);
        while (_currSpawnNum < _needSpawnNum)
        {
            SpawnItemOnGround(false, 1);
        }
        _needSpawnNum = 1;
        _currSpawnNum = 0;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gm._mainMenuOpen)
        {
            while (_currSpawnNum < _needSpawnNum)
            {
                SpawnItemOnGround(true, 1);
            }
            _currSpawnNum = 0;
        }

    }

    public void SpawnItemOnGround(bool displayInfo, int stack)
    {
        Vector2 poz = new Vector2(Random.Range(_spawnArea.x, _spawnArea.y), Random.Range(_spawnArea.z, _spawnArea.w));
        if (Physics2D.OverlapCircle(poz, 1f, gm.L_spawnArea))
        {
            return;
        }

        _currSpawnNum++;
        _pickUpsPool[_counterItems].position = poz;
        PickUpIndividual puInd = _pickUpsPool[_counterItems].GetComponent<PickUpIndividual>();
        puInd.Item = gm.AllItems[Random.Range(0, gm.AllItems.Length)];
      //  puInd.Item = gm.AllItems[Random.Range(0, 8)];
        puInd.CanBePickedUp = true;
        puInd.StackNum = stack > 0 ? stack : 1;
        puInd.CurrentDurability = puInd.Item.durablity;
        _pickUpsPool[_counterItems].gameObject.SetActive(true);
        if (displayInfo)
        {
            gm.DisplayMainInfo(null, puInd.Item.Name + " has apeared at " + poz);
        }
        _counterItems = (1 + _counterItems) % _pickUpsPool.Length;
    }
    public void DropItem(string extraText, TotalItem totalItem)
    {
        Vector2 poz = gm.PlTransform.position;
        _pickUpsPool[_counterItems].position = poz;
        PickUpIndividual puInd = _pickUpsPool[_counterItems].GetComponent<PickUpIndividual>();
        puInd.Item = totalItem.Item;
        puInd.CanBePickedUp = false;
        _pickUpsPool[_counterItems].gameObject.SetActive(true);
        puInd.StackNum = totalItem.StackNum; //if stack == 0 gameobject will be disabled, that's why its below the line that activates object
        puInd.CurrentDurability = totalItem.CurrentDurability;
        gm.DisplayMainInfo(null, extraText + "You've droped " + totalItem.Item.Name);
        _counterItems = (1 + _counterItems) % _pickUpsPool.Length;

    }


}
