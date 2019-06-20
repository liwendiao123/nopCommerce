using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.AIBookModel;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.AIBookModel
{
    public partial class AiBookService : IAiBookService
    {
        public int DeleteAiBookModel(AiBookModel store)
        {
            throw new NotImplementedException();
        }

        public AiBookModel GetAiBookModelById(int storeId, bool loadCacheableCopy = true)
        {
            throw new NotImplementedException();
        }

        public IList<AiBookModel> GetAllAiBookModels(bool loadCacheableCopy = true)
        {
            throw new NotImplementedException();
        }

        public string[] GetNotExistingAiBookModels(string[] storeIdsNames)
        {
            throw new NotImplementedException();
        }

        public int InsertAiBookModel(AiBookModel bookdir)
        {
            throw new NotImplementedException();
        }

        public IPagedList<AiBookModel> SearchProducts(int pageIndex = 0, int pageSize = int.MaxValue, IList<int> categoryIds = null, int manufacturerId = 0, int storeId = 0, int vendorId = 0, bool visibleIndividuallyOnly = false, bool markedAsNewOnly = false, bool? featuredProducts = null, string keywords = null, bool searchDescriptions = false, bool searchManufacturerPartNumber = true, bool searchSku = true, bool searchProductTags = false, int languageId = 0, IList<int> filteredSpecs = null, ProductSortingEnum orderBy = ProductSortingEnum.Position, bool showHidden = false, bool? overridePublished = null)
        {
            throw new NotImplementedException();
        }

        public int UpdateAiBookModel(AiBookModel bookdir)
        {
            throw new NotImplementedException();
        }
    }
}
