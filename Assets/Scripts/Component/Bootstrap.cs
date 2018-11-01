using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyPuzzle;

public class Bootstrap
    : MonoBehaviour
{
    [SerializeField] private Transform LevelMenuNode;
    [SerializeField] private Transform QuizMenuNode;
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button ResetButton;
    [SerializeField] private ResultComponent ResultComponent;

    private GameObjectRef refs;
    private void Awake()
    {
        this.refs = this.GetComponent<GameObjectRef>();

        this.LevelMenuNode.gameObject.SetActive(false);
        this.QuizMenuNode.gameObject.SetActive(false);
        this.ResetButton.onClick.AddListener(() => { PuzzleComponent.Instance.Puzzle.Reset(); });
        this.ResultComponent.OnBack = () =>
        {
            this.ResultComponent.gameObject.SetActive(false);
            PuzzleComponent.Instance.Reset();
            this.QuizMenuNode.gameObject.SetActive(true);
        };
        this.ResultComponent.OnNext = () =>
        {
            ++this.CurrentQuizID;
            this.ResultComponent.gameObject.SetActive(false);
            this.ResetButton.gameObject.SetActive(true);
            PuzzleComponent.Instance.StartGame(this.CurrentLevel, this.CurrentQuizID);
        };

        this.InitLevelMenu();
    }

    public string CurrentLevel { get; private set; }
    private void InitLevelMenu()
    {
        var template = refs["LevelButton"];
        var levels = Config.GetDiffcultTypes();
        foreach(var l in levels)
        {
            var go = Instantiate(template);
            go.transform.SetParent(this.LevelMenuNode);
            go.GetComponentInChildren<Text>().text = this.GetLevelDesc(l);
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                this.CurrentLevel = l;
                this.InitQuizMenu();

                this.ShowOrHideLevelQuiz(false);
            });
        }
    }

    public int CurrentQuizID { get; private set; }
    private List<GameObject> quizMenus = new List<GameObject>();
    private void InitQuizMenu()
    {
        this.CleanQuizMenu();

        var template = refs["QuizButton"];
        var quizs = Config.GetQuizsNumByDifficulty(this.CurrentLevel);
        var count = quizs.Count;
        for(var i=0; i < count; ++i)
        {
            var go = Instantiate(template);
            go.transform.SetParent(this.QuizMenuNode);
            go.GetComponentInChildren<Text>().text = this.GetQuizDesc(i);
            var id = int.Parse(quizs[i]);
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                this.CurrentQuizID = id;
                this.QuizMenuNode.gameObject.SetActive(false);
                this.ResetButton.gameObject.SetActive(true);
                PuzzleComponent.Instance.StartGame(this.CurrentLevel, id);
                PuzzleComponent.Instance.OnWinEvent = this.HandleOnWin;
            });
        }
    }

    private void CleanQuizMenu()
    {
        foreach (var go in this.quizMenus)
            Destroy(go);

        this.quizMenus.Clear();
    }

    private string GetLevelDesc(string l)
    {
        return l;
    }

    private string GetQuizDesc(int i)
    {
        return i.ToString();
    }

    private void ShowOrHideLevelQuiz(bool flag)
    {
        this.LevelMenuNode.gameObject.SetActive(flag);
        this.QuizMenuNode.gameObject.SetActive(!flag);
    }

    public void Enter()
    {
        this.StartGameButton.gameObject.SetActive(false);
        this.ShowOrHideLevelQuiz(true);
    }

    private void HandleOnWin()
    {
        this.ResetButton.gameObject.SetActive(false);
        this.ResultComponent.gameObject.SetActive(true);
    }
}
