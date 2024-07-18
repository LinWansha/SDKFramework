using System.Runtime.Serialization;

namespace Habby.Base
{
    public interface IReuse
    {
        void Reset();
        void Dispose();
    }
}