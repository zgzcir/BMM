using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : BaseManager
{
    private User user;
    private List<Friend> friends = new List<Friend>();

        
    public void AddFriendInformation(Friend fr)
    {
        friends.Add(fr);
    }

    public void ClearFriendInformation()
    {
        friends.Clear();
    }

    public List<Friend> GetFriends()
    {
        return friends;
    }
    
    public UserManager(Facade facade) : base(facade)
    {
    }

    public void SetUserInformation(int id, string username, string nickname, bool ifl)
    {
        user = new User(id, username, null, nickname, ifl);
    }

    public int GetUserID()
    {
        return user.Id;
    }

    public string GetNickname()
    {
        return user.Nickname;
    }

    public void Setlogined(string nickname)
    {
        user.IsLoginFirst = false;
        user.Nickname = nickname;
    }

    public bool IsFirstTimeLogin()
    {
        if (user.IsLoginFirst)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}