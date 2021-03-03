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
    public class SubSecaoService : ServiceBase<SUBSECAO>, ISubsecaoService
    {
        private readonly ISubsecaoRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        protected ERP_JurisEntities Db = new ERP_JurisEntities();

        public SubSecaoService(ISubsecaoRepository baseRepository, ILogRepository logRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
        }

        public SUBSECAO GetItemById(Int32 id)
        {
            SUBSECAO item = _baseRepository.GetItemById(id);
            return item;
        }

        public SUBSECAO CheckExist(SUBSECAO item, Int32? idAss)
        {
            SUBSECAO volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public List<SUBSECAO> GetAllItens(Int32 id)
        {
            return _baseRepository.GetAllItens(id);
        }

        public List<SUBSECAO> GetBySecao(Int32 idSecao)
        {
            return _baseRepository.GetBySecao(idSecao);
        }

        public List<SUBSECAO> GetAllItensAdm(Int32 id)
        {
            return _baseRepository.GetAllItensAdm(id);
        }

        public List<SUBSECAO> ExecuteFilter(Int32 idSecao, String nome, String descricao, Int32 idAss)
        {
            List<SUBSECAO> lista = _baseRepository.ExecuteFilter(idSecao, nome, descricao, idAss);
            return lista;
        }

        public Int32 Create(SUBSECAO item, LOG log)
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

        public Int32 Create(SUBSECAO item)
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


        public Int32 Edit(SUBSECAO item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    SUBSECAO obj = _baseRepository.GetById(item.SUSE_CD_ID);
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

        public Int32 Edit(SUBSECAO item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    SUBSECAO obj = _baseRepository.GetById(item.SUSE_CD_ID);
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

        public Int32 Delete(SUBSECAO item, LOG log)
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
