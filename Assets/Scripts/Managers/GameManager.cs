using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
	MainMenu,
	Game,
	Pause,
	Controls,
	GameEnd
}

public class GameManager : MonoBehaviour
{
	#region Singleton Code
	// A public reference to this script
	public static GameManager instance = null;

	// Awake is called even before start 
	// (I think its at the very beginning of runtime)
	private void Awake()
	{
		// If the reference for this script is null, assign it this script
		if(instance == null)
			instance = this;
		// If the reference is to something else (it already exists)
		// than this is not needed, thus destroy it
		else if(instance != this)
			Destroy(gameObject);
	}
	#endregion

	private int health;
	private int money;

	[SerializeField]
	private Stack<MenuState> menuStateStack;

	// Properties
	public int Health { get { return health; } }
	public int Money { get { return money; } }
	public MenuState CurrentMenuState { get { return menuStateStack.Peek(); } }

	// Start is called before the first frame update
	void Start()
	{
		menuStateStack = new Stack<MenuState>();
		MenuStateNew(MenuState.MainMenu);
	}

	// Update is called once per frame
	void Update()
	{
		// Recurring logic that runs dependant on the current MenuState
		switch(menuStateStack.Peek())
		{
			case MenuState.MainMenu:
				break;
			case MenuState.Game:
				// ESC key pauses the game
				if(Input.GetKeyDown(KeyCode.P))
					MenuStateNew(MenuState.Pause);
				break;
			case MenuState.Pause:
				// ESC key unpauses the game
				if(Input.GetKeyDown(KeyCode.P))
					MenuStateBack();
				break;
			case MenuState.GameEnd:
				break;
		}
	}

	/// <summary>
	/// Adds a menu menu to the menu stack
	/// </summary>
	/// <param name="newMenuState">The new menu being shown</param>
	public void MenuStateNew(MenuState newMenuState)
	{
		if(newMenuState == MenuState.MainMenu
			|| newMenuState == MenuState.Game)
		{
			menuStateStack.Clear();
			UIManager.instance.ClearMenuStateUI();
		}

		MenuStateEnter(newMenuState);
		menuStateStack.Push(newMenuState);
		UIManager.instance.ShowMenuStateUI(menuStateStack.Peek());
	}

	/// <summary>
	/// Returns to the previous menu
	/// </summary>
	public void MenuStateBack()
	{
		MenuState leavingMenuState = menuStateStack.Pop();
		UIManager.instance.HideMenuStateUI(leavingMenuState);
	}

	/// <summary>
	/// Initial Logic for entering a menu
	/// </summary>
	/// <param name="menuState">The new menu being shown</param>
	private void MenuStateEnter(MenuState menuState)
	{
		switch(menuState)
		{
			case MenuState.MainMenu:
				health = 100;
				money = 80;
				UIManager.instance.UpdatePlayerHealthText();
				UIManager.instance.UpdatePlayerMoneyText();
				MapManager.instance.ClearMap();
				break;
			case MenuState.Game:
				MapManager.instance.SpawnCurrentMap();  // Create the map
				EnemyManager.instance.SetupEnemyWaves();
				BuildManager.instance.Select(null); // Set initial selection
				break;
			case MenuState.Pause:
				break;
			case MenuState.Controls:
				break;
			case MenuState.GameEnd:
				break;
		}
	}

	/// <summary>
	/// Ends the game
	/// </summary>
	/// <param name="hasPlayerWon">Whether the player has won the game</param>
	public void EndGame(bool hasPlayerWon)
	{
		MenuStateNew(MenuState.GameEnd);
	}

	/// <summary>
	/// Update the player's health
	/// </summary>
	/// <param name="amount">The amount of health given/taken away from the player</param>
	public void UpdateHealth(int amount)
	{
		health += amount;

		// Changes the menu state to Game Over if the player loses all health
		if(health <= 0)
			MenuStateNew(MenuState.GameEnd);

		// Update UI
		UIManager.instance.UpdatePlayerHealthText();
	}

	/// <summary>
	/// Update the player's money
	/// </summary>
	/// <param name="amount">The amount of money given/taken away from the player</param>
	public void UpdateMoney(int amount)
	{
		money += amount;

		// Update UI
		UIManager.instance.UpdatePlayerMoneyText();
	}
}
