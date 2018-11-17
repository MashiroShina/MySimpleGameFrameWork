using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private List<Skill> skills = new List<Skill>();
    void Start()
    {
        skills.Add(new FirePunch(this));
        skills.Add(new KiBlast(this));
    }
    void Update()
    {
        //轮询招式列表
        foreach (Skill skill in skills)
        {           
            skill.Update(Time.deltaTime,Time.fixedDeltaTime);
        }
    }
    public void ResetSkill()
    {
        foreach (Skill skill in skills)
        {
            skill.Reset();
        }
    }
}
