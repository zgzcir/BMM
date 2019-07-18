using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend
{
    public int Id { get; set; }
    public string Nickname { get; set; }


    public Friend(int id, string nickname)
    {
        Id = id;
        Nickname = nickname;
    }
}