using AutoMapper;
using Domain;
using Domain.Models;
using Domain.Models.CSV;

namespace Bll.Automapper
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<CsvTransaction, Transaction>()
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
           
            CreateMap<Transaction, CsvTransaction>();
        }
    }
}
