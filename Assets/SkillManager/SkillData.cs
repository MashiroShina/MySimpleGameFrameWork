using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillData  {

    public static Dictionary<int, int[]> instructions = new Dictionary<int, int[]>();
    static SkillData()
    {
        //100-D 115-S 106-J
        instructions.Add(1, new int[] { 100, 115, 100, 106 });//dsdj
        instructions.Add(2, new int[] { 115, 100, 106 });//sdj
    }
}
