using System.Collections.Generic;

public class UndoCommandMgr
{
    private Stack<IUndoableCommand> commands = new Stack<IUndoableCommand>();

    public int CommandCount { get { return this.commands.Count; } }
    public void Execute(IUndoableCommand c)
    {
        c.Execute();
        this.commands.Push(c);
    }

    public void Undo()
    {
        if (this.CommandCount == 0)
            return;

        var c = this.commands.Pop();
        c.Undo();
    }
}
