using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Dialog
    {
        public List<DialogLine> Lines;

        public DialogLine CurrentLine;

        public int index = -1;

        public bool MoveNext()
        {
            index++;

            if (Lines == null || index >= Lines.Count)
            {
                CurrentLine = null;
                return false;
            }

            CurrentLine = Lines[index];
            return true;
        }
    }
}