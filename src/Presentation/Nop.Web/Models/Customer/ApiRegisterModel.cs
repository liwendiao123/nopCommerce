﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Models.Api;

namespace Nop.Web.Models.Customer
{
    public class ApiRegisterModel:BaseApiRequest
    {
        public bool PhoneEnabled { get; set; }
        public bool PhoneRequired { get; set; }
        [DataType(DataType.PhoneNumber)]
        [NopResourceDisplayName("Account.Fields.Phone")]
        public string Phone { get; set; }

        [NopResourceDisplayName("Account.Fields.LastName")]
        public string Name { get; set; }
        [NopResourceDisplayName("Account.Fields.Username")]
        public string UserName { get; set; }
        //[DataType(DataType.Password)]
        //[NoTrim]
        [NopResourceDisplayName("Account.Fields.Password")]
        public string Password { get; set; }
        //[DataType(DataType.Password)]
        //[NoTrim]
        [NopResourceDisplayName("Account.Fields.ConfirmPassword")]
        public string ConfirmPassword { get; set; }
        public string SmsCode { get; set; }
        public int Occupation { get; set; }
        public int DepartmentId { get; set; }
      //  [AllowHtml]
      // [DataType(DataType.ImageUrl)]
       // [JsonProperty("")]
        public string Imgurl { get; set; }
        public string InviteCode { get; set; }
        //[DataType(DataType.EmailAddress)]
        //[NopResourceDisplayName("Account.Fields.Email")]
        public string Email { get; set; }
        public string CertImg { get; set; }
        public int PlatformType { get; set; }
        //form fields & properties
        public bool GenderEnabled { get; set; }
        [NopResourceDisplayName("Account.Fields.Gender")]
        public string Gender { get; set; }

        [NopResourceDisplayName("Account.Fields.FirstName")]
        public string FirstName { get; set; }
        [NopResourceDisplayName("Account.Fields.LastName")]
        public string LastName { get; set; }

        public bool DateOfBirthEnabled { get; set; }
        [NopResourceDisplayName("Account.Fields.DateOfBirth")]
        public int? DateOfBirthDay { get; set; }
        [NopResourceDisplayName("Account.Fields.DateOfBirth")]
        public int? DateOfBirthMonth { get; set; }
        [NopResourceDisplayName("Account.Fields.DateOfBirth")]
        public int? DateOfBirthYear { get; set; }
        public bool DateOfBirthRequired { get; set; }
        public DateTime? ParseDateOfBirth()
        {
            if (!DateOfBirthYear.HasValue || !DateOfBirthMonth.HasValue || !DateOfBirthDay.HasValue)
                return null;

            DateTime? dateOfBirth = null;
            try
            {
                dateOfBirth = new DateTime(DateOfBirthYear.Value, DateOfBirthMonth.Value, DateOfBirthDay.Value);
            }
            catch { }
            return dateOfBirth;
        }

        public bool CompanyEnabled { get; set; }
        public bool CompanyRequired { get; set; }
        [NopResourceDisplayName("Account.Fields.Company")]
        public string Company { get; set; }

        public bool StreetAddressEnabled { get; set; }
        public bool StreetAddressRequired { get; set; }
        [NopResourceDisplayName("Account.Fields.StreetAddress")]
        public string StreetAddress { get; set; }

        public bool StreetAddress2Enabled { get; set; }
        public bool StreetAddress2Required { get; set; }
        [NopResourceDisplayName("Account.Fields.StreetAddress2")]
        public string StreetAddress2 { get; set; }

        public bool ZipPostalCodeEnabled { get; set; }
        public bool ZipPostalCodeRequired { get; set; }
        [NopResourceDisplayName("Account.Fields.ZipPostalCode")]
        public string ZipPostalCode { get; set; }

        public bool CityEnabled { get; set; }
        public bool CityRequired { get; set; }
        [NopResourceDisplayName("Account.Fields.City")]
        public string City { get; set; }

        public bool CountyEnabled { get; set; }
        public bool CountyRequired { get; set; }
        [NopResourceDisplayName("Account.Fields.County")]
        public string County { get; set; }

        public bool CountryEnabled { get; set; }
        public bool CountryRequired { get; set; }
        [NopResourceDisplayName("Account.Fields.Country")]
        public int CountryId { get; set; }
      //  public IList<SelectListItem> AvailableCountries { get; set; }

        public bool StateProvinceEnabled { get; set; }
        public bool StateProvinceRequired { get; set; }
        [NopResourceDisplayName("Account.Fields.StateProvince")]
        public int StateProvinceId { get; set; }
        [NopResourceDisplayName("Account.Fields.TimeZone")]
        public string TimeZoneId { get; set; }
        //EU VAT
        [NopResourceDisplayName("Account.Fields.VatNumber")]
        public string VatNumber { get; set; }
        public bool DisplayVatNumber { get; set; }

        public bool FaxEnabled { get; set; }
        public bool FaxRequired { get; set; }
        [DataType(DataType.PhoneNumber)]
        [NopResourceDisplayName("Account.Fields.Fax")]
        public string Fax { get; set; }
        public bool NewsletterEnabled { get; set; }
        [NopResourceDisplayName("Account.Fields.Newsletter")]
        public bool Newsletter { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

    }
}
