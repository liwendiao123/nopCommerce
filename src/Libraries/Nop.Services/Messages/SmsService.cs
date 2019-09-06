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
using Nop.Core.Infrastructure;

namespace Nop.Services.Messages
{
    public partial class SmsService : BaseService, ISmsService
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

        public QSResult<bool> ApplySms(SmsMsgRecord record)
        {
            if (record == null)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            try
            {
                var query = _smsRecordRepository.Table;
                var checkFiterResult = query.Where(
                    x => x.AppId == record.AppId
                    && x.Phone == record.Phone
                    && x.Type == record.Type
                    && x.TemplateCode == record.TemplateCode
                    && x.IsRead == 0
                    ).OrderByDescending(x => x.CreateTime).ToList();

                if (checkFiterResult.Count() > 0)
                {
                    var existrecord = checkFiterResult.FirstOrDefault();
                    if (existrecord == null || DateTime.Now.Subtract(existrecord.CreateTime).TotalSeconds > 300)
                    {
                       // _smsRecordRepository.Insert(record);                                      
                       return Result(false, " 验证码无效或超时",ErrorCode.sys_fail);
                    }
                    else
                    {

                        record = existrecord;
                        //record = existrecord;

                        record.IsRead = 1;
                      
                         _smsRecordRepository.Update(record);
                        return Result(true,"验证成功" ,ErrorCode.sys_success);
                    }
                }
                else
                {
                    return Result(false,"验证失败",ErrorCode.sys_fail);
                }

            }
            catch (Exception ex)
            {
                return Result(false, ex.Message, ErrorCode.sys_fail);
            }




           // throw new NotImplementedException();
        }

        public QSResult<bool> CheckMsgValid(SmsMsgRecord record)
        {

            if (record == null)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            try
            {
                var query = _smsRecordRepository.Table;
                var checkFiterResult = query.Where(
                    x => x.AppId == record.AppId
                    && x.Phone == record.Phone
                    && x.Type == record.Type
                    //&& x.TemplateCode == record.TemplateCode
                    && x.IsRead == 0
                    ).OrderByDescending(x => x.CreateTime).ToList();

                if (checkFiterResult.Count() > 0)
                {
                    var existrecord = checkFiterResult.FirstOrDefault();


                    if (existrecord == null || DateTime.Now.Subtract(existrecord.CreateTime).TotalSeconds > 300)
                    {
                       // _smsRecordRepository.Insert(record);

                        return Result(true);
                    }
                    else
                    {

                        record.TemplateCode = existrecord.TemplateCode;
                        return Result(false,"频繁发送信息", ErrorCode.mobile_sms_frequently);
                    }
                }
                else
                {
                    return Result(true);
                }

            }
            catch (Exception ex)
            {
                return Result(false,ex.Message ,ErrorCode.sys_fail);
            }
           

            //throw new NotImplementedException();
        }


        public QSResult<bool> CheckMsgValidWithCode(SmsMsgRecord record)
        {

            if (record == null)
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            try
            {
                var query = _smsRecordRepository.Table;
                var checkFiterResult = query.Where(
                    x => x.AppId == record.AppId
                    && x.Phone == record.Phone
                    && x.Type == record.Type
                    && x.TemplateCode == record.TemplateCode
                    && x.IsRead == 0
                    ).OrderByDescending(x => x.CreateTime).ToList();

                if (checkFiterResult.Count() > 0)
                {
                    var existrecord = checkFiterResult.FirstOrDefault();
                    if (existrecord == null || DateTime.Now.Subtract(existrecord.CreateTime).TotalSeconds  <= 300)
                    {
                        // _smsRecordRepository.Insert(record);
                        return Result(true);
                    }
                    else
                    {
                        record.TemplateCode = existrecord.TemplateCode;
                        return Result(false, "频繁发送信息", ErrorCode.mobile_sms_frequently);
                    }
                }
                else
                {
                    return Result(false, "验证码无效", ErrorCode.sys_fail);
                }

            }
            catch (Exception ex)
            {
                return Result(false, ex.Message, ErrorCode.sys_fail);
            }


            //throw new NotImplementedException();
        }

        public QSResult<bool> SendMsg(SmsMsgRecord record)
        {
            if (record == null)
            {
                return Result(false,ErrorCode.sys_param_format_error);
            }


            try
            {
                var query = _smsRecordRepository.Table;
                var checkFiterResult = query.Where(
                    x => x.AppId == record.AppId
                    && x.Phone == record.Phone
                    && x.Type == record.Type
                    && x.IsRead == 0
                    ).OrderByDescending(x => x.CreateTime).ToList();



                if (checkFiterResult.Count() > 0)
                {
                    var existrecord = checkFiterResult.FirstOrDefault();


                    if (existrecord == null || DateTime.Now.Subtract(existrecord.CreateTime).TotalSeconds > 300)
                    {

                        _smsRecordRepository.Insert(record);

                        return Result(true);
                    }
                    else
                    {
                        return Result(false, ErrorCode.mobile_sms_frequently);
                    }
                }
                else
                {
                    _smsRecordRepository.Insert(record);

                    return Result(true);
                }

            }
            catch (Exception ex)
            {
                return Result(false, ex.Message, ErrorCode.sys_fail);
            }                     
            //throw new NotImplementedException();
        }
    }
}
