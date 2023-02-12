using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TK.Enums;
using TK;

public class StatManager : MonoBehaviour
{
    GameManager gm;
    [SerializeField] Transform parentBuffDisplays;
    BuffScript[] buffScripts;
    public static Dictionary<Stats, AbilitiesValue> Abilities = new Dictionary<Stats, AbilitiesValue>();
    public static Dictionary<Stats, DerivativesValue> Derivatives = new Dictionary<Stats, DerivativesValue>();
    [SerializeField] TextMeshProUGUI[] disValue;
    public class AbilitiesValue
    {
        public Stats typeOfAbility;
        public int baseValue;
        public int magicalItemValue;
        public int buffValue;
        public float modifier = 1;

        public int MainValue()
        {
            return Mathf.RoundToInt(baseValue * modifier + magicalItemValue + buffValue);
        }
        public int EffectiveValue()
        {
            int oddNumberModifier = MainValue() % 2 == 0 ? 0 : 1;
            int mod = ((MainValue() - oddNumberModifier) / 2) - 5;

            return mod;
        }
    }

    public class DerivativesValue
    {
        public Stats typeOfDerivative;
        public int baseValue;
        int _baseStartValue;
        public int magicalItemValue;
        public int buffValue;
        int _governAblity;
        int _governAbilityModifier = 1;
        public float modifier = 1;
        int _luckModifier;
        public int MainValue()
        {
            switch (typeOfDerivative)
            {
                case Stats.Damage:
                    _baseStartValue = 0;
                    _governAblity = Abilities[Stats.Strength].EffectiveValue();
                    _governAbilityModifier = 1;
                    break;
                case Stats.ArmorClass:
                    _baseStartValue = 0;
                    _governAblity = Abilities[Stats.Dextrerity].EffectiveValue();
                    _governAbilityModifier = 1;
                    break;
                case Stats.HitPoints:
                    _baseStartValue = 100;
                    _governAblity = Abilities[Stats.Constitution].EffectiveValue();
                    _governAbilityModifier = 10;
                    break;
                case Stats.ManaPoints:
                    _baseStartValue = 100;
                    _governAblity = Abilities[Stats.Intelligence].EffectiveValue();
                    _governAbilityModifier = 10;
                    break;
            }

            _luckModifier = Abilities[Stats.Luck].EffectiveValue();

            return Mathf.RoundToInt(_governAbilityModifier * (_luckModifier + _governAblity) +
                baseValue * modifier +
                _baseStartValue +
                magicalItemValue +
                buffValue);
        }


    }

    public Spendables spendables;
    [System.Serializable]
    public class Spendables
    {
        [HideInInspector] public int currHealth;
        [HideInInspector] public int currMana;
        [SerializeField] TextMeshProUGUI[] _sDisplays, _cDisplays;
        [SerializeField] Image[] _sImages;

        public void DisplayHealthMana(int maxHP, int maxMP)
        {
            _sDisplays[0].text = _cDisplays[0].text = currHealth.ToString() + "/" + maxHP.ToString();
            _sDisplays[1].text = _cDisplays[1].text = currMana.ToString() + "/" + maxMP.ToString();
            _sImages[0].fillAmount = (float)currHealth / (float)maxHP;
            _sImages[1].fillAmount = (float)currMana / (float)maxMP;
        }


    }


    private void Awake()
    {
        gm = GameManager.gm;
        buffScripts = InventoryUtilities.GetChildByType<BuffScript>(parentBuffDisplays);
    }
    private void Start()
    {
        InitializationOfStats();

        TakeDamage(-50, SpendableType.HitPoints);
        TakeDamage(-50, SpendableType.ManaPoints);

        RefreshDisplays();
    }

    private void Update()
    {
     //   if (Input.GetKeyDown(KeyCode.H)) ShowAnyStat(Stats.Luck);

        for (int i = 0; i < buffScripts.Length; i++)
        {
            if (buffScripts[i].gameObject.activeInHierarchy)
            {
                RefreshDisplays();
                return;
            }
        }
    }

