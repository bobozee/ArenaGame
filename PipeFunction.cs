public class PipeFunction<T>(T initial) {
    T _intermediate = initial;
    public PipeFunction<T> pipe(Func<T, T> func) {
        return new PipeFunction<T>(func(_intermediate));
    }

    public T finish() { return _intermediate; }
}

public class Pipeline<P, T>(List<Func<P, T, T>> pipes) {
    
    private readonly List<Func<P, T, T>> _pipes = pipes;

    public void addPipe(Func<P, T, T> pipe) { _pipes.Add(pipe); }
    public void removePipe(Func<P, T, T> pipe) { _pipes.Remove(pipe); }
    public bool hasPipe(Func<P, T, T> pipe) { return _pipes.Contains(pipe); }

    public T execute(T initial, P p) {
        T _intermediate = initial;
        foreach (Func<P, T, T> pipe in _pipes) {
            _intermediate = pipe(p, _intermediate);
        }
        return _intermediate;
    }
}

public class Pipeline<T>(List<Func<T, T>> pipes) {
    
    private readonly List<Func<T, T>> _pipes = pipes;

    public void addPipe(Func<T, T> pipe) { _pipes.Add(pipe); }
    public void removePipe(Func<T, T> pipe) { _pipes.Remove(pipe); }
    public bool hasPipe(Func<T, T> pipe) { return _pipes.Contains(pipe); }

    public T execute(T initial) {
        T _intermediate = initial;
        foreach (Func<T, T> pipe in _pipes) {
            _intermediate = pipe(_intermediate);
        }
        return _intermediate;
    }
}