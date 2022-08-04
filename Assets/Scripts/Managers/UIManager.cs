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

	[SerializeField]    // Parent Objects
	private GameObject mainMenuParent, gameParent, pauseParent, gameEndParent;

	[SerializeField]    // Main Menu Buttons
	private GameObject playButton, quitButton;
	[SerializeField]    // Pause Menu Buttons
	private GameObject resumeButton, pauseToMainButton;
	[SerializeField]    // Game End Menu Buttons
	private GameObject gameEndToMainButton;
	
	[SerializeField]    // Game Menu Text
	private GameObject healthText, moneyText, waveText;
	[SerializeField]    // Game Menu Panels
	private GameObject towerBuildPanel, selectedObjectPanel, typeInfoPanel, typeInfoSubPanel;
	[SerializeField]    // Game Menu buttons
	private GameObject openTowerPanelButton, closeTowerPanelButton, sellTowerButton;

	[SerializeField]
	private GameObject gameEndHeaderText;

	private Dictionary<MenuState, GameObject> menuStateUIParents;

	// Properties
	public bool IsInfoPanelOpen { get { return typeInfoPanel.activeInHierarchy; } }

	// Start is called before the first frame update
	void Start()
	{
		LinkMenuStateParents();
		SetupUI();
	}

	// Update is called once per frame
	void Update()
	{
		
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
		// Menu buttons
		playButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
		quitButton.GetComponent<Button>().onClick.AddListener(() => Application.Quit());
		resumeButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
		pauseToMainButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
		gameEndToMainButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));

		// Tower info panel open/close buttons
		openTowerPanelButton.GetComponent<Button>().onClick.AddListener(() => SetTowerPanelActive(true));
		closeTowerPanelButton.GetComponent<Button>().onClick.AddListener(() => SetTowerPanelActive(false));

		// Set tower buy button onClicks
		foreach(Transform buildTowerButton in towerBuildPanel.transform)
			if(buildTowerButton.GetComponent<Tower>() != null)
				buildTowerButton.GetComponent<Button>().onClick.AddListener(
					() => BuildManager.instance.Build(buildTowerButton.GetComponent<Tower>().Type));

		// Set tower sell button onclick
		sellTowerButton.GetComponent<Button>().onClick.AddListener(
			() => BuildManager.instance.Sell());

		// Set tower info button onClicks
		foreach(Transform towerInfoButton in typeInfoPanel.transform.GetChild(2))
			towerInfoButton.GetComponent<Button>().onClick.AddListener(
				() => UpdateSelectedTypeInfoUI(towerInfoButton.GetComponent<Tower>().Type));
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
				typeInfoPanel.SetActive(false);
		}
	}

	/// <summary>
	/// Updates text about the player's health
	/// </summary>
	public void UpdatePlayerHealthText() 
	{ 
		healthText.GetComponent<TMP_Text>().text = "Health: " + GameManager.instance.Health; 
	}

	/// <summary>
	/// Updates text about the player's money
	/// </summary>
	public void UpdatePlayerMoneyText()
	{
		moneyText.GetComponent<TMP_Text>().text = "Money: " + GameManager.instance.Money;
	}

	/// <summary>
	/// Updates text about the wave
	/// </summary>
	public void UpdateWaveText()
	{
		waveText.GetComponent<TMP_Text>().text = EnemyManager.instance.GetWaveText();
	}

	/// <summary>
	/// Updates UI to display info about the currently selected game object
	/// </summary>
	/// <param name="selectedGameObj">The currently selected game object</param>
	public void UpdateSelectedObjectUI(GameObject selectedGameObj)
	{
		if(selectedGameObj == null)
		{
			selectedObjectPanel.SetActive(false);
		}
		else
		{
			selectedObjectPanel.SetActive(true);
			switch(selectedGameObj.tag)
			{
				case "Tile":
					selectedObjectPanel.transform.GetChild(0).gameObject.SetActive(true);
					selectedObjectPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = "Tile";

					// Hide everything else
					for(int i = 1; i < selectedObjectPanel.transform.childCount; i++)
						selectedObjectPanel.transform.GetChild(i).gameObject.SetActive(false);
					break;
				case "Tower":
					// Show everything but the 4th text element
					for(int i = 0; i < selectedObjectPanel.transform.childCount; i++)
						if(i != 4)
							selectedObjectPanel.transform.GetChild(i).gameObject.SetActive(true);

					TowerType towerType = selectedGameObj.GetComponent<Tower>().Type;
					TowerInfo towerInfo = TowerManager.instance.TowerInfo[towerType];

					// Set stat texts
					selectedObjectPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = towerType + " Tower";
					selectedObjectPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = towerInfo.GetDamageText();
					selectedObjectPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = towerInfo.GetAttackSpeedText();
					selectedObjectPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = towerInfo.GetRangeText();
					break;
				case "Enemy":
					// Show everything but the sell button
					for(int i = 0; i < selectedObjectPanel.transform.childCount; i++)
						if(selectedObjectPanel.transform.GetChild(i).gameObject != sellTowerButton)
							selectedObjectPanel.transform.GetChild(i).gameObject.SetActive(true);

					EnemyType enemyType = selectedGameObj.GetComponent<Enemy>().Type;
					EnemyInfo enemyInfo = EnemyManager.instance.EnemyInfo[enemyType];

					// Set stat texts
					selectedObjectPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = enemyType + " Enemy";
					selectedObjectPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = enemyInfo.GetHealthText();
					selectedObjectPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = enemyInfo.GetDamageText();
					selectedObjectPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = enemyInfo.GetBountyText();
					selectedObjectPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = enemyInfo.GetSpeedText();
					break;
				default:
					selectedObjectPanel.SetActive(false);
					break;
			}
		}
	}

	/// <summary>
	/// Update the side panel with tower type info
	/// </summary>
	/// <param name="selectedType">The selected tower type</param>
	private void UpdateSelectedTypeInfoUI(TowerType selectedType)
	{
		// Update value in TowerManager
		TowerManager.instance.SelectedTypeInfo = selectedType;
		TowerInfo towerInfo = TowerManager.instance.TowerInfo[selectedType];

		typeInfoSubPanel.SetActive(true);
		typeInfoSubPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = selectedType + " Tower";
		typeInfoSubPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = towerInfo.GetCostText();
		typeInfoSubPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = towerInfo.GetDamageText();
		typeInfoSubPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = towerInfo.GetAttackSpeedText();
		typeInfoSubPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = towerInfo.GetRangeText();
	}

	/// <summary>
	/// Shows or hides the tower info panel
	/// </summary>
	/// <param name="isTowerPanelActive">Whether the tower info panel is visible or hidden</param>
	private void SetTowerPanelActive(bool isTowerPanelActive)
	{
		typeInfoPanel.SetActive(isTowerPanelActive);

		// If the tower panel is visible, hide the side info panel
		if(isTowerPanelActive)
			typeInfoSubPanel.SetActive(false);
	}

	/// <summary>
	/// Updates header text on the game end panel
	/// </summary>
	/// <param name="hasWon"></param>
	public void UpdateGameEndText(bool hasWon)
	{
		gameEndHeaderText.GetComponent<TMP_Text>().text = hasWon ? "Game Won" : "Game Over";
	}
}
