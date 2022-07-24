using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo
{
	// Fields
	private Sprite sprite;
	private int cost;
	private int damage;
	private float attackSpeed;
	private int range;
	private bool hasAOE;

	// Properties
	public Sprite Sprite { get { return sprite; } }
	public int Cost { get { return cost; } }
	public int Damage { get { return damage; } }
	public float AttackSpeed { get { return attackSpeed; } }
	public int Range { get { return range; } }
	public bool AOE { get { return hasAOE; } }

	public TowerInfo(Sprite sprite, int cost, int damage, float attackSpeed, int range, bool hasAOE)
	{
		this.sprite = sprite;
		this.cost = cost;
		this.damage = damage;
		this.attackSpeed = attackSpeed;
		this.range = range;
		this.hasAOE = hasAOE;
	}
}
