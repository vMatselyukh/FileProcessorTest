﻿using Bll.Helpers;
using Domain.Interfaces;
using Domain.Models;
using FileProcessor.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploaderController : ControllerBase
    {
        private const int BYTES_IN_MEGABYTE = 1024 * 1024;

        private readonly IFileManager _fileManager;

        public FileUploaderController(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        [Route("uploadfile")]
        [HttpPost]
        public async Task<ActionResult> UploadFile([FromForm(Name = "File")] IFormFile file)
        {
            if (file.Length > BYTES_IN_MEGABYTE)
            {
                return BadRequest("Uploaded file is too big. Max is 1 Mb");
            }            

            try
            {
                var fileContent = await FileHelper.ReadAsStringAsync(file);

                var fileProcessResult = await _fileManager.ProcessFileAsync(fileContent, Path.GetExtension(file.FileName));

                if (fileProcessResult.IsSucceed)
                {
                    return Ok(new { ProcessedTransactions = fileProcessResult.ProcessedTransactions });
                }
                else
                {
                    return BadRequest(new { ErrorMessage = fileProcessResult.ErrorMessage });
                }
            }
            catch (FormatException)
            {
                return BadRequest("File format is incorrect");
            }
        }
    }
}