using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo
{
	// Fields
	private string enemyName;
	private int health;
	private int damage;
	private int goldWorth;
	private float moveSpeed;

	// Properties
	public string EnemyName { get { return enemyName; } }
	public int Health { get { return health; } }
	public int Damage { get { return damage; } }
	public int GoldWorth { get { return goldWorth; } }
	public float MoveSpeed { get { return moveSpeed;} }

	public EnemyInfo(string enemyName, int health, int damage, int goldWorth, float moveSpeed)
	{
		this.enemyName = enemyName;
		this.health = health;
		this.damage = damage;
		this.goldWorth = goldWorth;
		this.moveSpeed = moveSpeed;
	}
}
