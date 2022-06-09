namespace bftradeline.wrBF
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("System.Xml", "4.6.1064.2"), DebuggerStepThrough, DesignerCategory("code"), XmlType(Namespace="http://forexdatawsnew.org/")]
    public class StartingPrices
    {
        private double actualSPField;
        private PriceSize[] backStakeTakenField;
        private double farPriceField;
        private PriceSize[] layLiabilityTakenField;
        private double nearPriceField;

        public double ActualSP
        {
            get {
                return this.actualSPField; }
            set
            {
                this.actualSPField = value;
            }
        }

        public PriceSize[] BackStakeTaken
        {
            get
            { return this.backStakeTakenField; }
            set
            {
                this.backStakeTakenField = value;
            }
        }

        public double FarPrice
        {
            get
            { return this.farPriceField; }
            set
            {
                this.farPriceField = value;
            }
        }

        public PriceSize[] LayLiabilityTaken
        {
            get
            { return this.layLiabilityTakenField; }
            set
            {
                this.layLiabilityTakenField = value;
            }
        }

        public double NearPrice
        {
            get
            { return this.nearPriceField; }
            set
            {
                this.nearPriceField = value;
            }
        }
    }
}

