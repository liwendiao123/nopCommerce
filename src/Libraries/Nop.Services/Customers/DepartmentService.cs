using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Services.Events;
using System.Linq;
using Nop.Core.Caching;
using Nop.Services.Catalog;

namespace Nop.Services.Customers
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        #region
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Department> _departmentRepository;
        private readonly  ICacheManager _cacheManager;
        private readonly IStaticCacheManager _staticCacheManager;
        //private 
        #endregion
        public DepartmentService(
                IEventPublisher eventPublisher,
                IRepository<Department> departmentRepository,
                ICacheManager cacheManager,
                IStaticCacheManager staticCacheManager
            )
        {
            _departmentRepository = departmentRepository;
            _eventPublisher = eventPublisher;
            _cacheManager = cacheManager;
            _staticCacheManager = staticCacheManager;
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
        /// <summary>
        /// 获取所有学校
        /// </summary>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public List<Department> GetAllDeps(bool showHidden = false)
        {
            var query = _departmentRepository.Table;


            if (!showHidden)
            {
                query = query.Where(c => c.Active);
            }

            return query.ToList();

        }

        public IPagedList<Department> GetAllDeps(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _departmentRepository.Table;
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));
            if (!showHidden)
                query = query.Where(v => v.Active);

            query = query.Where(v => !v.Deleted);
            query = query.OrderBy(v => v.DisplayOrder).ThenBy(v => v.Name);

            var deps = new PagedList<Department>(query, pageIndex, pageSize);
            return deps;
        }

        public Department GetDepById(int id)
        {
            if (id <1)
                return null;

            return _departmentRepository.GetById(id);
        }

        public bool InsertDep(Department dep)
        {

            try
            {
                if (dep == null)
                    throw new ArgumentNullException(nameof(dep));

                if (dep is IEntityForCaching)
                    throw new ArgumentException("Cacheable entities are not supported by Entity Framework");
                _departmentRepository.Insert(dep);
                //cache
                _cacheManager.RemoveByPrefix(NopCatalogDefaults.CategoriesPrefixCacheKey);
                _staticCacheManager.RemoveByPrefix(NopCatalogDefaults.CategoriesPrefixCacheKey);
                _cacheManager.RemoveByPrefix(NopCatalogDefaults.ProductCategoriesPrefixCacheKey);

                //event notification
                _eventPublisher.EntityInserted(dep);

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }

          
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
