using dnlib.DotNet;
using System.Linq;

namespace Core
{
    public static class ResourceHelper
    {
        public static EmbeddedResource GetEncryptedResource(ModuleDefMD module)
        {
            var res = module.Resources
                .OfType<EmbeddedResource>()
                .FirstOrDefault(r => r.Name.Length > 5);

            if (res == null)
                Logger.Error("No resource found!");
            else
                Logger.Success("Found resource: " + res.Name);

            return res;
        }
    }
}