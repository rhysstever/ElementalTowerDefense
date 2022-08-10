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
	private GameObject mainMenuParent, gameParent, pauseParent, controlsParent, gameEndParent;

	// Main Menu
	[SerializeField]
	private GameObject playButton, mainMenuToControlsButton, quitButton;
	
	// Game
	[SerializeField]    // Text
	private GameObject healthText, moneyText, waveText;
	[SerializeField]    // Panels
	private GameObject towerBuildPanel, selectedObjectPanel, typeInfoPanel, typeInfoSubPanel;
	[SerializeField]    // Buttons
	private GameObject openTowerPanelButton, closeTowerPanelButton, sellTowerButton, pauseGameButton;

	// Pause
	[SerializeField]
	private GameObject resumeButton, pauseToControlsButton, pauseToMainButton;
	
	// Controls
	[SerializeField]
	private GameObject closeControlsButton, backButton, nextButton;

	// Game End
	[SerializeField] 
	private GameObject gameEndHeaderText, gameEndToMainButton;

	private Dictionary<MenuState, GameObject> menuStateUIParents;
	private MenuState menuStateBeforeControlsMenu;
	private int controlsTextIndex;

	// Properties
	public bool IsInfoPanelOpen { get { return typeInfoPanel.activeInHierarchy; } }

	// Start is called before the first frame update
	void Start()
	{
		LinkMenuStateParents();
		SetupUI();

		menuStateBeforeControlsMenu = MenuState.MainMenu;
		controlsTextIndex = -1;
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
		menuStateUIParents.Add(MenuState.Controls, controlsParent);
		menuStateUIParents.Add(MenuState.GameEnd, gameEndParent);
	}

	/// <summary>
	/// Creates on click listeners for each button in the scene
	/// </summary>
	private void SetupUI()
	{
		// Menu buttons
		playButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
		mainMenuToControlsButton.GetComponent<Button>().onClick.AddListener(() => OpenControlsMenu(MenuState.MainMenu));
		quitButton.GetComponent<Button>().onClick.AddListener(() => QuitGame());
		pauseGameButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Pause));
		resumeButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
		pauseToControlsButton.GetComponent<Button>().onClick.AddListener(() => OpenControlsMenu(MenuState.Pause));
		pauseToMainButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
		gameEndToMainButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));

		// Tower info panel open/close buttons
		openTowerPanelButton.GetComponent<Button>().onClick.AddListener(() => SetTowerPanelActive(true));
		closeTowerPanelButton.GetComponent<Button>().onClick.AddListener(() => SetTowerPanelActive(false));

		// Set tower buy buttons
		foreach(Transform buildTowerButton in towerBuildPanel.transform)
			if(buildTowerButton.GetComponent<Tower>() != null)
				buildTowerButton.GetComponent<Button>().onClick.AddListener(
					() => BuildManager.instance.Build(buildTowerButton.GetComponent<Tower>().Type));

		// Set tower sell button
		sellTowerButton.GetComponent<Button>().onClick.AddListener(
			() => BuildManager.instance.Sell());

		// Set tower info buttons
		foreach(Transform towerInfoButton in typeInfoPanel.transform.GetChild(2))
			towerInfoButton.GetComponent<Button>().onClick.AddListener(
				() => UpdateSelectedTypeInfoUI(towerInfoButton.GetComponent<Tower>().Type));

		// Controls buttons
		closeControlsButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(menuStateBeforeControlsMenu));
		nextButton.GetComponent<Button>().onClick.AddListener(() => NextControlsText());
		backButton.GetComponent<Button>().onClick.AddListener(() => BackControlsText());
	}

	/// <summary>
	/// Hide or show UI parents based on the new menu state
	/// </summary>
	/// <param name="newMenuState">The new menu state</param>
	public void ChangeMenuStateUI(MenuState newMenuState)
	{
		foreach(MenuState menuState in menuStateUIParents.Keys)
		{
			// Dont hide the game menu UI when the game is paused
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
						selectedObjectPanel.transform.GetChild(i).gameObject.SetActive(true);

					TowerType towerType = selectedGameObj.GetComponent<Tower>().Type;
					TowerInfo towerInfo = TowerManager.instance.TowerInfo[towerType];

					// Set stat texts
					selectedObjectPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = towerType + " Tower";
					selectedObjectPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = towerInfo.GetDamageText();
					selectedObjectPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = towerInfo.GetAttackSpeedText();
					selectedObjectPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = towerInfo.GetRangeText();
					selectedObjectPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = towerInfo.GetAfflictionText();
					break;
				case "Enemy":
					// Show everything but the sell button
					for(int i = 0; i < selectedObjectPanel.transform.childCount; i++)
						if(selectedObjectPanel.transform.GetChild(i).gameObject != sellTowerButton)
							selectedObjectPanel.transform.GetChild(i).gameObject.SetActive(true);

					Enemy enemy = selectedGameObj.GetComponent<Enemy>();
					EnemyInfo enemyInfo = EnemyManager.instance.EnemyInfo[enemy.Type];

					// Set stat texts
					selectedObjectPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = enemy.Type + " Enemy";
					selectedObjectPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Health: " + enemy.CurrentHealth + "/" + enemyInfo.Health;
					selectedObjectPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = "Speed: " + (enemy.CurrentMoveSpeed * 100);
					selectedObjectPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = enemyInfo.GetDamageText();
					selectedObjectPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = enemyInfo.GetBountyText();
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
		typeInfoSubPanel.transform.GetChild(5).GetComponent<TMP_Text>().text = towerInfo.GetAfflictionText();
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
	/// Open the controls menu panel
	/// </summary>
	/// <param name="previousMenuState">The previous state of the game</param>
	private void OpenControlsMenu(MenuState previousMenuState)
	{
		controlsTextIndex = -1;
		NextControlsText();
		menuStateBeforeControlsMenu = previousMenuState;
		GameManager.instance.ChangeMenuState(MenuState.Controls);
	}

	/// <summary>
	/// Shows the next group of controls menu text
	/// </summary>
	private void NextControlsText()
	{
		controlsTextIndex++;
		UpdateControlsMenu();
	}

	/// <summary>
	/// Shows the previous group of controls menu text
	/// </summary>
	private void BackControlsText()
	{
		controlsTextIndex--;
		UpdateControlsMenu();
	}

	/// <summary>
	/// Update the controls menu's elements
	/// </summary>
	private void UpdateControlsMenu()
	{
		// Show or hide the back and next buttons
		backButton.SetActive(controlsTextIndex != 0);
		nextButton.SetActive(controlsTextIndex != controlsParent.transform.GetChild(0).GetChild(1).childCount - 1);

		// Show the right text
		for(int i = 0; i < controlsParent.transform.GetChild(0).GetChild(1).childCount; i++)
			controlsParent.transform.GetChild(0).GetChild(1).GetChild(i).gameObject.SetActive(i == controlsTextIndex);
	}

	/// <summary>
	/// Updates header text on the game end panel
	/// </summary>
	/// <param name="hasWon"></param>
	public void UpdateGameEndText(bool hasWon)
	{
		gameEndHeaderText.GetComponent<TMP_Text>().text = hasWon ? "Game Won! \nAll Waves Cleared" : "Game Over!";
	}

	/// <summary>
	/// Quits the application
	/// </summary>
	private void QuitGame()
	{
		// Quits the game unless the game is running on web gl
		if(Application.platform != RuntimePlatform.WebGLPlayer)
			Application.Quit();
	}
}
