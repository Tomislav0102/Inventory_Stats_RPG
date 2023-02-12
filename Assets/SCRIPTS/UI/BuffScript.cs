using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TK.Enums;

public class BuffScript : MonoBehaviour
{
    GameManager gm;
    [SerializeField] Image imgFill, imgBuff;
    public SO_items Item;
    float _timer, _timerTick, _timerRampUp;
    float trajanje;
    SpendableType spendable;
    Stats affectedStat;
    int affectedValue;
    float affectedPercentage;
    Color colGreen = Color.green;
    Color colRed = Color.red;
    bool canBeginUpdating;

    private void Awake()
    {
        gm = GameManager.gm;
    }
    private void OnEnable()
    {
        if (Item == null || Item.useType != UseType.StatChange)
        {
            gameObject.SetActive(false);
        }

        imgBuff.sprite = Item.Sprite;
        trajanje = Item.buffTime + Item.rampUpTime;
        _timer = trajanje;
        _timerRampUp = Item.rampUpTime;
        imgFill.fillAmount = 1f;
        imgFill.color = colGreen;
        affectedStat = Item.statToChange;

        if (Item.hasTickBehaviour)
        {
            if (((int)affectedStat - 7) < 0)
            {
                gameObject.SetActive(false);
            }
            spendable = (SpendableType)((int)affectedStat - 7);
            _timerTick = float.MaxValue;
            canBeginUpdating = true;
            return;
        }
        StartCoroutine(BuffStarts());
    }

    private void OnDisable()
    {
        gm.StatManager.RefreshDisplays();
    }

    IEnumerator BuffStarts()
    {
        float bv;
        while(_timerRampUp > 0f)
        {
            _timerRampUp -= Time.deltaTime;

            if ((int)affectedStat < 5) 
            {
                bv = StatManager.Abilities[affectedStat].buffValue;
                bv = Mathf.Lerp(bv, Item.increaseValue, (Item.rampUpTime - _timerRampUp) / Item.rampUpTime);
                StatManager.Abilities[affectedStat].buffValue = (int)bv;
            }
            else 
            {
                bv = StatManager.Derivatives[affectedStat].buffValue;
                bv = Mathf.Lerp(bv, Item.increaseValue, (Item.rampUpTime - _timerRampUp) / Item.rampUpTime);
                StatManager.Derivatives[affectedStat].buffValue = (int)bv;
            }

            yield return null;
        }
        canBeginUpdating = true;
        if ((int)affectedStat < 5) //ability
        {
            if(Mathf.Approximately(Item.rampUpTime, 0f)) StatManager.Abilities[affectedStat].buffValue += Item.increaseValue;
            StatManager.Abilities[affectedStat].modifier += Item.increasePercentage;
        }
        else //derivative
        {
            if (Mathf.Approximately(Item.rampUpTime, 0f)) StatManager.Derivatives[affectedStat].buffValue += Item.increaseValue;
            StatManager.Derivatives[affectedStat].modifier += Item.increasePercentage;
        }
    }

    private void Update()
    {
        if (!canBeginUpdating)
        {
            return;
        }
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            imgFill.fillAmount = _timer / trajanje;
            imgFill.color = Color.Lerp(colRed, colGreen, _timer / trajanje);

            _timerTick += Time.deltaTime;
            if (Item.hasTickBehaviour && _timerTick > Item.tickRateOfFire)
            {
                _timerTick = 0f;
                string textToDislay= (spendable == SpendableType.HitPoints ? "Healed for " : "Mana recovered by ") +
                    Item.increaseValue + " points.";
                gm.DisplayMainInfo(null, textToDislay);
                print(textToDislay);
                gm.StatManager.TakeDamage(-Item.increaseValue, spendable);
            }
        }
        else
        {
            if (Item.hasTickBehaviour)
            {
                gameObject.SetActive(false);
            }

            if ((int)affectedStat < 5) //ability
            {
                StatManager.Abilities[affectedStat].buffValue -= Item.increaseValue;
                StatManager.Abilities[affectedStat].modifier -= Item.increasePercentage;
            }
            else //derivative
            {
                StatManager.Derivatives[affectedStat].buffValue -= Item.increaseValue;
                StatManager.Derivatives[affectedStat].modifier -= Item.increasePercentage;
            }
            gameObject.SetActive(false);
        }
    }


}
