using AutoMapper;
using FileManager.Core.Operations.FileOperations;
using FileManager.DataContracts.V1.File;
using Hangfire;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FileManager.WebApi.Jobs
{
    /// <summary>
    /// 
    /// </summary>
    public class FileManagerSchedule
    {
        private static bool _jobManagerFileInitialize = false;
        private static readonly object Sync = new object();

        private readonly IFileOperations _operations;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        public FileManagerSchedule(IFileOperations operations, IMapper mapper)
        {
            _operations = operations;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartBckgroundJob()
        {

            lock (Sync)
            {
                if (_jobManagerFileInitialize == false)
                {

                    RecurringJob.AddOrUpdate(() => ManagerFile(), Cron.Minutely());
                    _jobManagerFileInitialize = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ManagerFile()
        {
            var pagedResult = _operations.GetPaged(1, 100);
            var files = pagedResult.Results.ToList();

            byte[] buffer = new byte[4096];

            int len;

            files.ForEach(file =>
            {
                Parallel.Invoke(() =>
                {

                    file.Status = Infrastructure.Enums.FileStatus.InProcess;
                    file.ProcessDate = DateTime.Now;
                    file.InitialDate = DateTime.Now;

                    _operations.Update(file.Key.ToString(), _mapper.Map<FileDataContract>(file));
                });

                try
                {
                    using (var originStream = new FileStream($"{file.OriginPath}{file.Name}", FileMode.OpenOrCreate))
                    {
                        using (var destinyStream = new FileStream($"{file.DestinyPath}{file.Name}", FileMode.CreateNew))
                        {
                            while ((len = originStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                destinyStream.Write(buffer, 0, len);
                            }
                        }
                    }

                    Parallel.Invoke(() =>
                                   {
                                       file.FinishProcessDate = DateTime.Now;

                                       if (VerifyCheckSumBetweenFiles($"{file.OriginPath}{file.Name}", $"{file.DestinyPath}{file.Name}", out string result))
                                       {
                                           file.Checksum = result;
                                           file.Status = Infrastructure.Enums.FileStatus.Moved;
                                       }
                                       else
                                       {
                                           file.Checksum = result;
                                           file.Status = Infrastructure.Enums.FileStatus.Error;
                                       }

                                       _operations.Update(file.Key.ToString(), _mapper.Map<FileDataContract>(file));
                                   }
                     );
                }
                catch (SystemException ex)
                {
                    throw new Exception(ex.Message);
                }

            });
        }

        private bool VerifyCheckSumBetweenFiles(string originFile, string destinyFile, out string checkSumResult)
        {
            var originCkSum = GenerateCheckSum(originFile);
            var destinyCkSum = GenerateCheckSum(destinyFile);

            checkSumResult = "";

            if (string.IsNullOrEmpty(originCkSum) || string.IsNullOrEmpty(destinyCkSum))
                return false;

            if (originCkSum != destinyCkSum)
                return false;

            checkSumResult = originCkSum;
            return true;
        }

        private string GenerateCheckSum(string file)
        {
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();

            try
            {
                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);

                Byte[] hashCode = md5Provider.ComputeHash(fileStream);

                fileStream.Close();

                return BitConverter.ToString(hashCode).Replace("-", "");
            }
            catch
            {
                return "";
            }
        }
    }
}
