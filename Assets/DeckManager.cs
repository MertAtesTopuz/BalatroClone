using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using TMPro;
using System;

public class DeckManager : MonoBehaviour
{
    //ilerleyen zamanlarda eldeki kombinasyonların doğruluğunu kontrol etmek için ayrı bir handmanager isminde kod yazılabilir.
    //öyle bir durumda bu kod sadece veri depolama ve card oluşturup yok etme görevi üstlenir.
    //hand type scriptine el tipleri eklendi
    //ayrıca kart yaratma aşamalarında object pooling yapmalıyım.

    public static DeckManager instance;

    public List<CardsSO> mainDeck = new List<CardsSO>();
    public List<Card> cardsInDeck = new List<Card>();
    public List<Card> cardsInHand = new List<Card>();
    public List<Card> cardsOutDeck = new List<Card>();
    public List<GameObject> cardObjects = new List<GameObject>();
    public List<GameObject> selectedCards = new List<GameObject>();

    public GameObject cardPnl;
    public GameObject cardPrefab;

    public int selectedHandSize = 5;

    public event Action discardEvent;

    public TextMeshProUGUI typeTxt;
    public TextMeshProUGUI chipsTxt;
    public TextMeshProUGUI multTxt;
    public TextMeshProUGUI generalPointTxt;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        LoadCardsFromScriptableObjects();
        CardCreator();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            cardObjects[0].GetComponent<CardScript>().card.cardId = 5;
        }
    }

    private void LoadCardsFromScriptableObjects()
    {
        foreach (CardsSO cardSO in mainDeck)
        {
            Card card = new Card(cardSO.cardName, cardSO.cardType, cardSO.cardSuit, cardSO.point, cardSO.sprite, cardSO.id);
            cardsInDeck.Add(card);
        }
    }

    private void CardCreator()
    {
        for (int i = 0; i < 8; i++)
        {
           GameObject createdCardPrefab = Instantiate(cardPrefab, cardPnl.transform);
           cardObjects.Add(createdCardPrefab);
           CardScript cardScript = createdCardPrefab.GetComponent<CardScript>();

           if (cardScript != null)
           {
                int cardIndex = UnityEngine.Random.Range(0, cardsInDeck.Count);
                cardScript.card = cardsInDeck[cardIndex];
                cardsInHand.Add(cardsInDeck[cardIndex]);
                cardsInDeck.RemoveAt(cardIndex);
           }
        }
    }

    public void DiscardBtn()
    {

        foreach (GameObject item in selectedCards)
        {
           CardScript cardScript = item.GetComponent<CardScript>();
           cardsOutDeck.Add(cardScript.card);

           if (cardScript != null)
           {
                int cardIndex = UnityEngine.Random.Range(0, cardsInDeck.Count);
                cardScript.card = cardsInDeck[cardIndex];
                cardsInHand.Add(cardsInDeck[cardIndex]);
                cardsInDeck.RemoveAt(cardIndex);
           }
        }
        discardEvent?.Invoke();
    }

    public void PlayBtn()
    {
        HandRank handRank = HandTypes.instance.EvaluateHand();
        float currentChips = HandTypes.instance.handValues[handRank].chips;

        foreach (GameObject item in selectedCards)
        {
            CardScript cardScript = item.GetComponent<CardScript>();
            currentChips += cardScript.point;
            print(cardScript.point);
        }

        float generalPoint = currentChips * HandTypes.instance.handValues[handRank].mult;
        generalPointTxt.text = generalPoint.ToString();
    }

    public void ValuePrinter()
    {
        HandRank handRank = HandTypes.instance.EvaluateHand();

        typeTxt.text = handRank.ToString();
        chipsTxt.text = HandTypes.instance.handValues[handRank].chips.ToString();
        multTxt.text = HandTypes.instance.handValues[handRank].mult.ToString();
    }

}

