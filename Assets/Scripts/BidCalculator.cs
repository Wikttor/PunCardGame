using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BidCalculator
{
    public static int CalculateMaxBid(List<CardNetworked> cardSet)
    {
        int maxbid = 0;
        foreach (CardNetworked mainCard in cardSet)
        {

            int valueWithMainCard = CalculateMaxBidWithMainCard(cardSet, mainCard.cardSO);
            if (valueWithMainCard > maxbid)
            {
                maxbid = valueWithMainCard;
            }
        }
        return maxbid;
    }

    private static int CalculateMaxBidWithMainCard(List<CardNetworked> cardSet, TestCard mainCard)
    {
        int bid = mainCard.mainValue;
        List<TestCard> cardSetWithoutMain = new List<TestCard>();
        foreach (CardNetworked card in cardSet)
        {
            cardSetWithoutMain.Add(card.cardSO);
        }
        cardSetWithoutMain.Remove(mainCard);
        foreach (TestCard card in cardSetWithoutMain)
        {
            if (CheckCardEligibility(cardSet, mainCard, card))
            {
                bid += CalculateBonusValue(cardSet, mainCard, card);
            }
        }
        return bid;
    }

    private static bool CheckCardEligibility(List<CardNetworked> cardSet, TestCard mainCard, TestCard checkedCard)
    {
        bool isEligible = false;
        if (checkedCard.color == mainCard.color)
        {
            isEligible = true;
        }
        foreach (CardNetworked card in cardSet)
        {
            if (!isEligible &&
                card != checkedCard &&
                card.cardSO.mainValue == checkedCard.mainValue &&
                card.cardSO.color == mainCard.color
                )
            {
                isEligible = true;
            }
        }
        return isEligible;
    }

    private static int CalculateBonusValue(List<CardNetworked> cardSet, TestCard mainCard, TestCard checkedCard)
    {
        int bonusValueInSet = checkedCard.bonus;
        foreach (CardNetworked card in cardSet)
        {
            if (card.cardSO.color == mainCard.color && card.cardSO.mainValue == checkedCard.mainValue + 1)
            {
                bonusValueInSet = CalculateBonusValue(cardSet, mainCard, card.cardSO);
            }
        }
        return bonusValueInSet;
    }
}
