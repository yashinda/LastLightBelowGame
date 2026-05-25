using System;
using TMPro;
using UnityEngine;

public class ShopAfterDeath : MonoBehaviour
{
    public TMP_Text textEchoSvetlesAmount;
    public TMP_Text textReincarnationAmount;
    public TMP_Text textBoostMoveSpeedPercent;
    public TMP_Text textBoostHPPercent;
    public TMP_Text textArmor;
    
    public int reincarnationCost = 1200;
    public int moveSpeedCost = 600;
    public int hpSpeedCost = 700;
    public int armorCost = 900;
    

    private void Update()
    {
        float percentMoveSpeed = (SkillsAfterDeath.BoostMoveSpeed - 1.0f) * 100;
        textEchoSvetlesAmount.text = EchoSvetles.Amount.ToString();
        textReincarnationAmount.text = SkillsAfterDeath.ReincarnationAmount.ToString();
        textBoostMoveSpeedPercent.text = Mathf.RoundToInt(percentMoveSpeed).ToString();
        textBoostHPPercent.text = Mathf.RoundToInt(SkillsAfterDeath.BoostMaxHp).ToString();
        textArmor.text = SkillsAfterDeath.BoostArmor.ToString();
        if (SkillsAfterDeath.ReincarnationAmount == 0)
            textReincarnationAmount.color = Color.red;
        else
            textReincarnationAmount.color = Color.white;
    }
    
    public void OnMoveSpeedBuy()
    {
        var levelMoveSpeed = SkillsAfterDeath.BoostMoveSpeed;
        
        if (EchoSvetles.Amount < moveSpeedCost || levelMoveSpeed >= 1.25f)
            return;
        
        EchoSvetles.Spend(moveSpeedCost);
        SkillsAfterDeath.AddBoostMoveSpeed();
    }
    
    public void OnReincarnationBuy()
    {
        var reincarnationAmount = SkillsAfterDeath.ReincarnationAmount;
        
        if (EchoSvetles.Amount < reincarnationCost || reincarnationAmount >= 2)
            return;
        
        EchoSvetles.Spend(reincarnationCost);
        SkillsAfterDeath.AddReincarnation();
    }
    
    public void OnBoostHPBuy()
    {
        var levelBoostHP = SkillsAfterDeath.BoostMaxHp;
        
        if (EchoSvetles.Amount < hpSpeedCost || levelBoostHP >= 25.0f)
            return;
        
        EchoSvetles.Spend(hpSpeedCost);
        SkillsAfterDeath.AddBoostMaxHP();
    }

    public void OnArmorBuy()
    {
        var levelArmor = SkillsAfterDeath.BoostArmor;
        
        if (EchoSvetles.Amount < armorCost ||  levelArmor >= 30)
            return;
        
        EchoSvetles.Spend(armorCost);
        SkillsAfterDeath.AddBoostArmor();
    }
}
