using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Core
{
    public static class KeyDetector
    {
        public static bool DetectKey(ModuleDefMD module, out int key)
        {
            key = 0;

            foreach (var type in module.Types)
            {
                foreach (var method in type.Methods)
                {
                    if (!method.HasBody) continue;

                    var instr = method.Body.Instructions;

                    for (int i = 0; i < instr.Count - 1; i++)
                    {
                        if (!instr[i].IsLdcI4())
                            continue;

                        if (instr[i + 1].OpCode != OpCodes.Call)
                            continue;

                        var called = instr[i + 1].Operand as IMethod;
                        if (called == null) continue;

                        var sig = called.MethodSig;
                        if (sig == null) continue;

                        if (sig.Params.Count == 2 &&
                            sig.Params[0].FullName == "System.Byte[]" &&
                            sig.Params[1].FullName == "System.Int32" &&
                            sig.RetType.FullName == "System.Byte[]")
                        {
                            int value = instr[i].GetLdcI4Value();

                            if (value <= 0 || value > 100)
                                continue;

                            key = value;
                            Logger.Success("Key detected: " + key);
                            return true;
                        }
                    }
                }
            }

            Logger.Error("Key detection failed.");
            return false;
        }
    }
}