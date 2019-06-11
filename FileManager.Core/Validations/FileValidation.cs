using FileManager.Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Core.Validations
{
    public class FileValidation : AbstractValidator<File>
    {
        public FileValidation()
        {
            RuleFor(file => file.Name).NotNull().NotEmpty();
        }
    }
}
