﻿namespace bfnexchange.wrBF
{
    using System;
    using System.CodeDom.Compiler;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("System.Xml", "4.6.1064.2"), XmlType(Namespace="http://forexdatawsnew.org/")]
    public enum PersistenceType
    {
        LAPSE,
        PERSIST,
        MARKET_ON_CLOSE
    }
}

