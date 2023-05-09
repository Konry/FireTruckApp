// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using System.Reflection;

namespace BaseLibrary;

public static class InternalResourceLoader
{
    /// <summary>
    ///     Looks for an embedded resource in the calling assembly and returns the content as a text => usable for json test
    ///     files
    ///     You can add the folder by adding a "." between folder and filename
    ///     filename.txt => looks in the assembly for filename.txt, returns the text of one
    ///     folder.filename.txt => looks for files in the assembly in the corresponding folder, they can also be in an other
    ///     folder
    ///     Warning if there are several resources with the same name this will fail
    /// </summary>
    /// <param name="fileName">the file name to look for, can be with the folder</param>
    /// <returns></returns>
    /// <exception cref="EmbeddedResourceLoaderException">
    ///     When file not exists or more than one file has the same name, or any
    ///     other exception which occurred in this
    /// </exception>
    public static string GetTextFromEmbeddedResource(string fileName)
    {
        try
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("." + fileName));

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
