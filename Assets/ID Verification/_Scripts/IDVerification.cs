// using KModkit; // You must import this namespace to use KMBombInfoExtensions, among other things. See KModKit Docs below.
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Globalization;
using System.Linq;
using Rnd = UnityEngine.Random;

[RequireComponent(typeof(KMBombModule), typeof(KMSelectable))]
public partial class IDVerification : MonoBehaviour
{
    private KMBombInfo _bombInfo; // for accessing edgework, and certain events like OnBombExploded.
    private KMAudio _audio; // for interacting with the game's audio system.
    private KMBombModule _module;

    private static int s_moduleCount;
    private int _moduleId;
    private bool isSolved;
    private bool DisablePresses = false;

    [SerializeField] private KMSelectable _acceptButton;
    [SerializeField] private KMSelectable _denyButton;
    [SerializeField] private CardBoss _cardBoss;
    [SerializeField] private Sprite[] _IncorrectLogos;

    private string[,] PeopleData = new string[,]
    {
        {"1715143", "Talia York", "Accounting", "23/01/1981", "talia@bombcorp.com"},
        {"1715144", "Maria Wu", "Research and Development", "11/05/1992", "itschinamaria@bombcorp.com"},
        {"1715145", "Esther Banks", "Research and Development", "05/11/2000", "e-banks@bombcorp.com"},
        {"1715146", "Zackary Miranda", "Production", "19/11/1994", "z_a_c_c@bombcorp.com"},
        {"1715147", "Jasmin Burn", "Marketing", "23/02/1988", "burnitalldown@bombcorp.com"},
        {"1715148", "Isabella Rose", "Research and Development", "20/08/1988", "itsrose@bombcorp.com"},
        {"1715149", "Alfred Jones", "Human Resources", "20/01/1998", "aljones@bombcorp.com"},
        {"1715150", "Archie Lambert", "Human Resources", "10/06/1991", "lambertghini@bombcorp.com"},
        {"1715151", "Shania Crossley", "Production", "16/09/1984", "shaniacross@bombcorp.com"},
        {"1715152", "Diane Coles", "Marketing", "23/10/1981", "diane@bombcorp.com"},
        {"1715153", "Parker Mckay", "Accounting", "22/07/1997", "parkayyyy@bombcorp.com"},
        {"1716239", "Lloyd Hatfield", "Accounting", "09/09/1990", "hi_hat@bombcorp.com"},
        {"1716240", "Cadence Petersen", "Marketing", "08/07/1988", "cadencepetersen@bombcorp.com"},
        {"1716241", "Melina Conner", "Research and Development", "22/04/1992", "mel_conner@bombcorp.com"},
        {"1716243", "Lee Mackole", "Purchasing", "03/03/1995", "mchole@bombcorp.com"},
        {"1716244", "Morris Hendricks", "Purchasing", "22/12/1995", "morris_hendricks@bombcorp.com"},
        {"1716245", "Carl Cotton", "Human Resources", "30/11/1991", "fluffer@bombcorp.com"},
        {"1716246", "Alexa Lewis", "Marketing", "19/01/1990", "playdespacito@bombcorp.com"},
        {"1716247", "Trevor Mackenzie", "Human Resources", "02/04/1982", "trevtrev@bombcorp.com"},
        {"1716248", "Megan Mye", "Research and Development", "21/10/2000", "maganmye@bombcorp.com"},
        {"1716249", "Wade Downtown", "Production", "21/11/1994", "wadedowntown@bombcorp.com"},
        {"1716250", "Walt Kingfas", "Production", "21/10/1989", "waltkingfas@bombcorp.com"},
        {"1716251", "Faye Cestpas", "Production", "30/03/1980", "fayecestpas@bombcorp.com"},
        {"1717414", "Annah Ohmbound", "Human Resources", "02/05/1984", "annahohmbound@bombcorp.com"},
        {"1717415", "Stanley Blanc", "Production", "20/03/1987", "stanleyblanc@bombcorp.com"},
        {"1717416", "Leah Head", "Marketing", "10/01/1989", "leahhead@bombcorp.com"},
        {"1717417", "Estelle Ellis", "Research and Development", "02/10/1993", "freerealestelle@bombcorp.com"},
        {"1717418", "Yosef Paine", "Marketing", "10/12/1984", "yosefstairs@bombcorp.com"},
        {"1717419", "Tea Aldred", "Purchasing", "30/05/1986", "heresthemftea@bombcorp.com"},
        {"1717420", "Elisabeth Petersen", "Accounting", "08/10/1987", "el_pete@bombcorp.com"},
        {"1801593", "Ivan Courtney", "Human Resources", "17/05/1995", "clockinhall@bombcorp.com"},
        {"1801594", "Thomas Osmand", "Research and Development", "28/12/1997", "tomos@bombcorp.com"},
        {"1801595", "Ashley Stennish", "Accounting", "10/01/1999", "ashleysten@bombcorp.com"},
        {"1801596", "Timothy Taylor", "Purchasing", "06/05/1980", "timtay@bombcorp.com"},
        {"1801597", "Don Logan", "Production", "08/06/1984", "da_don@bombcorp.com"},
        {"1801598", "Lilly Archer", "Marketing", "04/04/1995", "archnemesis@bombcorp.com"},
        {"1802541", "Bertie Bright", "Purchasing", "20/11/1986", "brightbert@bombcorp.com"},
        {"1802542", "Sydney North", "Accounting", "03/05/1997", "sydney@bombcorp.com"},
        {"1802543", "Harvey Hughes", "Purchasing", "02/09/1996", "imhughes@bombcorp.com"},
        {"1802544", "Franklyn Cullen", "Production", "06/04/1997", "frankfurt@bombcorp.com"},
        {"1802543", "Joshua Barnes", "Human Resources", "17/12/1994", "joshuabarnes@bombcorp.com"},
        {"1802546", "Donte English", "Marketing", "06/11/1989", "notdonte@bombcorp.com"},
        {"1802547", "Fraser Dorsey", "Production", "05/01/1991", "frasier@bombcorp.com"},
        {"1802548", "Rachel Simpson", "Research and Development", "10/03/1976", "rachelsimp@bombcorp.com"},
        {"1802549", "Dan Salgado", "Production", "17/06/1988", "notsalv@bombcorp.com"},
        {"1802550", "Alissia Reyna", "Research and Development", "09/02/2000", "alice_reyna@bombcorp.com"},
        {"1802551", "Michael Bonilla", "Purchasing", "18/10/1981", "mike_bon@bombcorp.com"},
        {"1802552", "Charlie Sanford", "Production", "05/03/1996", "sansford@bombcorp.com"}
    }; //48 total people

