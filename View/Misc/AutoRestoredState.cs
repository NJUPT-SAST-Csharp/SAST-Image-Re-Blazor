namespace View.Misc;

public sealed class AutoRestoredState<T>(T init)
{
    public T Value { get; private set; } = init;

    public ScopedState CreateScope(T value)
    {
        return new ScopedState(this, value);
    }

    public static implicit operator T(AutoRestoredState<T> state)
    {
        return state.Value;
    }

    public static implicit operator AutoRestoredState<T>(T value)
    {
        return new AutoRestoredState<T>(value);
    }

    public readonly struct ScopedState : IDisposable
    {
        private readonly AutoRestoredState<T> stateRef;
        private readonly T initValue;

        internal ScopedState(AutoRestoredState<T> stateRef, T scopedValue)
        {
            this.stateRef = stateRef;

            initValue = stateRef.Value;
            stateRef.Value = scopedValue;
        }

        public void Dispose()
        {
            stateRef.Value = initValue;
        }
    }
}

public static class AutoRestoredStateExtensions
{
    public static AutoRestoredState<bool>.ScopedState CreateScope(
        this AutoRestoredState<bool> state
    )
    {
        return state.CreateScope(!state.Value);
    }
}
