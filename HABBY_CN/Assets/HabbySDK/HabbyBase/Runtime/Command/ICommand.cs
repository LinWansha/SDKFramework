namespace Habby.Command
{
    public interface ICommand
    {
        void Excute();

        string ExcuteEventName
        {
            get;
        }
        
    }
}