﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{   
    public sealed class UndoEventArgs : EventArgs
    {
        public bool ActionPossible { get; set; }
        public UndoEventArgs(bool actionPossible)
        {
            ActionPossible = actionPossible;
        }
    }
}

