using System;
using System.Windows.Input;

namespace SharpUtils
{
    public class KeyboardHooksEventArgs : EventArgs
    {
        public readonly Key Key;

        public KeyboardHooksEventArgs(Key key)
        {
            this.Key = key;
        }
    }
}