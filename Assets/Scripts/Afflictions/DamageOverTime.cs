using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : Affliction
{
    private float damage;
    private float procRate;
    private float procTimer;
    private bool hasProc;

    public float Damage { get { return damage; } }
    public float ProcRate { get { return procRate; } }
    public bool HasProc { get { return hasProc; } }

    public DamageOverTime(string name, float duration, float damage, float procRate) : base(name, AfflictionType.DamageOverTime, duration)
	{
        this.damage = damage;
        this.procRate = procRate;
        procTimer = 0.0f;
        hasProc = false;
    }

    /// <summary>
    /// Update the proc timer
    /// </summary>
    public override void Process()
	{
        base.Process();
        procTimer += Time.deltaTime;
        
        if(procTimer > procRate)
		{
            procTimer = 0.0f;
            hasProc = true;
		}
	}

    /// <summary>
    /// Reset the proc boolean
    /// </summary>
    public void Proc()
	{
        hasProc = false;
    }
}
