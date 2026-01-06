namespace TempleOfDoom.Core
{
    // Dit interface zorgt dat we levels kunnen inladen.
    public interface ILevelLoader
    {
        // Laad het level in van het opgegeven pad.
        Level LoadLevel(string path);
    }
}
