using UnityEngine;

public struct Card
{
    public string cardName;
    public CardType cardType;
    public CardSuit cardSuit;
    public int point;
    public Sprite sprite;
    public int cardId;

    public Card(string cardName, CardType cardType, CardSuit cardSuit, int point, Sprite sprite, int cardId)
    {
        this.cardName = cardName;
        this.cardType = cardType;
        this.cardSuit = cardSuit;
        this.point = point;
        this.sprite = sprite;
        this.cardId = cardId;
    }
} 
