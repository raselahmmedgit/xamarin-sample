using Covi.Services;
using System.Threading;

namespace Covi.iOS.Services
{
    public class CloseApplication : ICloseApplication
    {
        public void Close()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
