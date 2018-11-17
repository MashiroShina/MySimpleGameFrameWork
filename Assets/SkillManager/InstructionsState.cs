using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstructionsState : FsmState<Skill>
{
    private float inputWaitTime;

    /// <summary>
    /// 计时器
    /// </summary>
    private float timer;

    /// <summary>
    /// 招式指令状态对应的按键指令
    /// </summary>
    private KeyCode keyCode;

    /// <summary>
    /// 指令状态要执行的方法
    /// </summary>
    private UnityAction action;

    /// <summary>
    /// 最大招式指令状态ID
    /// </summary>
    private int maxStateID;

    public InstructionsState(int stateID, float inputWaitTime, KeyCode keyCode, UnityAction action, int maxStateID)
    {
        base.stateID = stateID;
        this.inputWaitTime = inputWaitTime;
        timer = 0;
        this.keyCode = keyCode;
        this.action = action;
        this.maxStateID = maxStateID;
    }

    public override void OnUpdate(Fsm<Skill> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        timer += elapseSeconds;
        if (timer >= inputWaitTime)
        {
            //指令输入等待时间耗尽，重置指令状态
            timer = 0;
            fsm.ChangeState(1);
        }
        //按下任意按键
        if (Input.anyKeyDown)
        {
            //重置计时器
            timer = 0;

            //按下了指令状态的对应按键
            if (Input.GetKeyDown(keyCode))//当这里判断成功时候表示第一个按键已经按正确了
            {
                Debug.Log("玩家按下了" + keyCode.ToString());

                //执行该指令要执行的方法
                if (action != null)
                {
                    action();
                }

                //最后一个指令状态
                if (base.stateID == maxStateID)
                {
                    //重置指令状态
                    fsm.ChangeState(1);
                }
                else
                {
                    //不是最后一个指令状态，切换到下一个指令状态
                    fsm.ChangeState(stateID + 1);
                }
            }
            //未按下指令状态的对应指令，重置指令状态
            else
            {
                fsm.ChangeState(1);
            }
        }
    }
}
