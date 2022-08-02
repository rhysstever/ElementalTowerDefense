using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo
{
	// Fields
	private Sprite towerSprite;
	private Sprite bulletSprite;
	private int cost;
	private int damage;
	private float attackSpeed;
	private int range;
	private bool hasAOE;
	private Dictionary<TowerType, TowerType> upgrades;

	// Properties
	public Sprite TowerSprite { get { return towerSprite; } }
	public Sprite BulletSprite { get { return bulletSprite; } }
	public int Cost { get { return cost; } }
	public int Damage { get { return damage; } }
	public float AttackSpeed { get { return attackSpeed; } }
	public int Range { get { return range; } }
	public bool AOE { get { return hasAOE; } }
	public Dictionary<TowerType, TowerType> Upgrades { get { return upgrades; } }

	public TowerInfo(Sprite towerSprite, Sprite bulletSprite, int cost, int damage, float attackSpeed, int range, bool hasAOE)
	{
		this.towerSprite = towerSprite;
		this.bulletSprite = bulletSprite;
		this.cost = cost;
		this.damage = damage;
		this.attackSpeed = attackSpeed;
		this.range = range;
		this.hasAOE = hasAOE;
		upgrades = new Dictionary<TowerType, TowerType>();
	}

	public void AddUpgrade(TowerType secondaryType, TowerType upgradeType)
	{
		upgrades.Add(secondaryType, upgradeType);
	}
}
