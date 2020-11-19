using AutoMapper;
using EMenuApplication.Models;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CRM_CommentCard, CommentCard_VM>();

            CreateMap<CRM_CommentCardQuestions, CommentCardQuestions_VM>();

            CreateMap<CRM_CQuestions, CQuestions_VM>();

            CreateMap<CRM_CQuestionsGroups, CQuestionsGroups_VM>();

            CreateMap<Sys_Fields, Sys_Fields_VM>();

        }
    }
}
