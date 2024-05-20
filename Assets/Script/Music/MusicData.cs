using System;

[Serializable]
public class MusicData
{
    public int ID;
    public string Name;
    public override string ToString()
    {
        return $"ID: {ID} || Name: {Name}";
    }
}

// The Receive File will be
/*
receive.json
[
    {
        "ID": 1,
        "Name": "1convit"
    },
    {
        "ID": 2,
        "Name": "2convit"
    }
]
*/