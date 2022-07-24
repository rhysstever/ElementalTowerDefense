using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region Singleton Code
	// A public reference to this script
	public static UIManager instance = null;

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

	[SerializeField]    // Parent objects
	private GameObject mainMenuParent, gameParent, pauseParent, gameEndParent;
	[SerializeField]    // Main Menu buttons
	private GameObject playButton, quitButton;
	[SerializeField]    // Pause buttons
	private GameObject resumeButton, pauseToMainButton;
	[SerializeField]    // Game End buttons
	private GameObject gameEndToMainButton;
	[SerializeField]    // Player Stats Text
	private GameObject healthText, moneyText;
	[SerializeField]    // Game parent panel
	private GameObject selectedObjectPanel, towerPanel, typeInfoPanel;

	private Dictionary<MenuState, GameObject> menuStateUIParents;

	// Properties
	public bool IsBuyingTower { get { return towerPanel.activeInHierarchy; } }

	// Start is called before the first frame update
	void Start()
	{
		LinkMenuStateParents();
		SetupUI();
	}

	// Update is called once per frame
	void Update()
	{
		// Toggle the tower panel with TAB
		if(Input.GetKeyDown(KeyCode.Tab)
			&& GameManager.instance.CurrentMenuState == MenuState.Game)
			ToggleTowerPanel();
	}

	/// <summary>
	/// Links a menu state with a parent object, via dictionary
	/// </summary>
	private void LinkMenuStateParents()
	{
		// Instaniate the dictionary
		menuStateUIParents = new Dictionary<MenuState, GameObject>();

		// Fill the dictionary, one entry per menu state
		menuStateUIParents.Add(MenuState.MainMenu, mainMenuParent);
		menuStateUIParents.Add(MenuState.Game, gameParent);
		menuStateUIParents.Add(MenuState.Pause, pauseParent);
		menuStateUIParents.Add(MenuState.GameEnd, gameEndParent);
	}

	/// <summary>
	/// Creates on click listeners for each button in the scene
	/// </summary>
	private void SetupUI()
	{
		playButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
		quitButton.GetComponent<Button>().onClick.AddListener(() => Application.Quit());
		resumeButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
		pauseToMainButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
		gameEndToMainButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));

		// Set Tower Buy Button onClicks
		foreach(Transform childTrans in towerPanel.transform.GetChild(2))
			childTrans.GetComponent<Button>().onClick.AddListener(
				() => UpdateSelectedTypeInfoUI(childTrans.GetComponent<Tower>().Type));
	}

	/// <summary>
	/// Hide or show UI parents based on the new menu state
	/// </summary>
	/// <param name="newMenuState">The new menu state</param>
	public void ChangeMenuStateUI(MenuState newMenuState)
	{
		foreach(MenuState menuState in menuStateUIParents.Keys)
		{
			if(newMenuState == MenuState.Pause
				&& menuState == MenuState.Game)
				continue;
			else
				menuStateUIParents[menuState].SetActive(menuState == newMenuState);

			// Hide the tower panel initially
			if(newMenuState == MenuState.Game)
				towerPanel.SetActive(false);
		}
	}

	/// <summary>
	/// Updates the text for player stats
	/// </summary>
	public void UpdatePlayerStatsText()
	{
		int health = GameManager.instance.health;
		int money = GameManager.instance.money;

		healthText.GetComponent<TMP_Text>().text = "Health: " + health;
		moneyText.GetComponent<TMP_Text>().text = "Money: " + money;
	}

	/// <summary>
	/// Updates UI to display info about the currently selected game object
	/// </summary>
	/// <param name="selectedGameObj">The currently selected game object</param>
	public void UpdateSelectedObjectUI(GameObject selectedGameObj)
	{
		if(selectedGameObj == null)
			selectedObjectPanel.SetActive(false);
		else if(selectedGameObj.tag == "Tile")
		{
			selectedObjectPanel.SetActive(true);
			selectedObjectPanel.transform.GetChild(0).gameObject.SetActive(true);
			selectedObjectPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = "Tile";
			selectedObjectPanel.transform.GetChild(1).gameObject.SetActive(false);
			selectedObjectPanel.transform.GetChild(2).gameObject.SetActive(false);
		}
		else if(selectedGameObj.tag == "Tower")
		{
			selectedObjectPanel.SetActive(true);
			selectedObjectPanel.transform.GetChild(0).gameObject.SetActive(true);
			TowerType type = selectedGameObj.GetComponent<Tower>().Type;
			TowerInfo towerInfo = TowerManager.instance.TowerInfo[type];
			selectedObjectPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = type + " Tower";
			selectedObjectPanel.transform.GetChild(1).gameObject.SetActive(true);
			selectedObjectPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Damage: " + towerInfo.Damage;
			selectedObjectPanel.transform.GetChild(2).gameObject.SetActive(true);
			selectedObjectPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = "Attack Speed: " + towerInfo.AttackSpeed;
		}
		else
			selectedObjectPanel.SetActive(false);
	}

	/// <summary>
	/// Update the side panel with tower type info
	/// </summary>
	/// <param name="selectedType"></param>
	private void UpdateSelectedTypeInfoUI(TowerType selectedType)
	{
		// Update value in TowerManager
		TowerManager.instance.SelectedTypeInfo = selectedType;

		typeInfoPanel.SetActive(true);
		typeInfoPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = selectedType + " Tower";
		typeInfoPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Cost: " + TowerManager.instance.TowerInfo[selectedType].Cost;
		typeInfoPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = "Damage: " + TowerManager.instance.TowerInfo[selectedType].Damage;
		typeInfoPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = "Attack Speed: " + TowerManager.instance.TowerInfo[selectedType].AttackSpeed;
		string rangeText = "Range: " + TowerManager.instance.TowerInfo[selectedType].Range;
		if(TowerManager.instance.TowerInfo[selectedType].AOE)
			rangeText += " AOE";
		typeInfoPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = rangeText;
	}

	/// <summary>
	/// Toggles the tower info panel
	/// </summary>
	private void ToggleTowerPanel()
	{
		towerPanel.SetActive(!towerPanel.activeInHierarchy);

		// If the tower panel is visible, hide the side info panel
		if(towerPanel.activeInHierarchy)
			typeInfoPanel.SetActive(false);
	}
}
