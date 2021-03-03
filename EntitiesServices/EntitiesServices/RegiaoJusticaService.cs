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
    public class RegiaoJusticaService : ServiceBase<REGIAO_JUSTICA>, IRegiaoJusticaService
    {
        private readonly IRegiaoJusticaRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        protected ERP_JurisEntities Db = new ERP_JurisEntities();

        public RegiaoJusticaService(IRegiaoJusticaRepository baseRepository, ILogRepository logRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
        }

        public REGIAO_JUSTICA GetItemById(Int32 id)
        {
            REGIAO_JUSTICA item = _baseRepository.GetItemById(id);
            return item;
        }

        public REGIAO_JUSTICA CheckExist(REGIAO_JUSTICA item, Int32? idAss)
        {
            REGIAO_JUSTICA volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public List<REGIAO_JUSTICA> GetAllItens(Int32 id)
        {
            return _baseRepository.GetAllItens(id);
        }

        public List<REGIAO_JUSTICA> GetByTipo(Int32 idTipo)
        {
            return _baseRepository.GetByTipo(idTipo);
        }

        public List<REGIAO_JUSTICA> GetAllItensAdm(Int32 id)
        {
            return _baseRepository.GetAllItensAdm(id);
        }

        public List<REGIAO_JUSTICA> ExecuteFilter(Int32 idTipo, String nome, String descricao, Int32 idAss)
        {
            List<REGIAO_JUSTICA> lista = _baseRepository.ExecuteFilter(idTipo, nome, descricao, idAss);
            return lista;
        }

        public Int32 Create(REGIAO_JUSTICA item, LOG log)
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

        public Int32 Create(REGIAO_JUSTICA item)
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


        public Int32 Edit(REGIAO_JUSTICA item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    REGIAO_JUSTICA obj = _baseRepository.GetById(item.REJU_CD_ID);
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

        public Int32 Edit(REGIAO_JUSTICA item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    REGIAO_JUSTICA obj = _baseRepository.GetById(item.REJU_CD_ID);
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

        public Int32 Delete(REGIAO_JUSTICA item, LOG log)
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
