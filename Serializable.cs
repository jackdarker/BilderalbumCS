﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JKFlow.Core
{
    /// <summary>
    /// Interface for serializable Objects.
    /// Cooperates with SerializerBase.
    /// </summary>
    interface ISerializable
    {
        void WriteToSerializer(SerializerBase Stream);
        void ReadFromSerializer(SerializerBase Stream);
    }
}
