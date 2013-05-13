
namespace AppGrid.Signals
{
    public enum SignalTypes
    {
        SIGKILL=9,// Terminate NOW!!!
        SIGQUIT=3, // Send to persistence
        SIGTERM = 15,// Terminate nicely
        SIGSTOP = 19, // Pause processing
        SIGCONT=18,  //Resume processing
        SIGSRVSTP=99, //Server will stop
        SIGMSG=101 // server message
    }
}