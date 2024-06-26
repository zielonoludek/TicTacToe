using UnityEngine;
public class GameController : MonoBehaviour
{
    private UIHandler uiHandler;
    public static GameController instance;
    public bool xTurn;

    private Symbols[][] buttonsState = new Symbols[3][];

    private int[][] winningCombinations = new int[8][]
    {
        new int[] {0, 1, 2},
        new int[] {3, 4, 5},
        new int[] {6, 7, 8},
        new int[] {0, 3, 6},
        new int[] {1, 4, 7},
        new int[] {2, 5, 8},
        new int[] {0, 4, 8},
        new int[] {2, 4, 6}
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
        for (int row = 0; row < 3; row++)
        {
            buttonsState[row] = new Symbols[3];
            for (int col = 0; col < 3; col++)
            {
                buttonsState[row][col] = Symbols.Empty;
            }
        }
    }

    private bool IsGridFilled()
    {
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
    }

    private void Result()
    {
        bool win = WinCheck();
        bool tie = IsGridFilled();
        if (win || tie) uiHandler.ShowGameResult(win, xTurn);
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

    public void ButtonPressed(int[] id, Symbols symbol)
    {
        int row = id[0];
        int col = id[1];

        switch (symbol)
        {
            case Symbols.Circle:
                buttonsState[row][col] = Symbols.Circle;
                break;
            case Symbols.Cross:
                buttonsState[row][col] = Symbols.Cross;
                break;
            case Symbols.Empty:
                buttonsState[row][col] = Symbols.Empty;
                break;
        }

        Result();
        xTurn = !xTurn; 
    }
}