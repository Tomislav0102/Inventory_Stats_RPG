using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK;
using TK.Enums;

public class EffectsTesting : MonoBehaviour
{
    [SerializeField] StatsAbilities statsAbilities;

    [SerializeField] bool addBuff = true;
    [SerializeField] int buffNUm;
    Effect BullsStrength = new Effect(); //+str
    Effect GiantsStrength = new Effect(); //+str
    Effect CatsGrace = new Effect(); //+dex
    Effect SpellDodge = new Effect(); //+ac
    Effect SpellWeakness = new Effect(); //-str
    Effect SpellFragility = new Effect(); //-con
    Effect SpellFumble = new Effect(); //-dex
    Effect ConditionFlatFooted = new Effect(); //can't use dex for ac (other ac bonuses remain, ie armor)
    List<Effect> allEff = new List<Effect>();

    private void Awake()
    {
        BullsStrength.nameOfEffect = "Bull's strength";
        BullsStrength.affectedStat = Stats.Strength;
        BullsStrength.value = 4;
        GiantsStrength.nameOfEffect = "GiantsStrength";
        GiantsStrength.affectedStat = Stats.Strength;
        GiantsStrength.value = 10;
        CatsGrace.nameOfEffect = "CatsGrace";
        CatsGrace.affectedStat = Stats.Dextrerity;
        CatsGrace.value = 4;
        SpellDodge.nameOfEffect = "SpellDodge";
        SpellDodge.affectedStat = Stats.ArmorClass;
        SpellDodge.value = 8;
        SpellWeakness.nameOfEffect = "SpellWeakness";
        SpellWeakness.affectedStat = Stats.Strength;
        SpellWeakness.value = -6;
        allEff.Add(BullsStrength);
        allEff.Add(GiantsStrength);
        allEff.Add(CatsGrace);
        allEff.Add(SpellDodge);
        allEff.Add(SpellWeakness);
    }
    public void GenerateBuff()
    {
        if ((int)allEff[buffNUm].affectedStat < 5) statsAbilities.AzurAbility(addBuff, allEff[buffNUm]);
        else statsAbilities.AzurDerivative(addBuff, allEff[buffNUm]);
       // print((addBuff ? "Added" : "Removed") + " " + allEff[buffNUm].nameOfEffect);
    }
}
