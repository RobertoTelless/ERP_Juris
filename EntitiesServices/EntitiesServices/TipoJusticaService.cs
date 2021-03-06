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
    public class TipoJusticaService : ServiceBase<TIPO_JUSTICA>, ITipoJusticaService
    {
        private readonly ITipoJusticaRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        protected ERP_JurisEntities Db = new ERP_JurisEntities();

        public TipoJusticaService(ITipoJusticaRepository baseRepository, ILogRepository logRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
        }

        public TIPO_JUSTICA GetItemById(Int32 id)
        {
            TIPO_JUSTICA item = _baseRepository.GetItemById(id);
            return item;
        }

        public TIPO_JUSTICA CheckExist(TIPO_JUSTICA item, Int32? idAss)
        {
            TIPO_JUSTICA volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public List<TIPO_JUSTICA> GetAllItens(Int32 id)
        {
            return _baseRepository.GetAllItens(id);
        }

        public List<TIPO_JUSTICA> GetAllItensAdm(Int32 id)
        {
            return _baseRepository.GetAllItensAdm(id);
        }

        public List<TIPO_JUSTICA> ExecuteFilter(String nome, String descricao, Int32 idAss)
        {
            List<TIPO_JUSTICA> lista = _baseRepository.ExecuteFilter(nome, descricao, idAss);
            return lista;
        }

        public Int32 Create(TIPO_JUSTICA item, LOG log)
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

        public Int32 Create(TIPO_JUSTICA item)
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


        public Int32 Edit(TIPO_JUSTICA item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    TIPO_JUSTICA obj = _baseRepository.GetById(item.TIJU_CD_ID);
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

        public Int32 Edit(TIPO_JUSTICA item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    TIPO_JUSTICA obj = _baseRepository.GetById(item.TIJU_CD_ID);
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

        public Int32 Delete(TIPO_JUSTICA item, LOG log)
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
