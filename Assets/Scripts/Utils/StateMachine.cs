using System;

namespace Utils
{
  public class StateMachine<T> where T : struct, Enum
  {
    static StateMachine() {
      if (!typeof(T).IsDefined(typeof(FlagsAttribute), false)) {
        throw new InvalidOperationException($"{typeof(T).Name} must contain the [Flags] attribute");
      }
    }

    private int _state;

    public void Add(T state) {
      _state |= Convert.ToInt32(state);
    }

    public void Remove(T state) {
      _state &= ~Convert.ToInt32(state);
    }

    public void Toggle(T state) {
      _state ^= Convert.ToInt32(state);
    }

    public void Clear() {
      _state = 0;
    }

    public bool Has(T state) {
      var intState = Convert.ToInt32(state);
      return (_state & intState) == intState;
    }
  }
}
