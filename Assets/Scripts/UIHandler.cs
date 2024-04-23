using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    private Text resultInfo;
    private Image resultImage;
    private Button[][] buttons = new Button[3][];

    private void Start()
    {
        resultInfo = infoPanel.GetComponentInChildren<Text>();
        resultImage = infoPanel.GetComponentInChildren<Image>();
        GameButtonsToArray();
        Menu();
    }
    private void Menu()
    {
        infoPanel.SetActive(true);
        menuPanel.SetActive(true);

        Button button = menuPanel.GetComponentInChildren<Button>();
        button.onClick.AddListener(StartGame);
    }
    private void StartGame()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        resultImage.gameObject.SetActive(false);
        resultInfo.text = "x";

        GameController.instance.ResetStatesArray();
        ResetGameButtons();
    }
    public void ShowGameResult(bool tie, bool xTurn)
    {
        resultImage.gameObject.SetActive(true);
        SetSprite(resultImage, Symbols.Empty);

        if (!tie)
        {
            resultInfo.text = "winner";
            if (xTurn) SetSprite(resultImage, Symbols.Cross);
            else SetSprite(resultImage, Symbols.Circle);
        }
        else resultInfo.text = "tie";

        Menu();
    }
    private void OnButtonClick(Button button, int[] id)
    {
        Symbols buttonState = SetButtonSymbol(GameController.instance.xTurn, button);
        GameController.instance.ButtonPressed(id, buttonState);
    }
    private void ResetGameButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            for (int j = 0; j < buttons[i].Length; j++)
            {
                buttons[i][j].enabled = true;
                SetSprite(buttons[i][j], Symbols.Empty);
            }
        }
    }
    private void GameButtonsToArray()
    {
        Button[] allButtons = gamePanel.GetComponentsInChildren<Button>();
        int buttonNum = 0;
        for (int i = 0; i < 3; i++)
        {
            buttons[i] = new Button[3];
            for (int j = 0; j < 3; j++)
            {
                Button currentButton = allButtons[buttonNum];
                int[] id = { i, j };
                buttons[i][j] = currentButton;
                buttons[i][j].onClick.AddListener(() => OnButtonClick(currentButton, id));
                buttonNum++;
            }
        }
    }
    private Symbols SetButtonSymbol(bool xTurn, Button button)
    {
        Symbols symbol = Symbols.Empty;
        if (button != null)
        {
            if (xTurn)
            {
                resultInfo.text = "o";
                symbol = Symbols.Cross;
            }
            else
            {
                resultInfo.text = "x";
                symbol = Symbols.Circle;
            }
            SetSprite(button, symbol);
            button.enabled = false;
        }
        return symbol;
    }
    private Sprite GetSprite(string name)
    {
        foreach (Sprite image in sprites)
        {
            if (image.name.Equals(name)) return image;
        }
        return null;
    }
    public void SetSprite(Button button, Symbols symbol)
    {
        switch (symbol)
        {
            case Symbols.Cross:
                button.image.sprite = GetSprite("cross");
                break;
            case Symbols.Circle:
                button.image.sprite = GetSprite("wheel");
                break;
            case Symbols.Empty:
                button.image.sprite = GetSprite("blank");
                break;
        }
    }
    public void SetSprite(Image image, Symbols symbol)
    {
        switch (symbol)
        {
            case Symbols.Cross:
                image.sprite = GetSprite("cross");
                break;
            case Symbols.Circle:
                image.sprite = GetSprite("wheel");
                break;
            case Symbols.Empty:
                image.sprite = GetSprite("blank");
                break;

        }
    }
}