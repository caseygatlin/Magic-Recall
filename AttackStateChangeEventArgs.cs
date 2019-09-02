using System;

namespace out_and_back
{
    enum AttackState
    {
        AbleInRange,
        AbleOutOfRange,
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
