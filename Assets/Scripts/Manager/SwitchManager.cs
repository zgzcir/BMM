using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI; 

public class SwitchManager : BaseManager
{
    public SwitchManager(Facade facade) : base(facade)
    {

    }
    
    
    

    public UserType userType;

    private ChangeTypeButton ctButton;

    public void ChangeType()
    {
        if (userType == UserType.Normal)
        {
            userType = UserType.Deaf;
        }
        else
        {
            userType = UserType.Normal;
        }
    }

    public UserType GetType()
    {
        return userType;
    }

    public enum UserType
    {
        Normal,
        Deaf
    }
}