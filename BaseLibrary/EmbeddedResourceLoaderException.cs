// Copyright (c) Jan Philipp Luehrig.All rights reserved.
// These files are licensed to you under the MIT license.

using System.Runtime.Serialization;

namespace BaseLibrary;

[Serializable]
public class EmbeddedResourceLoaderException : Exception
{
    //
    // For guidelines regarding the creation of new exception types, see
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    // and
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //

    public EmbeddedResourceLoaderException() { }
    public EmbeddedResourceLoaderException(string message) : base(message) { }
    public EmbeddedResourceLoaderException(string message, Exception inner) : base(message, inner) { }

    protected EmbeddedResourceLoaderException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
