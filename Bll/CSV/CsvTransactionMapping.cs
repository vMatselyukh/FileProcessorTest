using Domain.Models.CSV;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace Bll.CSV
{
    public class CsvTransactionMapping : CsvMapping<CsvTransaction>
    {
        public CsvTransactionMapping()
            : base()
        {
            MapProperty(0, x => x.TransactionId);
            MapProperty(1, x => x.Amount);
            MapProperty(2, x => x.Currency);
            MapProperty(3, x => x.Date, new DateTimeConverter("dd/MM/yyyy hh:mm:ss"));
            MapProperty(4, x => x.Status, new EnumConverter<CsvTransactionStatusEnum>());
        }
    }
}
