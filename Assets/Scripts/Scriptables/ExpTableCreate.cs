using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpTable", menuName = "Monomyto/ExpTable", order = 2)]
public class ExpTableCreate : ScriptableObject
{
    public int[] LevelsExp;

    public int ExpNeed(int level)
    {
        return LevelsExp[level - 1];
    }
}
