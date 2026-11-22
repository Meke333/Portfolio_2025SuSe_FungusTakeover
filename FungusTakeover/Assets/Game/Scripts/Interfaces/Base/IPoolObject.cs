namespace Interface
{
    public interface IPoolObject
    {
        //public void TransmitPoolInterface(IPoolManager ipoolManager);
        public bool IsGameObjectActive();
        public void SetActivity(bool value);

        public void ResetObject();
    }

}

