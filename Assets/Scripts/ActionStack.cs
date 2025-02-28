using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionStack
{
    private Stack<(Action execute, Action undo, bool isPauseAction)> actionStack = new Stack<(Action, Action, bool)>();
    private Stack<(Action execute, Action undo, bool isPauseAction)> redoStack = new Stack<(Action, Action, bool)>();

    public void Do(Action execute, Action undo, bool isPauseAction = false)
    {
        execute.Invoke();
        actionStack.Push((execute, undo, isPauseAction));
        redoStack.Clear();
    }

    public void Undo()
    {
        while (actionStack.Count > 0)
        {
            var action = actionStack.Pop();
            action.undo.Invoke();

            if (!action.isPauseAction)
            {
                redoStack.Push(action);
                break;
            }
        }
    }

    public void Redo()
    {
        while (redoStack.Count > 0) 
        {
            var action = redoStack.Pop();
            action.execute.Invoke();

            if (!action.isPauseAction)
            {
                actionStack.Push(action);
                break; 
            }
        }
    }



    public void Clear()
    {
        actionStack.Clear();
        redoStack.Clear();
    }

    public bool IsEmpty()
    {
        return actionStack.Count == 0;
    }
}
