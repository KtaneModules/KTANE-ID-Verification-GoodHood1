using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardBoss : MonoBehaviour
{

    private const float _cardZValue = 0.0014f;

    [SerializeField] Card _cardPrefab;

    public List<Card> _activeCards = new List<Card>();

    public void MakeCard(CardInfo cardInfo)
    {
        var card = Instantiate(_cardPrefab, transform);
        card.transform.localPosition = Vector3.forward * (_cardZValue + 0.001f * _activeCards.Count);
        card.transform.localRotation = Quaternion.Euler(0, 0, 90 + Random.Range(-15, 16));
        card._cardInfo = cardInfo;
        card.NameValue = cardInfo.Name;
        card.DepartmentValue = cardInfo.Department;
        card.EMPIDValue = cardInfo.EMP_ID;
        card.EmailValue = cardInfo.Email;
        card.DOBValue = cardInfo.D_O_B;
        card.DateOfIssueValue = cardInfo.Date_Of_Issue;
        card.ExpirationDateValue = cardInfo.Expiration_Date;
        card.CardIDValue = cardInfo.Card_ID;
        for (int i = 0; i < 3; i++)
            card.SetAddressValue(i, cardInfo.Address_Lines[i]);
        _activeCards.Add(card);
    }

    public void RemoveAllCards()
    {
        for (int i = 0; i < _activeCards.Count; i++)
            Destroy(_activeCards[i].gameObject);
        _activeCards.Clear();
    }

}