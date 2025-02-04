using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using System;


public enum HandRank
{
    None = 0,
    HighCard = 1,   
    Pair = 2,    
    TwoPair = 3,    
    ThreeOfAKind = 4, 
    Straight = 5,   
    Flush = 6,      
    FullHouse = 7,  
    FourOfAKind = 8,
    StraightFlush = 9,
    RoyalFlush = 10,
    FlushHouse = 11,
    FiveOfAKind = 12,
    FlushFive = 13
}

public struct HandValue
{
    public float chips;
    public float mult;

    public HandValue(float chips, float mult)
    {
        this.chips = chips;
        this.mult = mult;
    }
}

public class HandTypes : MonoBehaviour
{
    //royal flush daha eklenmedi. royal flush straight flushın değerini döndürmeli.
    //straight 5,4,3,2,ace değerinide doğru saymalı
    //flushhouse, fiveofakind ve flushfiveında chip ve mult değerleri atanmalı
    
    public static HandTypes instance;

    public Dictionary<HandRank, HandValue> handValues = new Dictionary<HandRank, HandValue>
    {
        {HandRank.HighCard, new HandValue(5f,1f)},
        {HandRank.Pair, new HandValue(10f,2f)},
        {HandRank.TwoPair, new HandValue(20f,2f)},
        {HandRank.ThreeOfAKind, new HandValue(30f,3f)},
        {HandRank.Straight, new HandValue(30f,4f)},
        {HandRank.Flush, new HandValue(35f,4f)},
        {HandRank.FullHouse, new HandValue(40f,4f)},
        {HandRank.FourOfAKind, new HandValue(60f,7f)},
        {HandRank.StraightFlush, new HandValue(100f,8f)}
        //{HandRank.FlushHouse, new HandValue(100f,8f)},
        //{HandRank.FiveOfAKind, new HandValue(100f,8f)},
        //{HandRank.FlushFive, new HandValue(100f,8f)}
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public HandRank EvaluateHand()
    {

        if (IsFlushFive()) return HandRank.FlushFive;
        if (IsFiveOfAKind()) return HandRank.FiveOfAKind;
        if (IsFlushHouse()) return HandRank.FlushHouse;
        //if (IsRoyalFlush()) return HandRank.RoyalFlush;
        if (IsStraightFlush()) return HandRank.StraightFlush;
        if (IsFourOfAKind()) return HandRank.FourOfAKind;
        if (IsFullHouse()) return HandRank.FullHouse;
        if (IsFlush()) return HandRank.Flush;
        if (IsStraight()) return HandRank.Straight;
        if (IsThreeOfAKind()) return HandRank.ThreeOfAKind;
        if (IsTwoPair()) return HandRank.TwoPair;
        if (IsPair()) return HandRank.Pair;

        return HandRank.HighCard;
    }

    public void AddValuesOfHandTypes(HandRank hand, float newChips, float newMult)
    {
        HandValue currentHandValue = handValues[hand];

        currentHandValue.chips += newChips;
        currentHandValue.mult += newMult;

        handValues[hand] = currentHandValue;
    }

    #region Types
    
    public bool IsHighCard()
    {
        return DeckManager.instance.selectedCards.Count > 0;
    }

    public bool IsPair()
    {
        return DeckManager.instance.selectedCards
            .GroupBy(card => card.GetComponent<CardScript>().cardId)
            .Any(group => group.Count() == 2);
    }

    public bool IsTwoPair()
    {
        var grouped = DeckManager.instance.selectedCards
            .GroupBy(x => x.GetComponent<CardScript>().cardId)
            .Select(g => g.Count())
            .OrderByDescending(x => x)
            .ToList();

        return grouped.SequenceEqual(new List<int> { 2, 2, 1 }) || grouped.SequenceEqual(new List<int> { 2, 2 });
    }

    public bool IsThreeOfAKind()
    {
        return DeckManager.instance.selectedCards
            .GroupBy(card => card.GetComponent<CardScript>().cardId)
            .Any(group => group.Count() == 3);
    }

    public bool IsStraight()
    {
        if (DeckManager.instance.selectedCards.Count != DeckManager.instance.selectedHandSize)
        {
            return false;
        }

        var sortedCards = DeckManager.instance.selectedCards
            .Select(card => card.GetComponent<CardScript>().cardId)
            .OrderBy(id => id)
            .ToList();

        for (int i = 0; i < sortedCards.Count - 1; i++)
        {
            if (sortedCards[i] + 1 != sortedCards[i + 1])
            {
                return false;
            }
        }

        return true;
    }

    public bool IsFlush()
    {
        if (DeckManager.instance.selectedCards.Count != DeckManager.instance.selectedHandSize)
        {
            return false;
        }

        var firstCardType = DeckManager.instance.selectedCards[0].GetComponent<CardScript>().cardType;

        return DeckManager.instance.selectedCards.All(card => card.GetComponent<CardScript>().cardType == firstCardType);
    }

    public bool IsFullHouse()
    {
        var grouped = DeckManager.instance.selectedCards
            .GroupBy(x => x.GetComponent<CardScript>().cardId)
            .Select(g => g.Count())
            .OrderByDescending(x => x)
            .ToList();

        return grouped.SequenceEqual(new List<int> { 3, 2 });
    }

    public bool IsFourOfAKind()
    {
        return DeckManager.instance.selectedCards
            .GroupBy(card => card.GetComponent<CardScript>().cardId)
            .Any(group => group.Count() == 4);
    }

    public bool IsStraightFlush()
    {
        return IsFlush() && IsStraight(); 
    }

    public bool IsFlushHouse()
    {
        return IsFlush() && IsFullHouse();
    }

    public bool IsFiveOfAKind()
    {
        return DeckManager.instance.selectedCards
            .GroupBy(card => card.GetComponent<CardScript>().cardId)
            .Any(group => group.Count() == 5);
    }

    public bool IsFlushFive()
    {
        return IsFlush() && IsFiveOfAKind();
    }

    #endregion
}

/*
    public bool IsThreeOfaKind()
    {
        if (selectedCards.Count < 3)
        {
            return false;
        }

        for (int i = 0; i < selectedCards.Count - 1; i++)
        {
            if (selectedCards[i].GetComponent<CardScript>().cardId == selectedCards[i + 1].GetComponent<CardScript>().cardId 
                && selectedCards[i].GetComponent<CardScript>().cardId == selectedCards[i + 2].GetComponent<CardScript>().cardId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public bool IsPair()
    {
        for (int i = 0; i < selectedCards.Count - 1; i++)
        {
            if (selectedCards[i].GetComponent<CardScript>().cardId == selectedCards[i + 1].GetComponent<CardScript>().cardId)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsStraight()
    {
        selectedCards = selectedCards
            .OrderBy(card => card.GetComponent<CardScript>().cardId)
            .ToList();
    
        if (selectedCards.Count != 5)
        {
            return false;
        }

        for (int i = 0; i < selectedCards.Count - 1; i++)
        {
            if (selectedCards[i].GetComponent<CardScript>().cardId + 1 != selectedCards[i + 1].GetComponent<CardScript>().cardId)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsFlush()
    {
        if (selectedCards.Count != selectedHandSize)
        {
            return false;
        }

        CardType firsCardType = selectedCards[0].GetComponent<CardScript>().cardType;

        foreach (GameObject item in selectedCards)
        {
            CardType currentCardType = item.GetComponent<CardScript>().cardType;

            if(!EqualityComparer<CardType>.Default.Equals(firsCardType, currentCardType))
            {
                return false;
            }
        }

        return true;
    }
*/