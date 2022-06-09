namespace bftradeline.wrBF
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("System.Xml", "4.6.1064.2"), DebuggerStepThrough, DesignerCategory("code"), XmlType(Namespace="http://forexdatawsnew.org/")]
    public class ExchangePrices
    {
        private PriceSize[] availableToBackField;
        private PriceSize[] availableToLayField;
        private PriceSize[] tradedVolumeField;

        public PriceSize[] AvailableToBack
        {
            get
            { return this.availableToBackField; }
            set
            {
                this.availableToBackField = value;
            }
        }

        public PriceSize[] AvailableToLay
        {
            get
            { return this.availableToLayField; }
            set
            {
                this.availableToLayField = value;
            }
        }

        public PriceSize[] TradedVolume
        {
            get
            { return this.tradedVolumeField; }
            set
            {
                this.tradedVolumeField = value;
            }
        }
    }
}

