using AutoMapper;
using FileManager.Core.Models;
using FileManager.Core.Repositories;
using FileManager.Core.Validations;
using FileManager.DataContracts;
using FileManager.DataContracts.V1.File;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileManager.Core.Operations.FileOperations
{
    public class FileOperations : IFileOperations
    {
        private readonly IFileRepository _repository;
        private readonly FileValidation _validator;
        private readonly IMapper _mapper;

        public FileOperations(IFileRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _validator = new FileValidation();
        }

        public FileDataContract GetById(string key)
        {
            var data = _repository.GetById(key);

            if (data == null)
                return new FileDataContract();

            return _mapper.Map<FileDataContract>(data);
        }

        public PagedResultDataContract<FileDataContract> GetPaged(int page, int size)
        {
            if (size == 0)
                size = 10;

            var data = _repository.GetPagedFile(page, size);

            var result = new PagedResultDataContract<FileDataContract>
            {
                CurrentPage = data.CurrentPage,
                PageCount = data.PageCount,
                PageSize = data.PageSize,
                RowCount = data.RowCount,

                Results = _mapper.Map<IEnumerable<FileDataContract>>(data.Results)
            };

            return result;
        }

        public FileDataContract Insert(FileDataContract data)
        {
            var newFile = _mapper.Map<File>(data);

            ValidationResult results = _validator.Validate(newFile);

            if (!results.IsValid)
            {
                data.Error = new List<InfoDataContract>();
                foreach (var error in results.Errors)
                {
                    data.Error.Add(new InfoDataContract { Field = error.PropertyName, Message = error.ErrorMessage });
                }

                return data;
            }

            var fileResult = _repository.Insert(newFile);

            return _mapper.Map<FileDataContract>(fileResult);
        }

        public FileDataContract Update(string key, FileDataContract data)
        {
            var currentFile = _repository.GetById(key);

            if (currentFile == null)
                return null;

            var updateFile = _mapper.Map<File>(data);

            ValidationResult results = _validator.Validate(updateFile);

            if (!results.IsValid)
            {
                data.Error = new List<InfoDataContract>();
                foreach (var error in results.Errors)
                {
                    data.Error.Add(new InfoDataContract { Field = error.PropertyName, Message = error.ErrorMessage });
                }

                return data;
            }

            currentFile.Status = data.Status;
            currentFile.ProcessDate = DateTime.Now;

            var updatedFile = _repository.Update(currentFile);

            return _mapper.Map<FileDataContract>(updatedFile);
        }

        public int? Delete(string key)
        {
            var currentFile = _repository.GetById(key);

            if (currentFile == null)
                return null;

            return _repository.Delete(currentFile);
        }
    }
}
