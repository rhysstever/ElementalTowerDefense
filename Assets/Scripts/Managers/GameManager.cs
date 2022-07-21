using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    MainMenu,
    Game,
    Pause,
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

    public int health;
    public int money;

    private MenuState currentMenuState;

    public MenuState CurrentMenuState { get { return currentMenuState; } }

    // Start is called before the first frame update
    void Start()
    {
        ChangeMenuState(MenuState.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        // Recurring logic that runs dependant on the current MenuState
        switch(currentMenuState)
        {
            case MenuState.MainMenu:
                break;
            case MenuState.Game:
                // ESC key pauses the game
                if(Input.GetKeyDown(KeyCode.Escape))
                    ChangeMenuState(MenuState.Pause);
                // Changes the menu state to Game Over if the player loses all health
                if(health <= 0)
                    ChangeMenuState(MenuState.GameEnd);
                break;
            case MenuState.Pause:
                // ESC key unpauses the game
                if(Input.GetKeyDown(KeyCode.Escape))
                    ChangeMenuState(MenuState.Game);
                break;
            case MenuState.GameEnd:
                break;
        }
    }

    /// <summary>
    /// Performs inital logic when the menu state changes
    /// </summary>
    /// <param name="newMenuState">The new menu state</param>
    public void ChangeMenuState(MenuState newMenuState)
	{
        switch(newMenuState)
        {
            case MenuState.MainMenu:
                health = 100;
                money = 100;
                MapManager.instance.ClearMap();

                break;
            case MenuState.Game:
                EnemyManager.instance.SetupEnemyWaves();

                // Creates the map if the player is coming from the main menu
                if(currentMenuState == MenuState.MainMenu)
                    MapManager.instance.SpawnCurrentMap();
                break;
            case MenuState.Pause:
                break;
            case MenuState.GameEnd:
                break;
        }

        currentMenuState = newMenuState;

        // Update UI
        UIManager.instance.ChangeMenuStateUI(currentMenuState);
    }

    /// <summary>
    /// Ends the game
    /// </summary>
    /// <param name="hasPlayerWon">Whether the player has won the game</param>
    public void EndGame(bool hasPlayerWon)
	{
        ChangeMenuState(MenuState.GameEnd);
	}
}
