using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Affliction
{
    private float slowPercent;

    public float SlowAmount { get { return slowPercent; } }

    public Slow(string name, float duration, float slowPercent) : base(name, AfflictionType.Slow, duration)
	{
        this.slowPercent = slowPercent;
	}
}
