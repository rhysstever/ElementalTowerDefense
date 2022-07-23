using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo
{
	// Fields
	private Sprite sprite;
	private TowerType type;
	private int cost;
	private int damage;
	private int attackSpeed;
	private Dictionary<TowerInfo, TowerType> upgrades;

	// Properties
	public Sprite Sprite { get { return sprite; } }
	public TowerType Type { get { return type; } }
	public int Cost { get { return cost; } }
	public int Damage { get { return damage; } }
	public int AttackSpeed { get { return attackSpeed; } }
	public Dictionary<TowerInfo, TowerType> Upgrades { get { return upgrades; } }

	public TowerInfo(Sprite sprite, TowerType type, int cost, int damage, int attackSpeed)
	{
		this.sprite = sprite;
		this.type = type;
		this.cost = cost;
		this.damage = damage;
		this.attackSpeed = attackSpeed;
		upgrades = new Dictionary<TowerInfo, TowerType>();
	}

	public void AddUpgrade(TowerInfo upgradeTowerInfo, TowerType upgradeTowerType)
	{
		upgrades.Add(upgradeTowerInfo, upgradeTowerType);
	}
}
