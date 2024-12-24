using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Validator
{
    public bool IsValidBirthYear(int birthYear, int currentYear)
    {
        return birthYear <= currentYear && birthYear >= currentYear - 50;
    }

    public bool IsValidExpirationYear(int expirationYear, int currentYear)
    {
        return expirationYear >= currentYear;
    }

    public bool IsValidOriginSymbol(string origin, Sprite symbol, List<OriginSymbolPair> pairs)
    {
        if (pairs == null || pairs.Count == 0)
        {
            Debug.LogError("Pairs collection is null or empty.");
            return false;
        }

        return pairs.Any(pair => pair.origin == origin && pair.symbol == symbol);
    }



}
