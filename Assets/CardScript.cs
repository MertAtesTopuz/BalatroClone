using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CardScript : MonoBehaviour
{
    public Card card;

    public string cardName;
    public CardType cardType;
    public CardSuit cardSuit;
    public int point;
    public Sprite sprite;
    public int cardId;

    private bool isSelected;

    [Header("UI")]
    private Image mainImage;

    void Start()
    {

        DeckManager.instance.discardEvent += CardValueUpdate;
        mainImage = GetComponent<Image>();
        CardValueUpdate();
    }

    void Update()
    {
        
    }

    public void SelectCard()
    {
        if (!isSelected && DeckManager.instance.selectedCards.Count < DeckManager.instance.selectedHandSize)
        {
            isSelected = true;
            DeckManager.instance.selectedCards.Add(gameObject);
            transform.DOMoveY(transform.position.y + 20f , .5f);
            DeckManager.instance.ValuePrinter();
        }
        else if(isSelected)
        {
            isSelected = false;
            DeckManager.instance.selectedCards.Remove(gameObject);
            transform.DOMoveY(transform.position.y - 20f , .5f);
            DeckManager.instance.ValuePrinter();
        }
    }

    private void CardValueUpdate()
    {
        cardName = card.cardName;
        cardType = card.cardType;
        cardSuit = card.cardSuit;
        point = card.point;
        sprite = card.sprite;
        cardId = card.cardId;
        mainImage.sprite = card.sprite;
    }
}
