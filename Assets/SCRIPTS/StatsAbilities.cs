using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK.Enums;
using TK;
using TMPro;


public class StatsAbilities : MonoBehaviour
{
    GameManager gm;
    [SerializeField] TextMeshProUGUI[] displays;
    Effect[] mainStatEffects = new Effect[5];

    Dictionary<Stats, HashSet<Effect>> finalEffects = new Dictionary<Stats, HashSet<Effect>>();
    Dictionary<Stats, int> finalStats = new Dictionary<Stats, int>();
    int[] baseStatValues = new int[9];
    readonly int numberOfStats = System.Enum.GetNames(typeof(Stats)).Length - 1; //-1 beacuse of NA
    int ModAbilityValue(int val)
    {
        int oddNumberModifier = val % 2 == 0 ? 0 : 1;
        int mod = ((val - oddNumberModifier) / 2) - 5;

        return mod;
    }
    private void Awake()
    {
        Ini();
    }
    void Ini()
    {
        for (int i = 0; i < 5; i++)
        {
            baseStatValues[i] = Random.Range(6, 20);
        }
        baseStatValues[5] = baseStatValues[6] = 0;
        baseStatValues[7] = baseStatValues[8] = 100;
        for (int i = 0; i < 9; i++)
        {
            finalStats[(Stats)i] = baseStatValues[i];
        }

        finalEffects.Add(Stats.Strength, new HashSet<Effect>());
        finalEffects.Add(Stats.Dextrerity, new HashSet<Effect>());
        finalEffects.Add(Stats.Constitution, new HashSet<Effect>());
        finalEffects.Add(Stats.Intelligence, new HashSet<Effect>());
        finalEffects.Add(Stats.Luck, new HashSet<Effect>());
        finalEffects.Add(Stats.Damage, new HashSet<Effect>());
        finalEffects.Add(Stats.ArmorClass, new HashSet<Effect>());
        finalEffects.Add(Stats.HitPoints, new HashSet<Effect>());
        finalEffects.Add(Stats.ManaPoints, new HashSet<Effect>());

        for (int i = 0; i < mainStatEffects.Length; i++)
        {
            mainStatEffects[i] = new Effect();
            mainStatEffects[i].affectedStat = (Stats)(i + mainStatEffects.Length);
            mainStatEffects[i].value = ModAbilityValue(finalStats[(Stats)i]);
        }
        ApplyMainEffect();
        for (int i = 0; i < 4; i++)
        {
            AzurDerivative(true, mainStatEffects[i]);
        }

        DisplayStats();
    }

    void DisStat(Stats st)
    {
        foreach (Effect eff in finalEffects[st])
        {
            print(st + " has " + eff.nameOfEffect);
        }
    }
    public void AzurAbility(bool addEffect, Effect effect)
    {
        Stats stat = effect.affectedStat;
        if (addEffect)
        {
            if (finalEffects[stat].Contains(effect))
            {
                //effect is in place, need to restart duration (spell is cast while that same spell is active)
                return;
            }
            else
            {
                finalEffects[stat].Add(effect);
                //print("added");
                //DisStat(stat);
            }
        }
        else
        {
            if (!finalEffects[stat].Contains(effect)) return;
            
            finalEffects[stat].Remove(effect);
            //print("after removal");
            //DisStat(stat);

        }

        AzurTotal(StatType.Ability);
        ApplyMainEffect();
        AzurTotal(StatType.Derivative);
    }
    void AzurTotal(StatType statType)
    {
        int startNum = statType == StatType.Ability ? 0 : 5;
        int endNum = statType == StatType.Ability ? 5 : 9;
        for (int i = startNum; i < endNum; i++)
        {
            finalStats[(Stats)i] = baseStatValues[i];
            foreach (Effect eff in finalEffects[(Stats)i])
            {
                finalStats[(Stats)i] += eff.value;
                if (statType == StatType.Ability) mainStatEffects[i].value = ModAbilityValue(finalStats[(Stats)i]);
            }
        }
        DisplayStats();
    }
    void ApplyMainEffect()
    {
        for (int i = 5; i < 9; i++)
        {
            for (int j = 0; j < mainStatEffects.Length; j++)
            {
                if (finalEffects[(Stats)i].Contains(mainStatEffects[j]))
                {
                    finalEffects[(Stats)i].Remove(mainStatEffects[j]);
                }

                if (i < mainStatEffects.Length - 1 && mainStatEffects[i].affectedStat == (Stats)i) 
                    finalEffects[(Stats)i].Add(mainStatEffects[j]);
            }
            finalEffects[(Stats)i].Add(mainStatEffects[4]); //luck
        }
    }
    public void AzurDerivative(bool addEffect, Effect effect)
    {
        Stats stat = effect.affectedStat;
        if (addEffect)
        {
            if (finalEffects[stat].Contains(effect))
            {
                //effect is in place, need to restart duration (spell is cast while that same spell is active)
                return;
            }
            else
            {
                finalEffects[stat].Add(effect);
            }
        }
        else
        {
            if (!finalEffects[stat].Contains(effect)) return;

            finalEffects[stat].Remove(effect);
        }

        AzurTotal(StatType.Derivative);
    }
    void DisplayStats()
    {
        for (int i = 0; i < displays.Length; i++)
        {
            displays[i].text = finalStats[(Stats)i].ToString();
            if (i < 5) displays[i].text += "     " + ModAbilityValue(finalStats[(Stats)i]).ToString();
        }
    }
}
