using System;

[System.Serializable]
public class Row : IComparable
{
    public string username;
    public string password;
    public int dinheiro;
    public int x1;
    public int x2;
    public int x3;
    public int x22;
    public Row(string username, string password)
    {
        this.username = username;
        this.password = password;
        dinheiro = 1000;
        x1 = 0;
        x2 = 0;
        x3 = 0;
        x22 = 0;
    }
    public Row(string username, string password, float dinheiro, int x1, int x2, int x3, int x22)
    {
        this.username = username;
        this.password = password;
        this.dinheiro = (int) dinheiro;
        this.x1 = x1;
        this.x2 = x2;
        this.x3 = x3;
        this.x22 = x22;
    }

    public int CompareTo(object obj)
    {
        if (!(obj is Row))
            throw new ArgumentException("Comparing error: argument is not a Row");

        Row other = obj as Row;
        int otherSum = other.x1 + other.x2 + other.x3 + other.x3;
        return otherSum.CompareTo(x1 + x2 + x3 + x22);
    }
}
