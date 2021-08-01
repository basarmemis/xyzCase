using System;

namespace Case.Akka
{
    public interface IDummyLogger : IDisposable
    {
        bool IsDisposed { get; }
        string PrintLog(string str);
    }

    public sealed class DummyLogger : IDummyLogger
    {
        private bool _isDisposed;

        public void Dispose()
        {
            _isDisposed = true;
        }
        public bool IsDisposed => _isDisposed;

        public string PrintLog(string str)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("ActorServices disposed");

            Console.WriteLine(str);
            return str;
        }
    }
}