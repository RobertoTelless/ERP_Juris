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
    public class EmpresaService : ServiceBase<EMPRESA>, IEmpresaService
    {
        private readonly IEmpresaRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        private readonly IUFRepository _ufRepository;
        protected ERP_JurisEntities Db = new ERP_JurisEntities();

        public EmpresaService(IEmpresaRepository baseRepository, ILogRepository logRepository, IUFRepository ufRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
            _ufRepository = ufRepository;
        }

        public EMPRESA GetItemById(Int32 id)
        {
            EMPRESA item = _baseRepository.GetItemById(id);
            return item;
        }

        public UF GetItemBySigla(String sigla)
        {
            UF item = _ufRepository.GetItemBySigla(sigla);
            return item;
        }

        public List<EMPRESA> GetAllItems()
        {
            List<EMPRESA> item = _baseRepository.GetAllItems();
            return item;
        }

        public List<UF> GetAllUF()
        {
            List<UF> item = _ufRepository.GetAllItens();
            return item;
        }

        public Int32 Edit(EMPRESA item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    EMPRESA obj = _baseRepository.GetById(item.EMPR_CD_ID);
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

        public Int32 Create(EMPRESA item)
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
    }
}
