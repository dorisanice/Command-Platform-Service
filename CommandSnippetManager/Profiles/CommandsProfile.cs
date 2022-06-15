using AutoMapper;
using CommandSnippetManager.Dtos;
using CommandSnippetManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandSnippetManager.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Soure -> Target
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<CommandUpdateDto, Command>();
            CreateMap<Command, CommandUpdateDto>();
        }
    }
}
