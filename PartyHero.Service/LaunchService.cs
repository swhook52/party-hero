using PartyHero.Launch;

namespace PartyHero.Service
{
    public interface ILaunchService
    {
        void LaunchGame(string gameName, string systemName);
    }

    public class LaunchService : ILaunchService
    {
        private ILauncher _launcher;

        public LaunchService(ILauncher launcher)
        {
            _launcher = launcher;
        }

        public void LaunchGame(string gameName, string systemName)
        {
            _launcher.Launch(gameName);
        }
    }
}
