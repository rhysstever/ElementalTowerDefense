using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo
{
	// Fields
	private Sprite enemySprite;
	private int health;
	private int damage;
	private int goldWorth;
	private float moveSpeed;

	// Properties
	public Sprite Sprite { get { return enemySprite; } }
	public int Health { get { return health; } }
	public int Damage { get { return damage; } }
	public int GoldWorth { get { return goldWorth; } }
	public float MoveSpeed { get { return moveSpeed;} }

	public EnemyInfo(Sprite enemySprite, int health, int damage, int goldWorth, float moveSpeed)
	{
		this.enemySprite = enemySprite;
		this.health = health;
		this.damage = damage;
		this.goldWorth = goldWorth;
		this.moveSpeed = moveSpeed;
	}

	public string GetHealthText() { return "Health: " + health; }
	public string GetDamageText() { return "Damage: " + damage; }
	public string GetBountyText() { return "Bounty: " + goldWorth; }
	public string GetSpeedText() { return "Speed: " + MoveSpeed; }
}
