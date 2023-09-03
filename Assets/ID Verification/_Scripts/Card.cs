using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public CardInfo _cardInfo;

    [SerializeField] private TextMesh _name;
    [SerializeField] private TextMesh _department;
    [SerializeField] private TextMesh _EMPID;
    [SerializeField] private TextMesh _email;
    [SerializeField] private TextMesh _DOB;
    [SerializeField] private TextMesh _date_of_issue;
    [SerializeField] private TextMesh _expiration_date;
    [SerializeField] private TextMesh _cardID;

    [SerializeField] private TextMesh[] _addressLines;

    [SerializeField] private TextMesh[] _allLabels;

    [SerializeField] public SpriteRenderer _logoRend;

    public string NameValue
    {
        get { return _name.text; }
        set { _name.text = value; }
    }
    public string DepartmentValue
    {
        get { return _department.text; }
        set { _department.text = value; }
    }
    public string EMPIDValue
    {
        get { return _EMPID.text; }
        set { _EMPID.text = value; }
    }
    public string EmailValue
    {
        get { return _email.text; }
        set { _email.text = value; }
    }
    public string DOBValue
    {
        get { return _DOB.text; }
        set { _DOB.text = value; }
    }
    public string DateOfIssueValue
    {
        get { return _date_of_issue.text; }
        set { _date_of_issue.text = value; }
    }
    public string ExpirationDateValue
    {
        get { return _expiration_date.text; }
        set { _expiration_date.text = value; }
    }
    public string CardIDValue
    {
        get { return _cardID.text; }
        set { _cardID.text = value; }
    }

    public void SetAddressValue(int index, string value)
    {
        _addressLines[index].text = value;
    }

    public void SetLabel(int index, string value)
    {
        _allLabels[index].text = value;
    }

    public void SetCardID(int[] ID)
    {
        string New_Card_ID_String = "";
        for (int i = 0; i < ID.Length; i++)
        {
            New_Card_ID_String += ID[i].ToString();
            if ((i + 1) % 5 == 0)
                New_Card_ID_String += " ";
        }

        _cardID.text = New_Card_ID_String;
    }
}
