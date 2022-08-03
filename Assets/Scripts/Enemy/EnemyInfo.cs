using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo
{
	// TODO: Add enemy sprite
	// Fields
	private int health;
	private int damage;
	private int goldWorth;
	private float moveSpeed;

	// Properties
	public int Health { get { return health; } }
	public int Damage { get { return damage; } }
	public int GoldWorth { get { return goldWorth; } }
	public float MoveSpeed { get { return moveSpeed;} }

	public EnemyInfo(int health, int damage, int goldWorth, float moveSpeed)
	{
		this.health = health;
		this.damage = damage;
		this.goldWorth = goldWorth;
		this.moveSpeed = moveSpeed;
	}
}
