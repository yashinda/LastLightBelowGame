using System;
using TMPro;
using UnityEngine;

public class ShopAfterDeath : MonoBehaviour
{
    public TMP_Text textEchoSvetlesAmount;
    public TMP_Text textReincarnationAmount;
    public int reincarnationCost = 1200;

    private void Update()
    {
        textEchoSvetlesAmount.text = EchoSvetles.Amount.ToString();
        textReincarnationAmount.text = SkillsAfterDeath.ReincarnationAmount.ToString();
        if (SkillsAfterDeath.ReincarnationAmount == 0)
            textReincarnationAmount.color = Color.red;
        else
            textReincarnationAmount.color = Color.white;
    }

    public void OnReincarnationBuy()
    {
        if (EchoSvetles.Amount < reincarnationCost)
            return;
        
        EchoSvetles.Spend(reincarnationCost);
        SkillsAfterDeath.AddReincarnation();
    }
}
