using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : Affliction
{
    private float damage;
    private float procRate;
    private float procTimer;

    public float Damage { get { return damage; } }
    public float ProcRate { get { return procRate; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void FixedUpdate()
	{
        if(GameManager.instance.CurrentMenuState == MenuState.Game)
            Process();
    }

    protected override void Process()
	{
        procTimer += Time.deltaTime;
        
        if(procTimer > procRate)
		{
            procTimer = 0.0f;
		}
	}

    public void SetAffliction(float duration, float damage, float procRate)
	{
        SetAffliction(AfflictionType.DamageOverTime, duration);
        this.damage = damage;
        this.procRate = procRate;
        procTimer = 0.0f;
	}
}
