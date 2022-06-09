namespace bfnexchange.wrBF
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("System.Xml", "4.6.1064.2"), DebuggerStepThrough, DesignerCategory("code"), XmlType(Namespace="http://forexdatawsnew.org/")]
    public class Runner
    {
        private double? adjustmentFactorField;
        private bfnexchange.wrBF.ExchangePrices exchangePricesField;
        private double? handicapField;
        private double? lastPriceTradedField;
        private Match[] matchesField;
        private Order[] ordersField;
        private DateTime? removalDateField;
        private long selectionIdField;
        private bfnexchange.wrBF.StartingPrices startingPricesField;
        private RunnerStatus statusField;
        private double totalMatchedField;

        [XmlElement(IsNullable=true)]
        public double? AdjustmentFactor
        {
            get { return this.adjustmentFactorField; }
            set
            {
                this.adjustmentFactorField = value;
            }
        }

        public bfnexchange.wrBF.ExchangePrices ExchangePrices
        {
            get { return this.exchangePricesField; }
            set
            {
                this.exchangePricesField = value;
            }
        }

        [XmlElement(IsNullable=true)]
        public double? Handicap
        {
            get { return this.handicapField; }
            set
            {
                this.handicapField = value;
            }
        }

        [XmlElement(IsNullable=true)]
        public double? LastPriceTraded
        {
            get { return this.lastPriceTradedField; }
            set
            {
                this.lastPriceTradedField = value;
            }
        }

        public Match[] Matches
        {
            get
            {
            return    this.matchesField;
            }
            set
            {
                this.matchesField = value;
            }
        }

        public Order[] Orders
        {
            get { return this.ordersField; }
            set
            {
                this.ordersField = value;
            }
        }

        [XmlElement(IsNullable=true)]
        public DateTime? RemovalDate
        {
            get { return this.removalDateField; }
            set
            {
                this.removalDateField = value;
            }
        }

        public long SelectionId
        {
            get { return this.selectionIdField; }
            set
            {
                this.selectionIdField = value;
            }
        }

        public bfnexchange.wrBF.StartingPrices StartingPrices
        {
            get { return this.startingPricesField; }
            set
            {
                this.startingPricesField = value;
            }
        }

        public RunnerStatus Status
        {
            get { return this.statusField; }
            set
            {
                this.statusField = value;
            }
        }

        public double TotalMatched
        {
            get { return this.totalMatchedField; }
            set
            {
                this.totalMatchedField = value;
            }
        }
    }
}