    private List<CardInfo> CardInfos = new List<CardInfo>();

    private int StageCount;
    private int stage;

    private List<int> RandomPeopleIndices = new List<int>();
    private List<bool> CorrectAnswers = new List<bool>();

    private List<List<int>> CardIDs = new List<List<int>>();


    private void Awake() {
        _moduleId = s_moduleCount++;
        _module = GetComponent<KMBombModule>();
        _audio = GetComponent<KMAudio>();

        _acceptButton.OnInteract += delegate () { ButtonPress(true); return false; };
        _denyButton.OnInteract += delegate () { ButtonPress(false); return false; };

    }

    private void Start() 
    {
        StageCount = Rnd.Range(2, 5);
        Log($"There will be {StageCount} cards.");
        CorrectAnswers.Clear();
        RandomPeopleIndices.Clear();
        CardIDs.Clear();
        CardInfos.Clear();
        for (int i = 0; i < StageCount; i++)
            CorrectAnswers.Add(Rnd.Range(0, 2) == 1 ? true : false); //True = Accept.

        var InitialLogLine = "The correct button presses will be: ";
        for (int i = 0; i < StageCount; i++)
        {
            var PressTypes = new Dictionary<bool, string>
            {
                { true, "Accept" },
                { false, "Deny" }
            };
            InitialLogLine += PressTypes[CorrectAnswers[i]] + ", ";
        }
        InitialLogLine = InitialLogLine.Substring(0, InitialLogLine.Length - 2);
        Log(InitialLogLine);

        stage = 0;
        CreateNewCard();
    }

