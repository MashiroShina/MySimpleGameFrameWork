using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skill:ManagerBase
{

    /// <summary>
    /// 招式ID
    /// </summary>
    protected int skillID;

    /// <summary>
    /// 招式指令的状态机
    /// </summary>
    protected Fsm<Skill> fsm=new Fsm<Skill>();

    /// <summary>
    /// 招式持有者
    /// </summary>
    protected Player player;

    /// <summary>
    /// 最大指令状态ID
    /// </summary>
    protected int maxSkillStateID;

    /// <summary>
    /// 指令状态ID
    /// </summary>
    protected int stateID = 0;

    public Skill(Player player)
    {
        this.player = player;
        skillID = GetSkillID();
        Inits();
        fsm.Start(1);
    }

    protected virtual int GetSkillID()
    {
        return -1;
    }
    protected virtual void SkillFight()
    {
        player.ResetSkill();
    }

    private void Inits()
    {
        if (SkillData.instructions.ContainsKey(skillID))
        { 
            int[] instructions = null;
            SkillData.instructions.TryGetValue(skillID, out instructions);

            maxSkillStateID = instructions.Length;

            for (int i = 0; i < instructions.Length; i++)
            {
                if (i == instructions.Length - 1)
                {
                    //最后一个指令需要执行招式的出招处理
                    AddInstructionsState((KeyCode)instructions[i], SkillFight);
                }
                else
                {
                    AddInstructionsState((KeyCode)instructions[i]);
                }
            }
           
        }
       
    }
    protected void AddInstructionsState(KeyCode keyCode, UnityAction action = null, float inputWaitTime = 0.5f)
    {
        fsm.AddIDState(new InstructionsState(++stateID, inputWaitTime, keyCode, action, maxSkillStateID));
        Debug.Log(stateID);
    }
    /// <summary>
    /// 指令状态机重置
    /// </summary>
    public void Reset()
    {
        fsm.ChangeState(1);
    }
    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        fsm.Update(elapseSeconds, realElapseSeconds);
    }

    public override void Shutdown()
    {
        throw new System.NotImplementedException();
    }
}
