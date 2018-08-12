



public interface IState<E> {
    void EnterState(E controller);
    void ExecuteState(E controller);
    void ExitState(E controller);
}
