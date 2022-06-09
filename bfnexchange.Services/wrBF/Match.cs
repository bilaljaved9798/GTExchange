namespace bfnexchange.wrBF
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("System.Xml", "4.6.1064.2"), DebuggerStepThrough, DesignerCategory("code"), XmlType(Namespace="http://forexdatawsnew.org/")]
    public class Match
    {
        private string betIdField;
        private DateTime matchDateField;
        private double priceField;
        private bfnexchange.wrBF.Side sideField;
        private double sizeField;

        public string BetId
        {
            get
            { return this.betIdField; }
            set
            {
                this.betIdField = value;
            }
        }

        public DateTime MatchDate
        {
            get
            { return this.matchDateField; }
            set
            {
                this.matchDateField = value;
            }
        }

        public double Price
        {
            get
            { return this.priceField; }
            set
            {
                this.priceField = value;
            }
        }

        public bfnexchange.wrBF.Side Side
        {
            get
            { return this.sideField; }
            set
            {
                this.sideField = value;
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

