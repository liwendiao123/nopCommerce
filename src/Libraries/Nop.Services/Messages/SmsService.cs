using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Messages.SMS;
using Nop.Services.Customers;
using Nop.Services.Events;
using System.Linq;

namespace Nop.Services.Messages
{
    public partial class SmsService : ISmsService
    {

        #region filed
        private readonly ICustomerService _customerService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<SmsMsgRecord> _smsRecordRepository;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IStoreContext _storeContext;
        private readonly ITokenizer _tokenizer;
        #endregion
        public SmsService(
            ICustomerService customerService
            ,IEventPublisher eventPublisher
            ,IRepository<SmsMsgRecord> smsRecordRepository
            ,IMessageTokenProvider messageTokenProvider
            ,IStoreContext storeContext
            ,ITokenizer tokenizer
            )
        {
            _customerService = customerService;
            _eventPublisher = eventPublisher;
            _smsRecordRepository = smsRecordRepository;
            _messageTokenProvider = messageTokenProvider;
            _storeContext = storeContext;
            _tokenizer = tokenizer;
        }


        #region  ctor


        #endregion

        public bool ApplySms(SmsMsgRecord record)
        {


            throw new NotImplementedException();
        }

        public bool CheckMsgValid(SmsMsgRecord record)
        {

            throw new NotImplementedException();
        }

        public bool SendMsg(SmsMsgRecord record)
        {
            if (record == null)
            {
                return false;
            }

             var query = _smsRecordRepository.Table;
                var checkFiterResult = query.Where(
                    x => x.AppId == record.AppId
                    && x.Phone == record.Phone
                    && x.Type == record.Type
                    && x.IsRead == 0
                    ).OrderByDescending(x=>x.CreateTime).ToList();



            if (query.Count() > 0)
            {
                var existrecord = query.FirstOrDefault();


                if (existrecord == null)
                {
                }                          
                //else if(DateTime.Now )
                //{

                //}
            }

            

            throw new NotImplementedException();
        }
    }
}
