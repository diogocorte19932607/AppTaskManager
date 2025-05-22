using AutoMapper;
using Microsoft.Extensions.Options;
using TaskManager.Domain.Configuration;
using TaskManager.Domain.Domain;
using TaskManager.Domain.Service.Generic;
using TaskManager.Infra.Entity;
using TaskManager.Infra.Repositories.Interfaces;
using TaskManager.Infra.UnitofWork;

namespace TaskManager.Domain.Service
{
    public class TarefaService<Tv, Te> : GenericServiceAsync<Tv, Te>
                                               where Tv : TarefaModel
                                               where Te : Tarefa
    {
        ITarefaRepository _tarefaRepository;
        private readonly AppSettings _appSettings;
        public TarefaService(IUnitofWork unitOfWork, IMapper mapper,
                             ITarefaRepository tarefaRepository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tarefaRepository = tarefaRepository;
        }
        public async Task<Tarefa> ModelarTarefa(TarefaCreateModel tarefa)
        {
            var result = new Tarefa
            {
                Title = tarefa.Title,
                CreatedAt = tarefa.CreatedAt,
                Description = tarefa.Description,
                Status = tarefa.Status
            };
            var tempId = Guid.NewGuid();
            return result;
        }
        public async Task<RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>> AdicionarTarefa(TarefaCreateModel tarefa)
        {

            try
            {
                var entity = await ModelarTarefa(tarefa);
                _tarefaRepository.Add(entity);
                _tarefaRepository.Save();

                return new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>
                {
                    ExibicaoMensagem = new ExibicaoMensagemViewModel
                    {
                        Cabecalho = "Tarefa",
                        Detalhes = "Tarefa cadastrado com sucesso!",
                        MensagemCurta = "Cadastrado com sucesso!",
                        StatusCode = 201
                    },
                    Objeto = entity.Id
                };
            }
            catch (Exception e)
            {
                return new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>
                {
                    ExibicaoMensagem = new ExibicaoMensagemViewModel
                    {
                        Cabecalho = "Erro",
                        Detalhes = e.Message,
                        MensagemCurta = "Falha ao salvar cliente",
                        StatusCode = 500
                    },
                    Objeto = Guid.Empty
                };
            }
        }
        public async Task<RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>> AtualizarTarefa(TarefaEditModel tarefa)
        {
            var tarefaAtualizar = await BuscarTarefaId(tarefa.Id);

            if (tarefaAtualizar == null)
            {
                throw new ArgumentNullException(nameof(tarefaAtualizar), "Tarefa não existe.");
            }

            var retornoController = new RetornoControllerViewModel<ExibicaoMensagemViewModel, Guid>();

            try
            {
                tarefaAtualizar.Title = tarefa.Title;
                tarefaAtualizar.Description = tarefa.Description;
                _tarefaRepository.Update(tarefaAtualizar);
                _tarefaRepository.Save();

                return retornoController;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<string> DeletarTarefa(string id)
        {

            var result = _tarefaRepository.GetSingleOrDefault(x => x.Id.ToString() == id);

            if (result == null)
                throw new Exception("Cliente não encontrado.");

            _tarefaRepository.Remove(result);
            _tarefaRepository.Save();

            return id;
        }
        public async Task<List<TarefaModel>> ListarTarefa()
        {
            var tarefaexiste = _tarefaRepository.GetAll();

            List<TarefaModel> tarefa = new List<TarefaModel>();
            foreach (var elem in tarefaexiste)
            {
                var lista = new TarefaModel();
                lista.Id = elem.Id;
                lista.Description = elem.Description;
                lista.Title = elem.Title;
                lista.Status = elem.Status;
                lista.CreatedAt = elem.CreatedAt;
                tarefa.Add(lista);
            }
            return tarefa.ToList();
        }
        public async Task<List<TarefaModel>> ListarTarefaPorId(Guid id)
        {
            var tarefaAtiva = _tarefaRepository.GetAll().Where(x => x.Id == id);

            List<TarefaModel> tarefas = new List<TarefaModel>();
            foreach (var elem in tarefaAtiva)
            {
                var lista = new TarefaModel();

                lista.Id = elem.Id;
                lista.Description = elem.Description;
                lista.Title = elem.Title;
                lista.Status = elem.Status;
                lista.CreatedAt = elem.CreatedAt;
                tarefas.Add(lista);
            }
            return tarefas.ToList();
        }
        public async Task<Tarefa> BuscarTarefaId(Guid id)
        {
            var tarefa = _tarefaRepository.Find(c => c.Id == id).FirstOrDefault();

            return tarefa;
        }
    }
}