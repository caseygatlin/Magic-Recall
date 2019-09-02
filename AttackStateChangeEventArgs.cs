using System;

namespace out_and_back
{
    enum AttackState
    {
        Able,
        Unable
    }

    class AttackStateChangeEventArgs : EventArgs
    {
        public AttackState AttackState;

        public AttackStateChangeEventArgs(AttackState state) : base()
        {
            AttackState = state;
        }
    }
}
