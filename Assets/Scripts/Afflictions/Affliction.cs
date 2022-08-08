using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AfflictionType
{
    DamageOverTime,
    Slow
}

public class Affliction
{
    private string name;
    private AfflictionType type;
    private float duration;
    private float currentDuration;

    public string Name { get { return name; } }
    public AfflictionType Type { get { return type; } }
    public float FullDuration { get { return duration; } }
    public float CurrentDuration { get { return currentDuration; } }

    public Affliction(string name, AfflictionType type, float duration)
	{
        this.name = name;
        this.type = type;
        this.duration = duration;
        currentDuration = duration;
	}

    /// <summary>
    /// Update the affliction's timer
    /// </summary>
    public virtual void Process()
	{
        currentDuration -= Time.deltaTime;
    }

    /// <summary>
    /// Reset the affliction's timer
    /// </summary>
    public void Reset()
	{
        currentDuration = duration;
	}
}