    void ShowAnyStat(Stats statToDisplay)
    {
        print("baseValue " + Abilities[statToDisplay].baseValue);
        print("magicalItemValue " + Abilities[statToDisplay].magicalItemValue);
        print("buffValue " + Abilities[statToDisplay].buffValue);
        print("modifier " + Abilities[statToDisplay].modifier);
        print("MainValue() " + Abilities[statToDisplay].MainValue());
    }
    private void InitializationOfStats()
    {
        Abilities.Clear();
        Derivatives.Clear();

        Abilities.Add(Stats.Strength, new AbilitiesValue());
        Abilities.Add(Stats.Dextrerity, new AbilitiesValue());
        Abilities.Add(Stats.Constitution, new AbilitiesValue());
        Abilities.Add(Stats.Intelligence, new AbilitiesValue());
        Abilities.Add(Stats.Luck, new AbilitiesValue());
        for (int i = 0; i < 5; i++)
        {
            Abilities[(Stats)i].typeOfAbility = (Stats)i;
            Abilities[(Stats)i].baseValue = UnityEngine.Random.Range(6, 25);
            Abilities[(Stats)i].modifier = 1f;
        }

        Derivatives.Add(Stats.Damage, new DerivativesValue());
        Derivatives.Add(Stats.ArmorClass, new DerivativesValue());
        Derivatives.Add(Stats.HitPoints, new DerivativesValue());
        Derivatives.Add(Stats.ManaPoints, new DerivativesValue());
        for (int i = 5; i < 9; i++)
        {
            Derivatives[(Stats)i].typeOfDerivative = (Stats)i;
            Derivatives[(Stats)i].modifier = 1f;
        }
    }

    public void TakeDamage(int damage, SpendableType expendableType)
    {
        switch (expendableType)
        {
            case SpendableType.HitPoints:
                spendables.currHealth -= damage;
                break;
            case SpendableType.ManaPoints:
                spendables.currMana -= damage;
                break;
        }

        int maxHealth = Derivatives[Stats.HitPoints].MainValue();
        int maxMana = Derivatives[Stats.ManaPoints].MainValue();
        if (spendables.currHealth > maxHealth)
        {
            spendables.currHealth = maxHealth;
        }
        else if (spendables.currHealth <= 0)
        {
            print("You're dead.");
        }
        spendables.currMana = Mathf.Clamp(spendables.currMana, 0, maxMana);

        spendables.DisplayHealthMana(maxHealth, maxMana);
    }

    public void ResetValues(ValueResetType valueReset)
    {
        for (int i = 0; i < 5; i++)
        {
            switch (valueReset)
            {
                case ValueResetType.MagicalItems:
                    Abilities[(Stats)i].magicalItemValue = 0;
                    break;
                case ValueResetType.Buffs:
                    Abilities[(Stats)i].buffValue = 0;
                    break;
                case ValueResetType.PrecentageModifiers:
                    Abilities[(Stats)i].modifier = 1f;
                    break;
            }
        }
        for (int i = 5; i < 9; i++)
        {
            switch (valueReset)
            {
                case ValueResetType.MagicalItems:
                    Derivatives[(Stats)i].magicalItemValue = 0;
                    break;
                case ValueResetType.Buffs:
                    Derivatives[(Stats)i].buffValue = 0;
                    break;
                case ValueResetType.PrecentageModifiers:
                    Derivatives[(Stats)i].modifier = 1f;
                    break;
            }
        }
    }
    public void RefreshDisplays()
    {
        for (int i = 0; i < 5; i++)
        {
            disValue[i].text = Abilities[(Stats)i].MainValue().ToString() + "    " + Abilities[(Stats)i].EffectiveValue().ToString();
        }
        for (int i = 5; i < 7; i++)
        {
            disValue[i].text = Derivatives[(Stats)i].MainValue().ToString();
        }

        spendables.DisplayHealthMana(Derivatives[Stats.HitPoints].MainValue(), Derivatives[Stats.ManaPoints].MainValue());
    }


    public void UsableItemsUsage(SO_items item)
    {
        switch (item.useType)
        {
            case UseType.Heal:
                TakeDamage(-item.useAmmout, SpendableType.HitPoints);
                break;
            case UseType.Mana:
                TakeDamage(-item.useAmmout, SpendableType.ManaPoints);
                break;
            case UseType.Gold:
                gm.DisplayGold(item.useAmmout);
                break;
            case UseType.StatChange:
                for (int i = 0; i < buffScripts.Length; i++)
                {
                    if (!buffScripts[i].gameObject.activeInHierarchy)
                    {
                        buffScripts[i].Item = item;
                        buffScripts[i].gameObject.SetActive(true);
                        return;
                    }
                }
                break;
        }
    }

}
