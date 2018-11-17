using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePunch : Skill {
    public FirePunch(Player player) : base(player)
    {
    }

    protected override int GetSkillID()
    {
        return 1;
    }

    protected override void SkillFight()
    {
        base.SkillFight();
        Debug.Log("升龙拳！");
    }

}
