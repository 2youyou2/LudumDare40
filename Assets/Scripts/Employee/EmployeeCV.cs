﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeCV 
{
    public float Happiness
    {
        get;
        private set;
    }
    public int MoneyCost
    {
        get;
        private set;
    }
    public string Name
    {
        get;
        private set;
    }

    public EmployeeCV()
    {
        GetRandomEmployeeStats();
    }

    public void GetRandomEmployeeStats()
    {
        Name = GenerateName(5);
        Happiness = Random.Range(GameMetaManager.Employee.EmployeeStats.minInitialHappiness, GameMetaManager.Employee.EmployeeStats.maxInitialHappiness);
        MoneyCost = Mathf.CeilToInt(Happiness * GameMetaManager.Employee.EmployeeStats.moneyFactor);
    }

    public static string GenerateName(int len)
    {
        System.Random r = new  System.Random();
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string Name = "";
        Name += consonants[r.Next(consonants.Length)].ToUpper();
        Name += vowels[r.Next(vowels.Length)];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            Name += consonants[r.Next(consonants.Length)];
            b++;
            Name += vowels[r.Next(vowels.Length)];
            b++;
        }

        return Name;
    }
}
