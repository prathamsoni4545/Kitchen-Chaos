using System;

public interface IHasProgress
{
    event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
        public bool showProgress;
    }
}
