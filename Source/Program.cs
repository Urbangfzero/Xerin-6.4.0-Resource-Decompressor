using Core;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

internal class Program
{
    static void Main(string[] args)
    {
        Console.Title = " Xerin v6.4.0 Resource Decompressor";
        Logger.Banner();

        if (!Loader.ValidateArguments(args, out string filePath))
            return;

        if (!Loader.LoadModule(filePath, out ModuleDefMD module))
            return;

        var resource = ResourceHelper.GetEncryptedResource(module);
        if (resource == null) return;

        byte[] encrypted = resource.CreateReader().ToArray();

        if (!KeyDetector.DetectKey(module, out int key))
        {
            Logger.Warn("Falling back to default key: 11");
            key = 11;
        }

        Logger.Info("Using Key: " + key);

        if (!Decryptor.Decrypt(encrypted, key, out byte[] decrypted))
            return;

        if (!Decompressor.Decompress(decrypted, out byte[] unpacked))
            return;

        if (!Loader.LoadUnpackedModule(unpacked, out ModuleDefMD unpackedModule))
            return;

        ResourceReplacer.Replace(module, unpackedModule);

        Saver.Save(module, filePath);
    }
    public static class Loader
    {
        public static bool ValidateArguments(string[] args, out string filePath)
        {
            filePath = null;

            if (args.Length == 0)
            {
                Logger.Error("Drag & drop EXE onto this tool.");
                Console.ReadKey();
                return false;
            }

            filePath = args[0];

            if (!File.Exists(filePath))
            {
                Logger.Error("File not found.");
                return false;
            }

            return true;
        }

        public static bool LoadModule(string path, out ModuleDefMD module)
        {
            module = null;
            try
            {
                Logger.Info("Loading assembly...");
                module = ModuleDefMD.Load(path);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
            }
        }

        public static bool LoadUnpackedModule(byte[] data, out ModuleDefMD module)
        {
            module = null;
            try
            {
                Logger.Info("Loading unpacked assembly...");
                module = ModuleDefMD.Load(data);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
            }
        }
    }
    public static class Saver
    {

        public static void Save(ModuleDefMD module, string input)
        {
            string output = Path.Combine(
                Path.GetDirectoryName(input),
                Path.GetFileNameWithoutExtension(input) + "-cleaned.exe"
            );
            var TF = new ModuleWriterOptions(module)
            {
                MetadataOptions = { Flags = MetadataFlags.KeepOldMaxStack | MetadataFlags.PreserveAll },
                Logger = DummyLogger.NoThrowInstance
            };

            module.Write(output,TF);
            Logger.Success("Saved: " + output);
        }
    }
}