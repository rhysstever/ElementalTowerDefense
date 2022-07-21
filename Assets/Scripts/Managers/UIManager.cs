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
    [SerializeField]    // Selected Object parent panel
    private GameObject selectedObjectPanel;

    private Dictionary<MenuState, GameObject> menuStateUIParents;

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
        playButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
        quitButton.GetComponent<Button>().onClick.AddListener(() => Application.Quit());
        resumeButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));
        pauseToMainButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
        gameEndToMainButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.MainMenu));
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
}
