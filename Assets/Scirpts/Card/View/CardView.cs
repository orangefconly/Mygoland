using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text mana;

    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;

    private Vector3 dragStartPosition;
    private Quaternion dragRotation;
    public Card Card { get; private set; }

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;
    }
    private void OnMouseEnter()
    {
        if (!CardInteraction.Instance.PlayerCanHover())
            return;
        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, transform.position.y+0.5f, 0);
        CardViewHoverSystem.Instance.Show(Card, pos);
    }

    private void OnMouseExit()
    {
        if (!CardInteraction.Instance.PlayerCanHover())
            return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }
    private void OnMouseDown()
    {
        if (!CardInteraction.Instance.PlayerCanInteract()) return;
        CardInteraction.Instance.PlayerIsDragging = true;
        wrapper.SetActive(true);
        CardViewHoverSystem.Instance.Hide();
        dragRotation = transform.rotation;
        dragStartPosition = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }
    private void OnMouseDrag()
   {
        if (!CardInteraction.Instance.PlayerCanInteract()) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }
    private void OnMouseUp()
    {
        if (!CardInteraction.Instance.PlayerCanInteract()) return;
        if (Physics.Raycast(transform.position,Vector3.forward,out RaycastHit hit , 10f))
        {

        }
        else
        {
            transform.position = dragStartPosition;
            transform.rotation = dragRotation;
        }
        CardInteraction.Instance.PlayerIsDragging =false;
    }
   
}
