using System.Collections;
using System;
using UnityEngine;

public class AccountException : ApplicationException
{
    public AccountException(string message) : base(message)
    {
    }
}
