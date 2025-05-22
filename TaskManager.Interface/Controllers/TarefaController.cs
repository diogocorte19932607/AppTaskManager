using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Interface.Model;

namespace TaskManager.Interface.Controllers
{
    public class TarefaController : BaseController
    {
        private readonly ILogger<TarefaController> _logger;
        public TarefaController(ILogger<TarefaController> logger, IConfiguration configuration, IHttpClientFactory httpClient) : base(configuration, httpClient)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListarTarefas()
        {
            await GetToken();
            var getTarefa = await _httpClient.GetAsync("api/Tarefa/Listar");
            if (!getTarefa.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getTarefa.ToString());
            }
            var resultTarefa = JsonConvert.DeserializeObject<List<DtoTarefa>>(await getTarefa.Content.ReadAsStringAsync());
            return Json(resultTarefa);
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            await GetToken();
            var getTarefa = await _httpClient.GetAsync("api/Tarefa/RetornaPorId/" + id.ToString());
            if (!getTarefa.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getTarefa.ToString());
            }
            var resultCliente = JsonConvert.DeserializeObject<DtoTarefa>(await getTarefa.Content.ReadAsStringAsync());
            return View(resultCliente);
        }

        [HttpPost]
        public async Task<IActionResult> CriarTarefa([FromBody] DtotarefaCreate model)
        {
            try
            {
                await GetToken();

                var jsonContent = JsonConvert.SerializeObject(model);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Tarefa/Adicionar", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { sucesso = true, ret = await response.Content.ReadAsStringAsync() });
                }
                else
                {
                    return Json(new { sucesso = false, ret = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public async Task<bool> EditarTarefa([FromBody] DtotarefaEdit model)
        {
            await GetToken();

            var jsonContent = JsonConvert.SerializeObject(model);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Tarefa/Atualizar", contentString);

            return response.IsSuccessStatusCode;
        }
        [HttpDelete("DeletarTarefa/{id}")]
        public async Task<bool> DeletarTarefa(string id)
        {
            await GetToken();

            var response = await _httpClient.DeleteAsync(_configuration["ApiURL"] + "api/Tarefa/Deletar/" + id);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
