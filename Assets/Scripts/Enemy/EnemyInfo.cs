using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo
{
	// Fields
	private Sprite enemySprite;
	private float health;
	private float moveSpeed;
	private int damage;
	private int goldWorth;

	// Properties
	public Sprite Sprite { get { return enemySprite; } }
	public float Health { get { return health; } }
	public float MoveSpeed { get { return moveSpeed;} }
	public int Damage { get { return damage; } }
	public int GoldWorth { get { return goldWorth; } }

	public EnemyInfo(Sprite enemySprite, float health, float moveSpeed, int damage, int goldWorth)
	{
		this.enemySprite = enemySprite;
		this.health = health;
		this.damage = damage;
		this.goldWorth = goldWorth;
		this.moveSpeed = moveSpeed;
	}

	public string GetDamageText() { return "Damage: " + damage; }
	public string GetBountyText() { return "Bounty: " + goldWorth; }
}
