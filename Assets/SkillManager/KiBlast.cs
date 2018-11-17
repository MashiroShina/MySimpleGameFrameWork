using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiBlast : Skill {
    public KiBlast(Player player) : base(player)
    {
    }

    protected override int GetSkillID()
    {
        return 2;
    }

    protected override void SkillFight()
    {
        base.SkillFight();
        Debug.Log("气功波！");
    }

}
