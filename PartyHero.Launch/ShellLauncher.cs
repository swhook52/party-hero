using System.Diagnostics;

namespace PartyHero.Launch
{
    public class ShellLauncher : ILauncher
    {
        public void Launch(string application, string arguments = null)
        {
            Process.Start(new ProcessStartInfo(application, arguments));
        }
    }
}