    private CardInfo MakeRandomCardInfo()
    {
        var RandomPersonIndex = Rnd.Range(0, 48);
        while (RandomPeopleIndices.Contains(RandomPersonIndex))
            RandomPersonIndex = Rnd.Range(0, 48);
        RandomPeopleIndices.Add(RandomPersonIndex);

        CardInfo _cardInfo = new CardInfo();

        _cardInfo.EMP_ID = PeopleData[RandomPersonIndex, 0];
        _cardInfo.Name = PeopleData[RandomPersonIndex, 1];
        _cardInfo.Department = PeopleData[RandomPersonIndex, 2];
        _cardInfo.Email = PeopleData[RandomPersonIndex, 4];

        var DOB = PeopleData[RandomPersonIndex, 3];
        DateTime DOB_DT = DateTime.ParseExact(DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        _cardInfo.D_O_B = DOB;

        DateTime dateOfIssue_DT = DOB_DT.AddDays(Rnd.Range(3650, 7301));
        _cardInfo.Date_Of_Issue = dateOfIssue_DT.ToString("dd/MM/yyyy");

        DateTime expDate_DT = dateOfIssue_DT.AddDays(Rnd.Range(257, 986));
        _cardInfo.Expiration_Date = expDate_DT.ToString("dd/MM/yyyy");

        _cardInfo.SetCardID(GenerateCardID(_cardInfo));

        return _cardInfo;
    }

    private int[] GenerateCardID(CardInfo curCard)
    {
        int[] curID = new int[20];
        curID[0] = curCard.Date_Of_Issue[0] - '0';
        curID[1] = curCard.Date_Of_Issue[1] - '0';
        string SurnameAlphaNum = TwoDigitAlphaNum(curCard.Name.Split(' ')[1][0]);
        curID[2] = SurnameAlphaNum[0] - '0';
        curID[3] = SurnameAlphaNum[1] - '0';
        curID[4] = (curID[0] + curID[1] + curID[2] + curID[3]) % 10;
        curID[5] = Rnd.Range(0, 10);
        curID[6] = Rnd.Range(0, 10);
        curID[7] = curCard.D_O_B[0] - '0';
        curID[8] = curCard.D_O_B[1] - '0';
        curID[9] = (curID[5] + curID[6] + curID[7] + curID[8]) % 10;
        curID[10] = Rnd.Range(0, 10);
        string FirstnameAlphaNum = TwoDigitAlphaNum(curCard.Name.Split(' ')[0][0]);
        curID[11] = FirstnameAlphaNum[0] - '0';
        curID[12] = FirstnameAlphaNum[1] - '0';
        curID[13] = Rnd.Range(0, 10);
        curID[14] = (curID[10] + curID[11] + curID[12] + curID[13]) % 10;
        int EMP_ID_Sum = 0;
        for (int i = 0; i < curCard.EMP_ID.Length; i++)
            EMP_ID_Sum += curCard.EMP_ID[i] - '0';
        curID[15] = EMP_ID_Sum % 10;
        curID[16] = curCard.EMP_ID[1] - '0';
        var DepartmentID = new string[] { "Production", "Research and Development", "Purchasing", "Marketing", "Human Resources", "Accounting" };
        curID[17] = Array.IndexOf(DepartmentID, curCard.Department) + 1;
        curID[18] = Rnd.Range(0, 10);
        int CheckSum = 0;
        for (int i = 0; i < 19; i++)
            CheckSum += curID[i];
        curID[19] = CheckSum % 10;

        return curID;
    }

    private string TwoDigitAlphaNum(char letter)
    {
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string index = (alpha.IndexOf(Char.ToUpper(letter)) + 1).ToString();
        if (index.Length == 1)
            index = "0" + index;
        return index;
    }

    private void CreateNewCard()
    {
        _cardBoss.MakeCard(MakeRandomCardInfo());
        Log($"Card {stage + 1}");
        if (CorrectAnswers[stage] == false)
        {
            AlterCard(_cardBoss._activeCards[stage]);
            Log("Therefore you should press Deny.");
        }
        else
        {
            Log("This card is valid.");
            Log("Therefore you should press Accept.");
        }
    }

    private void AlterCard(Card card)
    {
        var CorrectLabels = new string[] { "EMP-ID", "Email", "D.O.B", "Date of Issue", "Expiration Date", "Card-ID" };
        var AlterType = Rnd.Range(0, 3);
        if (AlterType == 0) // Card Formatting
        {
            var ErrorType = Rnd.Range(0, 5);
            if (ErrorType == 0) // Misspelling
            {
                var ErrorLabels = new string[] { "DI-PME", "Remail", "O.B.D", "Date of Isue", "Expiration Dayt", "Cod-ID" };
                var RandomLabel = Rnd.Range(0, 6);
                card.SetLabel(RandomLabel, ErrorLabels[RandomLabel] + " :");
                Log($"The label '{CorrectLabels[RandomLabel]}' has been misspelt as '{ErrorLabels[RandomLabel]}'");
            }
            if (ErrorType == 1) // Date Formatting
            {
                var Dates = new string[] { card.DOBValue, card.DateOfIssueValue, card.ExpirationDateValue };
                var RandomDate = Rnd.Range(0, 3);
                var NewDate = Dates[RandomDate].Substring(6) + "/" + Dates[RandomDate].Substring(0, 5);
                if (RandomDate == 0) card.DOBValue = NewDate;
                if (RandomDate == 1) card.DateOfIssueValue = NewDate;
                if (RandomDate == 2) card.ExpirationDateValue = NewDate;
                Log($"The {CorrectLabels[RandomDate+2]} has its year first, which is the wrong format.");
            }
            if (ErrorType == 2) // Dates Gap
            {
                var randomDays = Rnd.Range(986, 1500);
                var NewDate =  DateTime.ParseExact(card.ExpirationDateValue, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(randomDays).ToString("dd/MM/yyyy");
                card.ExpirationDateValue = NewDate;
                Log($"The Date of Issue and the Expiration Date are more than 986 days apart.");
            }
            if (ErrorType == 3) //  Incorrect Logo
            {
                var IncorrectLogoIndex = Rnd.Range(0, 2);
                card._logoRend.sprite = _IncorrectLogos[IncorrectLogoIndex];
                Log("The logo is incorrect.");
            }
            if (ErrorType == 4) // Address
            {
                var IncorrectAddresses = new string[,]
                {
                    { "Dept. of Bombs", "Department of Verif.", "Dept. of Cards" },
                    { "1273 Rockefeller Road", "1327 Rockefeller Street", "1273 Stonefeller Street" },
                    { "Brooklyn NY11213", "Queens NY11212", "London NY11212" }
                };
                var RandomAddress = new int[] { Rnd.Range(0, 3), Rnd.Range(0, 3) };
                card.SetAddressValue(RandomAddress[0], IncorrectAddresses[RandomAddress[0], RandomAddress[1]]);
                Log($"Incorrect Address. Line {RandomAddress[0]+1} is '{IncorrectAddresses[RandomAddress[0], RandomAddress[1]]}' which is not correct.");
            }
        }
        if (AlterType == 1) // Employee Information
        {
            var DateTypeToAlter = Rnd.Range(0, 5);
            var IncorrectPersonIndex = Rnd.Range(0, 48);
            while (IncorrectPersonIndex == RandomPeopleIndices[stage])
                IncorrectPersonIndex = Rnd.Range(0, 48);

            var DataTypes = new string[] { card.EMPIDValue, card.NameValue, card.DepartmentValue, card.DOBValue, card.EmailValue };
            var DataTypeNames = new string[] { "EMP-ID", "Name", "Department", "D.O.B", "Email" };
            var RandomDataType = Rnd.Range(0, DataTypes.Length);

            var NewData = PeopleData[IncorrectPersonIndex, RandomDataType];
            if (RandomDataType == 0) card.EMPIDValue = NewData;
            if (RandomDataType == 1) card.NameValue = NewData;
            if (RandomDataType == 2) card.DepartmentValue = NewData;
            if (RandomDataType == 3) card.DOBValue = NewData;
            if (RandomDataType == 4) card.EmailValue = NewData;

            Log($"The employee data is incorrect. The {DataTypeNames[RandomDataType]} reads {NewData} when it should be {PeopleData[RandomPeopleIndices[stage], RandomDataType]}");
        }
        if (AlterType == 2) // Card ID
        {
            var NonRandomDigits = new int[] { 0, 1, 2, 3, 4, 7, 8, 9, 11, 12, 14, 15, 16, 17, 19 };
            var CardID = card._cardInfo.Card_ID_List;
            var DigitToAlter = NonRandomDigits[Rnd.Range(0, NonRandomDigits.Length)];

            var NumToAdd = Rnd.Range(1, 10);
            var PreAltered = CardID[DigitToAlter];
            CardID[DigitToAlter] = (CardID[DigitToAlter] + NumToAdd) % 10;

            Log($"Card ID is incorrect. Digit {DigitToAlter+1} is displayed as {CardID[DigitToAlter]} but it should be {PreAltered}.");

            card.SetCardID(CardID);
        }
    }

    private void ButtonPress(bool btn)
    {
        if (isSolved || DisablePresses) return;
        if (btn == true) _audio.PlaySoundAtTransform("Accept", _acceptButton.transform);
        else _audio.PlaySoundAtTransform("Deny", _acceptButton.transform);

        if (CorrectAnswers[stage] == btn && stage == StageCount-1)
        {
            Solve();
        } 
        else if (CorrectAnswers[stage] == btn && stage < StageCount-1)
        {
            stage++;
            CreateNewCard();
        }
        else if (CorrectAnswers[stage] != btn)
        {
            Strike($"Your input for card {stage} was incorrect.");
        }
    }


    public void Log(string message) => Debug.Log($"[{_module.ModuleDisplayName} #{_moduleId}] {message}");

    public void Strike(string message) {
        Log($"✕ {message}");
        Log("Resetting...");
        StartCoroutine(StrikeAnim());
        // * Add code that should execute on every strike (eg. a strike animation) here.
    }

    public void Solve() {
        Log("◯ Module solved!");
        _module.HandlePass();
        isSolved = true;
    }

    private IEnumerator StrikeAnim()
    {
        DisablePresses = true;
        _cardBoss.RemoveAllCards();
        _module.HandleStrike();
        _audio.PlaySoundAtTransform("Siren", _module.transform);
        yield return new WaitForSeconds(5f);
        Start();
        DisablePresses = false;

    }
}
