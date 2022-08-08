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
	private float range;
	private bool hasAOE;
	private Affliction affliction;
	private Dictionary<TowerType, TowerType> upgrades;

	// Properties
	public Sprite TowerSprite { get { return towerSprite; } }
	public Sprite BulletSprite { get { return bulletSprite; } }
	public int Cost { get { return cost; } }
	public int Damage { get { return damage; } }
	public float AttackSpeed { get { return attackSpeed; } }
	public float Range { get { return range; } }
	public bool AOE { get { return hasAOE; } }
	public Affliction Affliction { get { return affliction; } }
	public Dictionary<TowerType, TowerType> Upgrades { get { return upgrades; } }

	public TowerInfo(Sprite towerSprite, Sprite bulletSprite, int cost, int damage, float attackSpeed, float range, bool hasAOE, Affliction affliction)
	{
		this.towerSprite = towerSprite;
		this.bulletSprite = bulletSprite;
		this.cost = cost;
		this.damage = damage;
		this.attackSpeed = attackSpeed;
		this.range = range;
		this.hasAOE = hasAOE;
		this.affliction = affliction;
		upgrades = new Dictionary<TowerType, TowerType>();
	}

	// Methods
	/// <summary>
	/// Add a possible upgrade to this tower type
	/// </summary>
	/// <param name="secondaryType">The other tower type needed to upgrade</param>
	/// <param name="upgradeType">The upgrade tower type result</param>
	public void AddUpgrade(TowerType secondaryType, TowerType upgradeType)
	{
		upgrades.Add(secondaryType, upgradeType);
	}

	public string GetCostText() { return "Cost: " + cost; }
	public string GetDamageText() { return "Damage: " + damage; }
	public string GetAttackSpeedText() { return "Attack Rate: " + attackSpeed + "s"; }
	public string GetRangeText() 
	{
		string text = "Range: " + range;
		if(hasAOE)
			text += " AOE";
		return text; 
	}
	public string GetAfflictionText()
	{
		if(affliction == null)
			return "";

		string str = affliction.Name + ": ";

		switch(affliction.Type)
		{
			case AfflictionType.DamageOverTime:
				DamageOverTime dot = (DamageOverTime)affliction;
				str += dot.Damage + " dmg every " + dot.ProcRate + "s for " + dot.FullDuration + "s";
				break;
			case AfflictionType.Slow:
				Slow slow = (Slow)affliction;
				int slowAmount = (int)(slow.SlowAmount * 100);
				str += slowAmount + "% slow for " + slow.FullDuration + "s";
				break;
		}

		return str;
	}
}
