using AutoMapper;
using CommandSnippetManager.Data;
using CommandSnippetManager.Dtos;
using CommandSnippetManager.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandSnippetManager.Controllers
{
    //api commands
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Get api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        // Get api/commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {

            var commandItems = _repository.GetCommandById(id);
            if (commandItems != null)
                return Ok(_mapper.Map<CommandReadDto>(commandItems));
            return NotFound();
        }

        //Post api/Commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            // return Ok(commandReadDto);
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }


        //Put api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelfromRepo = _repository.GetCommandById(id);
            if (commandModelfromRepo == null)
                return NotFound();
            _mapper.Map(commandUpdateDto, commandModelfromRepo);
            _repository.UpdateCommand(commandModelfromRepo);
            _repository.SaveChanges();
            return NoContent();
        }

        //Patch api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelfromRepo = _repository.GetCommandById(id);
            if (commandModelfromRepo == null)
            { return NotFound(); }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelfromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch)) 
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelfromRepo);
            _repository.UpdateCommand(commandModelfromRepo);
            _repository.SaveChanges();
            return NoContent();
        }

        //Delete api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelfromRepo = _repository.GetCommandById(id);
            if (commandModelfromRepo == null)
            { return NotFound(); }
            _repository.DeleteCommand(commandModelfromRepo);
            _repository.SaveChanges();
            return NoContent();
        }


    }
}
