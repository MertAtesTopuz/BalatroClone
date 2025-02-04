using UnityEngine;

[CreateAssetMenu(fileName = "Cards", menuName = "Scriptable Objects/CardsSO")]
public class CardsSO : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public CardSuit cardSuit;
    public int point;
    public Sprite sprite;
    public int id;
}

public enum CardType
{
    Spade,
    Heart,
    Diamond,
    Club
}

public enum CardSuit
{
    Ace,
    Numbered,
    Face
}
