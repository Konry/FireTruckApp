// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using System.Reflection;

namespace BaseLibrary;

public static class InternalResourceLoader
{
    public static string GetTextFromEmbeddedResource(string fileName)
    {
        try
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith($".{fileName}"));

            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new EmbeddedResourceLoaderException($"Can not load the resource {resourceName}");
            }

            using StreamReader reader = new(stream);
            string result = reader.ReadToEnd();

            return result;
        }
        catch (InvalidOperationException e)
        {
            throw new EmbeddedResourceLoaderException($"Can not load file {fileName}", e);
        }
    }
}
