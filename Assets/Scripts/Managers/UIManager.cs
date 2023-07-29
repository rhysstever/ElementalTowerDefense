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
	private Button playButton, mainMenuToControlsButton, quitButton;
	
	// Game
	[SerializeField]    // Player Text
	private GameObject healthText, moneyText, waveText;
	[SerializeField]    // Selected Object Text
	private GameObject selectedObjHeader, selectedObjText1, selectedObjText2, selectedObjText3, selectedObjText4;
	[SerializeField]    // Tower Info Sub-Panel Text
	private GameObject towerInfoTypeText, towerInfoCostText, towerInfoDamageText, towerInfoASText, towerInfoRangeText, towerInfoAfflictionText;
	[SerializeField]    // Panels
	private GameObject towerBuildPanel, selectedObjectPanel, typeInfoPanel, typeInfoSubPanel;
	[SerializeField]    // Empty Parents
	private GameObject selectedObjTextParent, selectedTowerButtonParent;
	[SerializeField]    // Buttons
	private Button openTowerPanelButton, closeTowerPanelButton, sellTowerButton, towerTargetTypeButton, pauseGameButton;

	// Pause
	[SerializeField]
	private Button resumeButton, pauseToControlsButton, pauseToMainButton;

	// Controls
	[SerializeField]
	private GameObject controlsTextParent;
	[SerializeField]	// Buttons
	private Button closeControlsButton, backButton, nextButton;

	// Game End
	[SerializeField]
	private GameObject gameEndHeaderText;
	[SerializeField]
	private Button gameEndToMainButton;

	private Dictionary<MenuState, GameObject> menuStateUIParents;
	private int controlsTextIndex;

	// Properties
	public bool IsInfoPanelOpen { get { return typeInfoPanel.activeInHierarchy; } }

	// Start is called before the first frame update
	void Start()
	{
		LinkMenuStateParents();
		SetupUI();

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
		playButton.onClick.AddListener(() => GameManager.instance.MenuStateNew(MenuState.Game));
		mainMenuToControlsButton.onClick.AddListener(() => OpenControlsMenu());
		quitButton.onClick.AddListener(() => QuitGame());
		pauseGameButton.onClick.AddListener(() => GameManager.instance.MenuStateNew(MenuState.Pause));
		resumeButton.onClick.AddListener(() => GameManager.instance.MenuStateBack());
		pauseToControlsButton.onClick.AddListener(() => OpenControlsMenu());
		pauseToMainButton.onClick.AddListener(() => GameManager.instance.MenuStateNew(MenuState.MainMenu));
		gameEndToMainButton.onClick.AddListener(() => GameManager.instance.MenuStateNew(MenuState.MainMenu));

		// Tower info panel open/close buttons
		openTowerPanelButton.onClick.AddListener(() => SetTowerPanelActive(true));
		closeTowerPanelButton.onClick.AddListener(() => SetTowerPanelActive(false));

		// Set tower buy buttons
		foreach(Transform buildTowerButton in towerBuildPanel.transform)
			if(buildTowerButton.GetComponent<Tower>() != null)
				buildTowerButton.GetComponent<Button>().onClick.AddListener(
					() => BuildManager.instance.Build(buildTowerButton.GetComponent<Tower>().Type));

		// Set tower sell button
		sellTowerButton.onClick.AddListener(
			() => BuildManager.instance.Sell());

		// Set tower target type button
		towerTargetTypeButton.onClick.AddListener(
			() => BuildManager.instance.CycleCurrentTowerTargetType());

		// Set tower info buttons
		foreach(Transform towerInfoButton in typeInfoPanel.transform.GetChild(2))
			towerInfoButton.GetComponent<Button>().onClick.AddListener(
				() => UpdateSelectedTypeInfoUI(towerInfoButton.GetComponent<Tower>().Type));

		// Controls buttons
		closeControlsButton.onClick.AddListener(() => GameManager.instance.MenuStateBack());
		nextButton.onClick.AddListener(() => NextControlsText());
		backButton.onClick.AddListener(() => BackControlsText());
	}

	public void ShowMenuStateUI(MenuState menuState)
	{
		menuStateUIParents[menuState].SetActive(true);

		if(menuState == MenuState.Game)

		switch(menuState)
		{
			case MenuState.MainMenu:
				break;
			case MenuState.Game:
				// Hide the tower type info panel
				typeInfoPanel.SetActive(false);
				break;
			case MenuState.Pause:
				break;
			case MenuState.Controls:
				break;
			case MenuState.GameEnd:
				UpdateGameEndText(GameManager.instance.Health > 0);
				break;
		}
	}

	public void HideMenuStateUI(MenuState menuState)
	{
		menuStateUIParents[menuState].SetActive(false);
	}

	public void ClearMenuStateUI()
	{
		foreach(GameObject menuStateParent in menuStateUIParents.Values)
		{
			menuStateParent.SetActive(false);
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
					selectedObjHeader.SetActive(true);
					selectedObjHeader.GetComponent<TMP_Text>().text = "Tile";

					// Hide everything else
					for(int i = 1; i < selectedObjectPanel.transform.childCount; i++)
						selectedObjectPanel.transform.GetChild(i).gameObject.SetActive(false);
					break;
				case "Tower":
					// Show all text and buttons
					selectedObjTextParent.SetActive(true);
					selectedTowerButtonParent.SetActive(true);

					Tower tower = selectedGameObj.GetComponent<Tower>();
					TowerInfo towerInfo = TowerManager.instance.TowerInfo[tower.Type];

					// Set header and stat text
					selectedObjHeader.GetComponent<TMP_Text>().text = tower.Type + " Tower";
					selectedObjText1.GetComponent<TMP_Text>().text = towerInfo.GetDamageText();
					selectedObjText2.GetComponent<TMP_Text>().text = towerInfo.GetAttackSpeedText();
					selectedObjText3.GetComponent<TMP_Text>().text = towerInfo.GetRangeText();
					selectedObjText4.GetComponent<TMP_Text>().text = towerInfo.GetAfflictionText();

					// Set button text
					sellTowerButton.GetComponentInChildren<TMP_Text>().text = "Sell for " + (towerInfo.Cost / 2);
					towerTargetTypeButton.GetComponentInChildren<TMP_Text>().text = tower.TargetType.ToString();
					break;
				case "Enemy":
					// Show all text
					selectedObjTextParent.SetActive(true);

					// Hide all tower buttons
					selectedTowerButtonParent.SetActive(false);

					Enemy enemy = selectedGameObj.GetComponent<Enemy>();
					EnemyInfo enemyInfo = EnemyManager.instance.EnemyInfo[enemy.Type];

					// Set header and stat text
					selectedObjHeader.GetComponent<TMP_Text>().text = enemy.Type + " Enemy";
					selectedObjText1.GetComponent<TMP_Text>().text = "Health: " + enemy.CurrentHealth + "/" + enemyInfo.Health;
					selectedObjText2.GetComponent<TMP_Text>().text = "Speed: " + (enemy.CurrentMoveSpeed * 100);
					selectedObjText3.GetComponent<TMP_Text>().text = enemyInfo.GetDamageText();
					selectedObjText4.GetComponent<TMP_Text>().text = enemyInfo.GetBountyText();
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
		towerInfoTypeText.GetComponent<TMP_Text>().text = selectedType + " Tower";
		towerInfoCostText.GetComponent<TMP_Text>().text = towerInfo.GetCostText();
		towerInfoDamageText.GetComponent<TMP_Text>().text = towerInfo.GetDamageText();
		towerInfoASText.GetComponent<TMP_Text>().text = towerInfo.GetAttackSpeedText();
		towerInfoRangeText.GetComponent<TMP_Text>().text = towerInfo.GetRangeText();
		towerInfoAfflictionText.GetComponent<TMP_Text>().text = towerInfo.GetAfflictionText();
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
	private void OpenControlsMenu()
	{
		controlsTextIndex = -1;
		NextControlsText();
		GameManager.instance.MenuStateNew(MenuState.Controls);
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
		backButton.gameObject.SetActive(controlsTextIndex != 0);
		nextButton.gameObject.SetActive(controlsTextIndex != controlsTextParent.transform.childCount - 1);

		// Show the right text
		for(int i = 0; i < controlsTextParent.transform.childCount; i++)
			controlsTextParent.transform.GetChild(i).gameObject.SetActive(i == controlsTextIndex);
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
