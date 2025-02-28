using UnityEngine;
using System.IO;

public class GameController : MonoBehaviour
{
    private ActionStack actionStack = new ActionStack();
    private UIHandler uiHandler;
    public static GameController instance;

    public bool xTurn;
    public bool isPaused = false;
    private bool gameOver = false;

    private Symbols[][] buttonsState = new Symbols[3][];

    private int[][] winningCombinations = new int[8][]
    {
        new int[] {0, 1, 2}, new int[] {3, 4, 5}, new int[] {6, 7, 8},
        new int[] {0, 3, 6}, new int[] {1, 4, 7}, new int[] {2, 5, 8},
        new int[] {0, 4, 8}, new int[] {2, 4, 6}
    };

    public void Initialize()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    void Awake()
    {
        Initialize();
        ResetStatesArray();
        uiHandler = FindFirstObjectByType<UIHandler>();
    }

    public void ResetStatesArray()
    {
        xTurn = true;
        gameOver = false;
        actionStack.Clear();
        for (int row = 0; row < 3; row++)
        {
            buttonsState[row] = new Symbols[3];
            for (int col = 0; col < 3; col++)
            {
                buttonsState[row][col] = Symbols.Empty;
            }
        }
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true; 
            actionStack.Do(
                () => uiHandler.TogglePauseMenu(true),
                () => uiHandler.TogglePauseMenu(false),
                true 
            );
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false; 
            actionStack.Do(
                () => uiHandler.TogglePauseMenu(false),
                () => uiHandler.TogglePauseMenu(true),
                true 
            );
        }
    }

    public void ButtonPressed(int[] id, Symbols symbol)
    {
        if (isPaused || gameOver) return;

        int row = id[0];
        int col = id[1];
        Symbols previousState = buttonsState[row][col];
        string previousText = uiHandler.GetResultText();

        actionStack.Do(
            () =>
            {
                buttonsState[row][col] = symbol;
                uiHandler.UpdateButtonUI(row, col, symbol);
                uiHandler.SetResultText(xTurn ? "o" : "x");
            },
            () =>
            {
                buttonsState[row][col] = previousState;
                uiHandler.UpdateButtonUI(row, col, previousState);
                uiHandler.SetResultText(previousText);
            }
        );
        Result();
        xTurn = !xTurn;
    }

    public void UndoMove()
    {
        if (isPaused || gameOver || actionStack.IsEmpty()) return;
        actionStack.Undo();
        xTurn = !xTurn;
        uiHandler.SetResultText(xTurn ? "x" : "o");
    }

    public void RedoMove()
    {
        if (isPaused || gameOver || actionStack.IsEmpty()) return;
        actionStack.Redo();
        xTurn = !xTurn;
    }

    private void Result()
    {
        bool win = WinCheck();
        bool tie = IsGridFilled();
        if (win || tie)
        {
            gameOver = true;
            uiHandler.ShowGameResult(win, xTurn);
        }
    }

    private bool IsGridFilled()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (buttonsState[row][col] == Symbols.Empty) return false;
            }
        }
        return true;
    }

    public bool WinCheck()
    {
        foreach (int[] combination in winningCombinations)
        {
            int btn1 = combination[0];
            int btn2 = combination[1];
            int btn3 = combination[2];

            if (buttonsState[btn1 / 3][btn1 % 3] == buttonsState[btn2 / 3][btn2 % 3] &&
                buttonsState[btn2 / 3][btn2 % 3] == buttonsState[btn3 / 3][btn3 % 3] &&
                buttonsState[btn1 / 3][btn1 % 3] != Symbols.Empty)
                return true;
        }
        return false;
    }
}
