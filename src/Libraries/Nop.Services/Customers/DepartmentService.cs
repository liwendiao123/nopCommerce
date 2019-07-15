using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Services.Events;

namespace Nop.Services.Customers
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        #region
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Department> _departmentRepository;
        //private 
        #endregion
        public DepartmentService(
                IEventPublisher eventPublisher,
                IRepository<Department> departmentRepository
            )
        {
            _departmentRepository = departmentRepository;
            _eventPublisher = eventPublisher;
        }

        public bool DeleteDep(Department dep)
        {
            if (dep == null)
                throw new ArgumentNullException(nameof(dep));

            dep.Deleted = true;
            //event notification
            _eventPublisher.EntityDeleted(dep);
            return UpdateDep(dep);

          
        }

        public List<Department> GetAllDeps()
        {
            throw new NotImplementedException();
        }

        public IPagedList<Department> GetAllDeps(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            throw new NotImplementedException();
        }

        public Department GetDepById(int id)
        {
            if (id <1)
                return null;

            return _departmentRepository.GetById(id);
        }

        public bool InsertDep(Department dep)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDep(Department dep)
        {

            try
            {
                if (dep == null)
                    throw new ArgumentNullException(nameof(dep));

                _departmentRepository.Update(dep);

                //event notification
                _eventPublisher.EntityUpdated(dep);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

           
        }
    }
}
