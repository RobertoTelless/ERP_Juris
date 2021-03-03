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
    public class SecaoService : ServiceBase<SECAO>, ISecaoService
    {
        private readonly ISecaoRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        protected ERP_JurisEntities Db = new ERP_JurisEntities();

        public SecaoService(ISecaoRepository baseRepository, ILogRepository logRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
        }

        public SECAO GetItemById(Int32 id)
        {
            SECAO item = _baseRepository.GetItemById(id);
            return item;
        }

        public SECAO CheckExist(SECAO item, Int32? idAss)
        {
            SECAO volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public List<SECAO> GetAllItens(Int32 id)
        {
            return _baseRepository.GetAllItens(id);
        }

        public List<SECAO> GetByRegiao(Int32 idRegiao)
        {
            return _baseRepository.GetByRegiao(idRegiao);
        }

        public List<SECAO> GetAllItensAdm(Int32 id)
        {
            return _baseRepository.GetAllItensAdm(id);
        }

        public List<SECAO> ExecuteFilter(Int32 idRegiao, String nome, String descricao, Int32 idAss)
        {
            List<SECAO> lista = _baseRepository.ExecuteFilter(idRegiao, nome, descricao, idAss);
            return lista;
        }

        public Int32 Create(SECAO item, LOG log)
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

        public Int32 Create(SECAO item)
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


        public Int32 Edit(SECAO item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    SECAO obj = _baseRepository.GetById(item.SECA_CD_ID);
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

        public Int32 Edit(SECAO item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    SECAO obj = _baseRepository.GetById(item.SECA_CD_ID);
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

        public Int32 Delete(SECAO item, LOG log)
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
