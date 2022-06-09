namespace bftradeline.wrBF
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("System.Xml", "4.6.1064.2"), DebuggerStepThrough, DesignerCategory("code"), XmlType(Namespace="http://forexdatawsnew.org/")]
    public class Order
    {
        private double? avgPriceMatchedField;
        private string betIdField;
        private double? bspLiabilityField;
        private bftradeline.wrBF.OrderType orderTypeField;
        private bftradeline.wrBF.PersistenceType persistenceTypeField;
        private DateTime? placedDateField;
        private double priceField;
        private bftradeline.wrBF.Side sideField;
        private double? sizeCancelledField;
        private double sizeField;
        private double? sizeLapsedField;
        private double? sizeMatchedField;
        private double? sizeRemainingField;
        private double? sizeVoidedField;
        private OrderStatus statusField;

        [XmlElement(IsNullable=true)]
        public double? AvgPriceMatched
        {
            get
            { return this.avgPriceMatchedField; }
            set
            {
                this.avgPriceMatchedField = value;
            }
        }

        public string BetId
        {
            get
            { return this.betIdField; }
            set
            {
                this.betIdField = value;
            }
        }

        [XmlElement(IsNullable=true)]
        public double? BspLiability
        {
            get
            { return this.bspLiabilityField; }
            set
            {
                this.bspLiabilityField = value;
            }
        }

        public bftradeline.wrBF.OrderType OrderType
        {
            get
            { return this.orderTypeField; }
            set
            {
                this.orderTypeField = value;
            }
        }

        public bftradeline.wrBF.PersistenceType PersistenceType
        {
            get
            { return this.persistenceTypeField; }
            set
            {
                this.persistenceTypeField = value;
            }
        }

        [XmlElement(IsNullable=true)]
        public DateTime? PlacedDate
        {
            get
            { return this.placedDateField; }
            set
            {
                this.placedDateField = value;
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

        public bftradeline.wrBF.Side Side
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

        [XmlElement(IsNullable=true)]
        public double? SizeCancelled
        {
            get
            { return this.sizeCancelledField; }
            set
            {
                this.sizeCancelledField = value;
            }
        }

        [XmlElement(IsNullable=true)]
        public double? SizeLapsed
        {
            get
            { return this.sizeLapsedField; }
            set
            {
                this.sizeLapsedField = value;
            }
        }

        [XmlElement(IsNullable=true)]
        public double? SizeMatched
        {
            get
            { return this.sizeMatchedField; }
            set
            {
                this.sizeMatchedField = value;
            }
        }

        [XmlElement(IsNullable=true)]
        public double? SizeRemaining
        {
            get
            { return this.sizeRemainingField; }
            set
            {
                this.sizeRemainingField = value;
            }
        }

        [XmlElement(IsNullable=true)]
        public double? SizeVoided
        {
            get
            { return this.sizeVoidedField; }
            set
            {
                this.sizeVoidedField = value;
            }
        }

        public OrderStatus Status
        {
            get
            { return this.statusField; }
            set
            {
                this.statusField = value;
            }
        }
    }
}

