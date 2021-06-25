namespace ShareClientWpf
{
    public interface IWindowCaputure
    {
        public bool TryCaputure(out byte[] data);
    }
}
