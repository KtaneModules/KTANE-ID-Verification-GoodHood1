using System.Collections.Generic;

public class CardInfo {

    public string Name { get; set; }
    public string Department { get; set; }
    public string EMP_ID { get; set; }
    public string Email { get; set; }
    public string D_O_B { get; set; }
    public string Date_Of_Issue { get; set; }
    public string Expiration_Date { get; set; }
    public string Card_ID { get; set; }
    public string[] Address_Lines { get; set; }

    public int[] Card_ID_List { get; set;  }

    public CardInfo()
    {
        Address_Lines = new string[3];
        Address_Lines[0] = "Dept. of Verification";
        Address_Lines[1] = "1273 Rockefeller Street";
        Address_Lines[2] = "Brooklyn NY11212";
    }

    public void SetCardID(int[] ID)
    {
        Card_ID_List = ID;
        string New_Card_ID_String = "";
        for (int i = 0; i < ID.Length; i++)
        {
            New_Card_ID_String += ID[i].ToString();
            if ((i + 1) % 5 == 0)
                New_Card_ID_String += " ";
        }

        Card_ID = New_Card_ID_String;
    }
}
