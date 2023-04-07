using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
    public        List<CardContainer> Cards;
    public static UnityEvent<bool>    SwitchButton = new();
    public        GameObject          CardPrefab;
    public        List<GameObject>    deck1 = new();
    public        List<GameObject>    deck2 = new();
    public        bool                Turn;
    public        AIBrain             AIbrain;
    public        int                 AISelectedCard, PlayerSelectedCard;

    public TMP_Text PlayerScoreText, AIScoreText, TurnText, EndGameText;

    private Category LastSelectedCategory;
    private Category OtherPlayerCategory;

    public int        PlayerScore, AIScore;
    public GameObject GameplayUI,  ScoreUI, TimerUI;

    private GameObject InstantiateRandomCard(int cardIndex)
    {
        GameObject card = Instantiate(CardPrefab, GameplayUI.transform);
        card.SetActive(false);
        Card _cardComponent = card.GetComponent<Card>();
        int  categoryIndex  = 0;
        _cardComponent.CardImage.sprite = Cards[cardIndex]
            .CardSprite;
        foreach (Category category in _cardComponent.Categories)
        {
            category.CategoryPressed.AddListener(CategorySelected);
            string randomCategory = Cards[cardIndex]
                .Categories[categoryIndex]
                .Name;
            int randomStats = Cards[cardIndex]
                .Categories[categoryIndex]
                .Stats;
            category.Stats =  randomStats;
            category.Name  =  randomCategory;
            categoryIndex  += 1;

            category.UpdateUI();
        }

        return card;
    }

    public void CategorySelected(string categoryName, int categoryStats)
    {
        SwitchButton?.Invoke(false);
        RectTransform rectTransform;
        Vector2       startPos, endPos;
        if (!Turn)
        {
            rectTransform = deck1[AISelectedCard]
                .GetComponent<RectTransform>();
            startPos = rectTransform.anchoredPosition;
            endPos   = new Vector2(rectTransform.anchoredPosition.x - 650f, rectTransform.anchoredPosition.y);
            LastSelectedCategory = deck1[AISelectedCard]
                .GetComponent<Card>()
                .GetCategory(categoryName);
            OtherPlayerCategory = deck2[PlayerSelectedCard]
                .GetComponent<Card>()
                .GetCategory(categoryName);
        }
        else
        {
            rectTransform = deck2[PlayerSelectedCard]
                .GetComponent<RectTransform>();
            startPos = rectTransform.anchoredPosition;
            endPos   = new Vector2(rectTransform.anchoredPosition.x + 650f, rectTransform.anchoredPosition.y);
            LastSelectedCategory = deck2[PlayerSelectedCard]
                .GetComponent<Card>()
                .GetCategory(categoryName);
            OtherPlayerCategory = deck1[AISelectedCard]
                .GetComponent<Card>()
                .GetCategory(categoryName);
        }

        StartCoroutine(PlayCardAnimation(startPos, endPos, rectTransform, 0.25f));
        Invoke(nameof(RevealAICard), 0.25f);
        Invoke(nameof(CheckWinner), 2f);
    }

    private void CheckWinner()
    {
        if (LastSelectedCategory.Stats >= OtherPlayerCategory.Stats)
        {
            if (Turn)
            {
                StartCoroutine(EndTurn(true));
            }
            else
            {
                StartCoroutine(EndTurn(false));
            }
        }
        else
        {
            if (Turn)
            {
                StartCoroutine(EndTurn(false));
            }
            else
            {
                StartCoroutine(EndTurn(true));
            }
        }
    }

    private IEnumerator EndTurn(bool winner)
    {
        Vector2 endPos = new Vector2(0f, 0f);
        if (!winner)
        {
            endPos.x = -1000f;
        }
        else
        {
            endPos.x = 1000f;
        }

        RectTransform rect1 = deck1[AISelectedCard]
            .GetComponent<RectTransform>();
        RectTransform rect2 = deck2[PlayerSelectedCard]
            .GetComponent<RectTransform>();
        StartCoroutine(PlayCardAnimation(rect1.anchoredPosition, endPos, rect1, 0.25f));
        StartCoroutine(PlayCardAnimation(rect2.anchoredPosition, endPos, rect2, 0.25f));
        yield return new WaitForSeconds(1f);
        deck1[AISelectedCard]
            .SetActive(false);
        deck2[PlayerSelectedCard]
            .SetActive(false);
        rect1.anchoredPosition = new Vector2();
        rect2.anchoredPosition = new Vector2();
        if (winner)
        {
            deck2.Add(deck1[AISelectedCard]);
            deck1.Remove(deck1[AISelectedCard]);
        }
        else
        {
            deck1.Add(deck2[PlayerSelectedCard]);
            deck2.Remove(deck2[PlayerSelectedCard]);
        }

        UpdateScoreUI();

        if (deck1.Count is 0 || deck2.Count is 0)
        {
            EndGame();
        }

        else
        {
            Turn = !Turn;
            PlayTurn();
        }
    }

    private void EndGame()
    {
        GameplayUI.SetActive(false);
        ScoreUI.SetActive(false);
        TimerUI.SetActive(false);
        if (deck2.Count > deck1.Count)
        {
            EndGameText.text = "!! Player Won !!";
        }
        else if (deck1.Count > deck2.Count)
        {
            EndGameText.text = "!! AI Won !!";
        }
        else
        {
            EndGameText.text = "!! Draw !!";
        }
    }

    private void UpdateScoreUI()
    {
        PlayerScore          = deck2.Count;
        AIScore              = deck1.Count;
        PlayerScoreText.text = PlayerScore.ToString();
        AIScoreText.text     = AIScore.ToString();
    }

    public void GenerateCardPool(int deckCount)
    {
        deck1.ForEach(Destroy);
        deck2.ForEach(Destroy);
        deck1 = new();
        deck2 = new();

        for (int i = 0; i < deckCount; i++)
        {
            GameObject card = InstantiateRandomCard(i);
            if (deck1.Count == deckCount / 2)
            {
                deck2.Add(card);
            }
            else
            {
                deck1.Add(card);
            }
        }
    }

    private void PlayTurn()
    {
        if (Turn)
        {
            TurnText.text = "Player Turn!";
        }
        else
        {
            TurnText.text = "AI Turn!";
        }

        PlayerSelectedCard = Random.Range(0, deck2.Count);
        AISelectedCard     = AIbrain.SelectCard(deck1.Count);
        StartCoroutine(AnimateSelectedCards());
    }

    private void PostPlayTurn()
    {
        if (Turn)
        {
            SwitchButton?.Invoke(true);
        }
        else
        {
            SwitchButton?.Invoke(false);
        }
    }

    private IEnumerator AnimateSelectedCards()
    {
        if (!Turn)
        {
            deck2[PlayerSelectedCard]
                .transform.SetSiblingIndex(0);
            deck1[AISelectedCard]
                .transform.GetChild(0)
                .gameObject.GetComponent<CanvasGroup>()
                .alpha = 0;
        }
        else
        {
            deck1[AISelectedCard]
                .transform.SetSiblingIndex(0);
        }

        AnimateCard(deck1[AISelectedCard], 0);
        AnimateCard(deck2[PlayerSelectedCard], 1);
        yield return new WaitForSeconds(0.6f);
        PostPlayTurn();
        if (!Turn)
        {
            Invoke(nameof(PlayAITurn), 0.4f);
        }
    }

    private void RevealAICard()
    {
        deck1[AISelectedCard]
            .transform.GetChild(0)
            .gameObject.GetComponent<CanvasGroup>()
            .alpha = 1;
    }

    private void PlayAITurn()
    {
        List<string> categoryNames = Cards[0]
            .GetCategoryNames();
        Category category = deck1[AISelectedCard]
            .GetComponent<Card>()
            .GetCategory(categoryNames[AIbrain.SelectCategory()]);
        category.Button.onClick.Invoke();
    }

    private void AnimateCard(GameObject cardObject, int direction) //0->fromleft, 1->fromright
    {
        RectTransform rectTransform = cardObject.GetComponent<RectTransform>();
        Vector2       startPosition = rectTransform.anchoredPosition;
        if (direction is 0)
        {
            startPosition.x = -1000f;
        }
        else
        {
            startPosition.x = 1000f;
        }

        Vector2 endPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = startPosition;
        cardObject.SetActive(true);

        StartCoroutine(PlayCardAnimation(startPosition, endPosition, rectTransform));
    }

    private IEnumerator PlayCardAnimation(Vector2 startPos, Vector2 endPos, RectTransform rectTransform, float delay = 0.5f)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= 1f)
        {
            elapsedTime                    += Time.deltaTime / delay;
            rectTransform.anchoredPosition =  Vector2.Lerp(startPos, endPos, elapsedTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void OnTimerEnd()
    {
        EndGame();
    }

    public void StartGame(int deckCount)
    {
        GameplayUI.SetActive(true);
        ScoreUI.SetActive(true);
        TimerUI.SetActive(true);
        GenerateCardPool(deckCount);
        PlayerScore          = deckCount / 2;
        AIScore              = deckCount / 2;
        Turn                 = Random.Range(0, 10) > 5;
        PlayerScoreText.text = PlayerScore.ToString();
        AIScoreText.text     = AIScore.ToString();
        TurnText.text        = "";
        EndGameText.text     = "";
        PlayTurn();
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}