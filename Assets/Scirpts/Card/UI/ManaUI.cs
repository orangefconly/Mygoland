using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private TMP_Text mana;
    public void UpdateManaText(int currentMana)
    {
        mana.text = currentMana.ToString();
    }
    public void LackManaWarning()
    {
        Color originalColor = new Color(1f, 1f, 1f); 

        Color targetRed = new Color(1f, 0.2f, 0.2f); // ÉÔÁÁµÄºìÉ«

        mana.DOKill();

        mana.DOColor(targetRed, 0.1f)
            .OnComplete(() =>
            {
                mana.DOColor(originalColor, 0.1f).SetDelay(1f);
            });
    }
}
