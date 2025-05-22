using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using TaskManager.Domain.Configuration;
using TaskManager.Domain.Domain;
using TaskManager.Domain.Service;
using TaskManager.Infra.Entity;
using TaskManager.Infra.Repositories.Interfaces;
using TaskManager.Infra.UnitofWork;
using Xunit;

namespace TaskManager.Domain.Tests.Service
{
    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
        private readonly Mock<IUnitofWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly TarefaService<TarefaModel, Tarefa> _service;

        public TarefaServiceTests()
        {
            _tarefaRepositoryMock = new Mock<ITarefaRepository>();
            _unitOfWorkMock = new Mock<IUnitofWork>();
            _mapperMock = new Mock<IMapper>();
            _appSettings = Options.Create(new AppSettings());

            _service = new TarefaService<TarefaModel, Tarefa>(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _tarefaRepositoryMock.Object,
                _appSettings
            );
        }

        [Fact]
        public async Task AdicionarTarefa_DeveAdicionarComSucesso()
        {
            var model = new TarefaCreateModel { Title = "Nova tarefa", Description = "Descricao", CreatedAt = DateTime.UtcNow, Status = System.Threading.Tasks.TaskStatus.Created };

            var result = await _service.AdicionarTarefa(model);

            _tarefaRepositoryMock.Verify(r => r.Add(It.IsAny<Tarefa>()), Times.Once);
            _tarefaRepositoryMock.Verify(r => r.Save(), Times.Once);
            Assert.Equal(201, result.ExibicaoMensagem.StatusCode);
        }

        [Fact]
        public async Task AdicionarTarefa_DeveRetornarErro_EmCasoDeExcecao()
        {
            _tarefaRepositoryMock.Setup(r => r.Add(It.IsAny<Tarefa>())).Throws(new Exception("Erro ao salvar"));
            var model = new TarefaCreateModel { Title = "Erro", Description = "Teste", CreatedAt = DateTime.UtcNow, Status = System.Threading.Tasks.TaskStatus.Created };

            var result = await _service.AdicionarTarefa(model);

            Assert.Equal(500, result.ExibicaoMensagem.StatusCode);
            Assert.Contains("Falha", result.ExibicaoMensagem.MensagemCurta);
        }

        [Fact]
        public async Task AtualizarTarefa_DeveAtualizarComSucesso()
        {
            var tarefaExistente = new Tarefa { Id = Guid.NewGuid(), Title = "Antiga", Description = "Desc antiga" };
            var model = new TarefaEditModel { Id = tarefaExistente.Id, Title = "Nova", Description = "Desc nova", Status = System.Threading.Tasks.TaskStatus.Running.ToString() };

            _tarefaRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Tarefa, bool>>>())).Returns(new List<Tarefa> { tarefaExistente });

            var result = await _service.AtualizarTarefa(model);

            _tarefaRepositoryMock.Verify(r => r.Update(It.IsAny<Tarefa>()), Times.Once);
            _tarefaRepositoryMock.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public async Task AtualizarTarefa_DeveLancarExcecao_SeNaoEncontrada()
        {
            _tarefaRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Tarefa, bool>>>())).Returns(new List<Tarefa>());
            var model = new TarefaEditModel { Id = Guid.NewGuid(), Title = "Qualquer", Description = "Teste", Status = System.Threading.Tasks.TaskStatus.Created.ToString() };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AtualizarTarefa(model));
        }

        [Fact]
        public async Task DeletarTarefa_DeveRemoverComSucesso()
        {
            var tarefa = new Tarefa { Id = Guid.NewGuid() };
            _tarefaRepositoryMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Tarefa, bool>>>())).Returns(tarefa);

            var result = await _service.DeletarTarefa(tarefa.Id.ToString());

            _tarefaRepositoryMock.Verify(r => r.Remove(tarefa), Times.Once);
            _tarefaRepositoryMock.Verify(r => r.Save(), Times.Once);
            Assert.Equal(tarefa.Id.ToString(), result);
        }

        [Fact]
        public async Task DeletarTarefa_DeveLancarExcecao_SeNaoEncontrada()
        {
            _tarefaRepositoryMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Tarefa, bool>>>())).Returns((Tarefa)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeletarTarefa(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task ListarTarefa_DeveRetornarListaDeTarefas()
        {
            _tarefaRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Tarefa>
            {
                new Tarefa { Id = Guid.NewGuid(), Title = "1", Description = "D1", Status = System.Threading.Tasks.TaskStatus.Created, CreatedAt = DateTime.UtcNow },
                new Tarefa { Id = Guid.NewGuid(), Title = "2", Description = "D2", Status = System.Threading.Tasks.TaskStatus.Running, CreatedAt = DateTime.UtcNow }
            });

            var result = await _service.ListarTarefa();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ListarTarefaPorId_DeveRetornarTarefaCorreta()
        {
            var id = Guid.NewGuid();
            _tarefaRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Tarefa>
            {
                new Tarefa { Id = id, Title = "Filtrada", Description = "Desc", Status = System.Threading.Tasks.TaskStatus.Created, CreatedAt = DateTime.UtcNow },
                new Tarefa { Id = Guid.NewGuid(), Title = "Outra", Description = "Outra desc", Status = System.Threading.Tasks.TaskStatus.Running, CreatedAt = DateTime.UtcNow }
            });

            var result = await _service.ListarTarefaPorId(id);

            Assert.Single(result);
            Assert.Equal("Filtrada", result[0].Title);
        }

        [Fact]
        public async Task BuscarTarefaId_DeveRetornarTarefa()
        {
            var id = Guid.NewGuid();
            _tarefaRepositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Tarefa, bool>>>()))
                .Returns(new List<Tarefa> { new Tarefa { Id = id } });

            var result = await _service.BuscarTarefaId(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }
    }
}