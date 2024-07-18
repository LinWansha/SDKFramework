namespace Habby.Base
{
    public interface IReferenceCount:IReuse
    {
        uint ReferenceCount
        {
            get;
        }

        void UpCount();
        void DownCount();
    }
}