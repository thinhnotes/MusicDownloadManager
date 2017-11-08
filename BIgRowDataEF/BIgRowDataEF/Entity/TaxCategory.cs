namespace BigRowDataEF.Entity
{
    public class TaxCategory
    {
        public int Id { get; set; }
        public string State { get; set; }

        [ColumnDescription("Zip Code")]
        public string ZipCode { get; set; }

        public string Extension { get; set; }

        [ColumnDescription("Country Tax")]
        public string CountryTax { get; set; }

        [ColumnDescription("State Tax")]
        public string StateTax { get; set; }

        [ColumnDescription("County Tax")]
        public string CountyTax { get; set; }

        [ColumnDescription("City Tax")]
        public string CityTax { get; set; }
    }
}