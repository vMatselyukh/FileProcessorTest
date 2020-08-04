using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Domain.Models.CSV;
using Domain.Models.XML;

namespace Bll.Automapper
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            //csv transaction <-> common transaction
            CreateMap<CsvTransaction, Transaction>()
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.Currency.ToString()))
                .AfterMap((src, dest) => {
                    switch (src.Status)
                    {
                        case CsvTransactionStatusEnum.Approved:
                            dest.Status = TransactionStatusEnum.A;
                            break;
                        case CsvTransactionStatusEnum.Failed:
                            dest.Status = TransactionStatusEnum.R;
                            break;
                        case CsvTransactionStatusEnum.Finished:
                            dest.Status = TransactionStatusEnum.D;
                            break;
                    }
                });

            //xml transaction <-> common transaction
            CreateMap<XmlTransaction, Transaction>()
                .ForMember(d => d.Currency, opt => opt.MapFrom(s => s.CurrencyCode.ToString()))
                .AfterMap((src, dest) => {
                    switch (src.Status)
                    {
                        case XmlTransactionStatusEnum.Approved:
                            dest.Status = TransactionStatusEnum.A;
                            break;
                        case XmlTransactionStatusEnum.Rejected:
                            dest.Status = TransactionStatusEnum.R;
                            break;
                        case XmlTransactionStatusEnum.Done:
                            dest.Status = TransactionStatusEnum.D;
                            break;
                    }
                });

            //common transaction <-> ef transaction
            CreateMap<Transaction, EfContext.Transaction>();
            CreateMap<EfContext.Transaction, Transaction>();
        }
    }
}
