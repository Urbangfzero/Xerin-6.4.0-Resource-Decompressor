using dnlib.DotNet;
using System.Linq;

namespace Core
{
    public static class ResourceReplacer
    {
        public static void Replace(ModuleDefMD original, ModuleDefMD unpacked)
        {
            Logger.Info("Replacing resources...");

            foreach (var res in unpacked.Resources)
            {
                var existing = original.Resources.FirstOrDefault(r => r.Name == res.Name);

                if (existing != null)
                {
                    original.Resources.Remove(existing);
                    Logger.Warn("Removed: " + existing.Name);
                }

                original.Resources.Add(res);
                Logger.Success("Added: " + res.Name);
            }
        }
    }
}