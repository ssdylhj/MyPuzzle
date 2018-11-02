using UnityEngine;
using UnityEngine.UI;
using MyPuzzle;

public class TopMenuComponent
    : MonoBehaviour
{
    private GameObjectRef refs;

    private void Awake()
    {
        this.refs = this.GetComponent<GameObjectRef>();
        this.refs["Back"].GetComponent<Button>().onClick.AddListener(() => Bootstrap.Instance.UndoCommandMgr.Undo());
    }

    private void Update()
    {
        this.ShowOrHideBackButton();
    }

    private void ShowOrHideBackButton()
    {
        this.refs["Back"].SetActiveEx(Bootstrap.Instance.UndoCommandMgr.CommandCount > 1);
    }
}
