using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionStack
{
    private Stack<Action> actionStack = new Stack<Action>();
    private Stack<Action> redoStack = new Stack<Action>();

    public void Do(Action execute, Action undo)
    {
        execute.Invoke();
        actionStack.Push(undo);
        redoStack.Clear();
    }

    public void Undo()
    {
        if (actionStack.Count > 0)
        {
            Action undoAction = actionStack.Pop();
            undoAction.Invoke();
            redoStack.Push(undoAction);
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            Action redoAction = redoStack.Pop();
            redoAction.Invoke();
            actionStack.Push(redoAction);
        }
    }
}