using System;
using TMPro;
using UnityEngine;

public class ShopAfterDeath : MonoBehaviour
{
    public TMP_Text textEchoSvetlesAmount;
    public TMP_Text textReincarnationAmount;
    public TMP_Text textBoostMoveSpeedPercent;
    
    public int reincarnationCost = 1200;
    public int moveSpeedCost = 600;
    public int revolverCost = 800;
    public int rifleCost = 1500;
    
    public GameObject revolver;
    public GameObject rifle;
    
    public WeaponChanger weaponChanger;

    private void Update()
    {
        textEchoSvetlesAmount.text = EchoSvetles.Amount.ToString();
        textReincarnationAmount.text = SkillsAfterDeath.ReincarnationAmount.ToString();
        textBoostMoveSpeedPercent.text = (SkillsAfterDeath.BoostMoveSpeed - 1.0f).ToString();
        if (SkillsAfterDeath.ReincarnationAmount == 0)
            textReincarnationAmount.color = Color.red;
        else
            textReincarnationAmount.color = Color.white;
    }

    public void OnMoveSpeedBuy()
    {
        var levelMoveSpeed = PlayerPrefs.GetFloat("BoostMoveSpeed");
        
        if (EchoSvetles.Amount < moveSpeedCost || levelMoveSpeed >= 1.25f)
            return;
        
        SkillsAfterDeath.AddBoostMoveSpeed();
    }

    public void OnRevolverBuy()
    {
        if (EchoSvetles.Amount < revolverCost)
            return;
        
        weaponChanger.AddWeapon(revolver);
    }
    
    public void OnRifleBuy()
    {
        if (EchoSvetles.Amount < rifleCost)
            return;
        
        weaponChanger.AddWeapon(rifle);
    }

    public void OnReincarnationBuy()
    {
        if (EchoSvetles.Amount < reincarnationCost)
            return;
        
        EchoSvetles.Spend(reincarnationCost);
        SkillsAfterDeath.AddReincarnation();
    }
}
