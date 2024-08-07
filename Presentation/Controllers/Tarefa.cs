using Application.Tarefa.Interfaces;
using Application.Tarefa.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Tarefa : Controller
    {
        private readonly ITarefaServices _tarefaServices;
        public Tarefa(ITarefaServices tarefaServices)
        {
            _tarefaServices = tarefaServices;
        }

        [Route("GetList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TarefaView>))]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var result = await _tarefaServices.GetList();
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Register(TarefaView tarefaView)
        {
            try
            {

                var result = _tarefaServices.Add(tarefaView);
                var response = new { Message = result.ReasonPhrase };

                switch (result.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        return StatusCode(StatusCodes.Status400BadRequest, result.ReasonPhrase);
                    case System.Net.HttpStatusCode.InternalServerError:
                        return StatusCode(StatusCodes.Status500InternalServerError, result.ReasonPhrase);

                    default:
                        return Ok(response);
                }
            }
            catch (Exception)
            {

                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TarefaView))]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _tarefaServices.GetById(id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type=typeof(string))]
        public IActionResult Update(TarefaView tarefaView)
        {
            try
            {
                var result = _tarefaServices.Update(tarefaView);
                switch (result.StatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        return StatusCode(StatusCodes.Status400BadRequest, result.ReasonPhrase);
                    case System.Net.HttpStatusCode.NotFound:
                        return StatusCode(StatusCodes.Status404NotFound, result.ReasonPhrase);

                    default:
                        return Ok(StatusCodes.Status200OK);
                }
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("Remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult Remove(int id)
        {
            try
            {
                var result = _tarefaServices.Remove(id);
                switch (result.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return StatusCode(StatusCodes.Status404NotFound, result.ReasonPhrase);

                    default:
                        return Ok(StatusCodes.Status200OK);
                }
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
    }
}
