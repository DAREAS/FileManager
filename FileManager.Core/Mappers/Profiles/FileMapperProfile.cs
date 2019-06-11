using AutoMapper;
using FileManager.Core.Models;
using FileManager.DataContracts.V1.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Core.Mappers.Profiles
{
    public class FileMapperProfile : Profile
    {
        public FileMapperProfile()
        {
            MapRequestsToModels();
            MapModelsToResponse();
        }

        private void MapRequestsToModels()
        {
            CreateMap<FileDataContract, File>();
        }

        private void MapModelsToResponse()
        {
            CreateMap<File, FileDataContract>()
                .ForMember(
                    target => target.Error,
                    option => option.Ignore()
                );
        }
    }
}
