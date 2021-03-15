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
    public class VaraService : ServiceBase<VARA>, IVaraService
    {
        private readonly IVaraRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        private readonly IUFRepository _ufRepository;
        protected ERP_JurisEntities Db = new ERP_JurisEntities();

        public VaraService(IVaraRepository baseRepository, ILogRepository logRepository, IUFRepository ufRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
            _ufRepository = ufRepository;
        }

        public VARA GetItemById(Int32 id)
        {
            VARA item = _baseRepository.GetItemById(id);
            return item;
        }

        public VARA CheckExist(VARA item, Int32? idAss)
        {
            VARA volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public List<VARA> GetAllItens(Int32 id)
        {
            return _baseRepository.GetAllItens(id);
        }

        public List<UF> GetAllUF()
        {
            return _ufRepository.GetAllItens();
        }

        public UF GetUFBySigla(String sigla)
        {
            UF item = _ufRepository.GetItemBySigla(sigla);
            return item;
        }

        public List<VARA> GetByForo(Int32 idForo)
        {
            return _baseRepository.GetByForo(idForo);
        }

        public List<VARA> GetAllItensAdm(Int32 id)
        {
            return _baseRepository.GetAllItensAdm(id);
        }

        public List<VARA> ExecuteFilter(Int32 idForo, String nome, String descricao, String juiz, String cidade, String Bairro, Int32 uf, Int32 idAss)
        {
            List<VARA> lista = _baseRepository.ExecuteFilter(idForo, nome, descricao, juiz, cidade, Bairro, uf, idAss);
            return lista;
        }

        public Int32 Create(VARA item, LOG log)
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

        public Int32 Create(VARA item)
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


        public Int32 Edit(VARA item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    VARA obj = _baseRepository.GetById(item.VARA_CD_ID);
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

        public Int32 Edit(VARA item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    VARA obj = _baseRepository.GetById(item.VARA_CD_ID);
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

        public Int32 Delete(VARA item, LOG log)
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
