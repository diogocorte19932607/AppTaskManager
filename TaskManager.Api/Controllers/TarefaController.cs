using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Domain;
using TaskManager.Domain.Service;
using TaskManager.Infra.Entity;

namespace TaskManager.Api.Controllers
{
    [Route("api/Tarefa")]
    [Authorize]
    public class TarefaController : Controller
    {
        private readonly TarefaService<TarefaModel, Tarefa> _tarefa;

        public TarefaController(TarefaService<TarefaModel, Tarefa> tarefaServico)
        {
            _tarefa = tarefaServico;
        }

        [HttpPost("Adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] TarefaCreateModel tarefa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (tarefa == null)
                return BadRequest();

            var tarefaResposta = await _tarefa.AdicionarTarefa(tarefa);

            if (tarefaResposta == null)
            {
                return StatusCode(500, "Erro ao adicionar Candidato!");
            }
            if (tarefaResposta.ExibicaoMensagem != null)
            {
                return StatusCode(tarefaResposta.ExibicaoMensagem.StatusCode, tarefaResposta);
            }

            return Ok(tarefaResposta);

        }


        [HttpPost("Atualizar")]
        public async Task<IActionResult> Atualizar([FromBody] TarefaEditModel tarefa)
        {

            if (tarefa == null)
                return BadRequest();

            var tarefaResposta = await _tarefa.AtualizarTarefa(tarefa);

            if (tarefaResposta == null)
            {
                return StatusCode(500, "Erro ao atualizar Candidato!");
            }
            if (tarefaResposta.ExibicaoMensagem != null)
            {
                return StatusCode(tarefaResposta.ExibicaoMensagem.StatusCode, tarefaResposta);
            }

            return Ok(tarefaResposta);
        }

        [HttpDelete("Deletar/{id}")]
        public async Task<IActionResult> DeletarTarefa(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var resultado = await _tarefa.DeletarTarefa(id);
            return Ok();
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> ListarTodas()
        {


            var clientes = await _tarefa.ListarTarefa();

            return Ok(clientes);
        }

        [HttpGet("RetornaPorId/{id}")]
        public async Task<IActionResult> RetornaPorId(Guid id)
        {

            var clientes = await _tarefa.ListarTarefaPorId(id);

            return Ok(clientes.FirstOrDefault());
        }
    }
}
