using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text number;

    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
}
