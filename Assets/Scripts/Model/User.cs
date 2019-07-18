using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }

    public string Password { get; set; }

    public string Nickname { get; set; }
    public bool IsLoginFirst { get; set; }


    public User(int id, string username, string password, string nickname = "null", bool ilf = true)
    {
        Id = id;
        UserName = username;
        Password = password;
        Nickname = nickname;
        IsLoginFirst = ilf;
    }
}