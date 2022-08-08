using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AfflictionType
{
    DamageOverTime,
    Slow
}

public class Affliction : MonoBehaviour
{
    private AfflictionType type;
    private float duration;

    public AfflictionType Type { get { return type; } }
    public float Duration { get { return duration; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void FixedUpdate()
    {
        if(GameManager.instance.CurrentMenuState == MenuState.Game)
            Process();
    }

    protected virtual void Process()
	{
        duration -= Time.deltaTime;
    }

    protected void SetAffliction(AfflictionType type, float duration)
	{
        this.type = type;
        this.duration = duration;
	}
}
