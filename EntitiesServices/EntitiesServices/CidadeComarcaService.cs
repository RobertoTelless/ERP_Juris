using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;
using ModelServices.Interfaces.Repositories;
using ModelServices.Interfaces.EntitiesServices;
using CrossCutting;
using System.Data.Entity;
using System.Data;

namespace ModelServices.EntitiesServices
{
    public class CidadeComarcaService : ServiceBase<CIDADE_COMARCA>, ICidadeComarcaService
    {
        private readonly ICidadeComarcaRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        private readonly IUFRepository _ufRepository;
        protected ERP_JurisEntities Db = new ERP_JurisEntities();

        public CidadeComarcaService(ICidadeComarcaRepository baseRepository, ILogRepository logRepository, IUFRepository ufRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
            _ufRepository = ufRepository;
        }

        public CIDADE_COMARCA GetItemById(Int32 id)
        {
            CIDADE_COMARCA item = _baseRepository.GetItemById(id);
            return item;
        }

        public CIDADE_COMARCA CheckExist(CIDADE_COMARCA item, Int32? idAss)
        {
            CIDADE_COMARCA volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public List<CIDADE_COMARCA> GetAllItens(Int32 id)
        {
            return _baseRepository.GetAllItens(id);
        }

        public List<UF> GetAllUF()
        {
            return _ufRepository.GetAllItens();
        }

        public List<CIDADE_COMARCA> GetAllItensAdm(Int32 id)
        {
            return _baseRepository.GetAllItensAdm(id);
        }

        public List<CIDADE_COMARCA> ExecuteFilter(String nome, Int32 uf, Int32 idAss)
        {
            List<CIDADE_COMARCA> lista = _baseRepository.ExecuteFilter(nome, uf, idAss);
            return lista;
        }

        public Int32 Create(CIDADE_COMARCA item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _logRepository.Add(log);
                    _baseRepository.Add(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 Create(CIDADE_COMARCA item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _baseRepository.Add(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }


        public Int32 Edit(CIDADE_COMARCA item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    CIDADE_COMARCA obj = _baseRepository.GetById(item.CICO_CD_ID);
                    _baseRepository.Detach(obj);
                    _logRepository.Add(log);
                    _baseRepository.Update(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 Edit(CIDADE_COMARCA item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    CIDADE_COMARCA obj = _baseRepository.GetById(item.CICO_CD_ID);
                    _baseRepository.Detach(obj);
                    _baseRepository.Update(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 Delete(CIDADE_COMARCA item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _logRepository.Add(log);
                    _baseRepository.Remove(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

    }
}
