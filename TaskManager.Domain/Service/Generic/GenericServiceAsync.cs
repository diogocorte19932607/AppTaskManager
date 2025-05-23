﻿using AutoMapper;
using System.Linq.Expressions;
using TaskManager.Domain.Domain;
using TaskManager.Infra.Entity;
using TaskManager.Infra.UnitofWork;

namespace TaskManager.Domain.Service.Generic
{
    public class GenericServiceAsync<Tv, Te> : IServiceAsync<Tv, Te> where Tv : BaseDomain
                                         where Te : BaseEntity
    {
        protected IUnitofWork _unitOfWork;
        protected IMapper _mapper;
        public GenericServiceAsync(IUnitofWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericServiceAsync()
        {
        }

        public virtual async Task<IEnumerable<Tv>> GetAll()
        {
            var entities = await _unitOfWork.GetRepositoryAsync<Te>()
                .GetAll();
            return _mapper.Map<IEnumerable<Tv>>(source: entities);
        }

        public virtual async Task<Tv> GetOne(Guid id)
        {
            var entity = await _unitOfWork.GetRepositoryAsync<Te>()
                .GetOne(predicate: x => x.Id == id);
            return _mapper.Map<Tv>(source: entity);
        }

        public virtual async Task<Guid> Add(Tv view)
        {
            var entity = _mapper.Map<Te>(source: view);
            await _unitOfWork.GetRepositoryAsync<Te>().Insert(entity);
            await _unitOfWork.SaveAsync();
            return entity.Id;
        }

        public async Task<int> Update(Tv view)
        {
            await _unitOfWork.GetRepositoryAsync<Te>().Update(view.Id, _mapper.Map<Te>(source: view));
            return await _unitOfWork.SaveAsync();
        }

        public virtual async Task<int> Remove(Guid id)
        {
            Te entity = await _unitOfWork.Context.Set<Te>().FindAsync(id);
            await _unitOfWork.GetRepositoryAsync<Te>().Delete(id);
            return await _unitOfWork.SaveAsync();
        }

        public virtual async Task<IEnumerable<Tv>> Get(Expression<Func<Te, bool>> predicate)
        {
            var items = await _unitOfWork.GetRepositoryAsync<Te>()
                .Get(predicate: predicate);
            return _mapper.Map<IEnumerable<Tv>>(source: items);
        }
    }
}
