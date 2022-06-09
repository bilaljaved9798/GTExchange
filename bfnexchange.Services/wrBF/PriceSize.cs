namespace bfnexchange.wrBF
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("System.Xml", "4.6.1064.2"), DebuggerStepThrough, DesignerCategory("code"), XmlType(Namespace="http://forexdatawsnew.org/")]
    public class PriceSize
    {
        private double priceField;
        private double sizeField;

        public double Price
        {
            get
            { return this.priceField; }
            set
            {
                this.priceField = value;
            }
        }

        public double Size
        {
            get
            { return this.sizeField; }
            set
            {
                this.sizeField = value;
            }
        }
    }
}

