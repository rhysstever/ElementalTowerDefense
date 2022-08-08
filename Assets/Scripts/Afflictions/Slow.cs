using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Affliction
{
    private float slowPercent;

    public float SlowAmount { get { return slowPercent; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetAffliction(float duration, float slowPercentAmount)
	{
        SetAffliction(AfflictionType.Slow, duration);
        slowPercent = slowPercentAmount;
	}
}
