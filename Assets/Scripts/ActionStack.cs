using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionStack
{
    private Stack<(Action execute, Action undo)> actionStack = new Stack<(Action, Action)>();
    private Stack<(Action execute, Action undo)> redoStack = new Stack<(Action, Action)>();

    public void Do(Action execute, Action undo)
    {
        execute.Invoke();
        actionStack.Push((execute, undo));
        redoStack.Clear();
    }

    public void Undo()
    {
        if (actionStack.Count > 0)
        {
            var action = actionStack.Pop();
            action.undo.Invoke();
            redoStack.Push(action);
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            var action = redoStack.Pop();
            action.execute.Invoke();
            actionStack.Push(action);
        }
    }

    public void Clear()
    {
        actionStack.Clear();
        redoStack.Clear();
    }
}
