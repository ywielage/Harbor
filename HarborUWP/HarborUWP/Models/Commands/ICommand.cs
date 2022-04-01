namespace HarborUWP.Models.Commands
{
    internal interface ICommand
    {
        void Execute(Models.Application application);
    }
}
