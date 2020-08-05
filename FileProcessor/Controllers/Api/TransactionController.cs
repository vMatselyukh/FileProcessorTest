using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Interfaces.Managers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FileProcessor.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionManager _transactionManager;
        public TransactionController(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        [Route("getall")]
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(JsonConvert.SerializeObject(_transactionManager.GetAll()));
        }

        [Route("getbystatus")]
        [HttpGet]
        public ActionResult GetByStatus(TransactionStatusEnum status)
        {
            return Ok(JsonConvert.SerializeObject(_transactionManager.GetByStatus(status)));
        }

        [Route("getbycurrency")]
        [HttpGet]
        public ActionResult GetByCurrency(CurrencyEnum currency)
        {
            return Ok(JsonConvert.SerializeObject(_transactionManager.GetByCurrency(currency)));
        }

        [Route("getbydaterange")]
        [HttpGet]
        public ActionResult GetByDateRange([Required]DateTime beginDate, [Required]DateTime endDate)
        {
            return Ok(JsonConvert.SerializeObject(_transactionManager.GetByDateRange(beginDate, endDate)));
        }
    }
}