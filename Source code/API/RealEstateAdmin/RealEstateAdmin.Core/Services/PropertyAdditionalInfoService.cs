using Microsoft.Extensions.Logging;
using RealEstate.Domain.Data.Entities;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using RealEstateAdmin.Core.DTO.Forms;
using RealEstateAdmin.Core.Helpers;
using RealEstateAdmin.Core.ServiceContracts;

namespace RealEstateAdmin.Core.Services
{
    public class PropertyAdditionalInfoService : IPropertyAdditionalInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyAdditionalInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 1 Add Additional 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public async Task<PropertyAdditionalInfo> AddIPropertyAdditionalInfo(PropertyForm form, Property property)
        {
            PropertyAdditionalInfo? propertyAdditionalInfo = (await _unitOfWork.PropertyAdditionalInfoRepository.Find(propertyInfo => propertyInfo.PropertyId == property.PropertyId)).SingleOrDefault();
            ServiceResult result = new();

            if (propertyAdditionalInfo is null)
            {
                propertyAdditionalInfo = await _unitOfWork.PropertyAdditionalInfoRepository.Add(new()
                {
                    PropertyId = property.PropertyId,
                    PetPolicy = form.PetPolicy,
                    PetDeposit = form.PetDeposit,
                    PetRent = form.PetRent,
                    PetRateNegotiable = form.PetRateNegotiable,
                    MyPropLeaseTermserty = form.MyPropLeaseTermserty,
                    LeaseDuration = form.LeaseDuration,
                    Amenities = form.Amenities,
                    AvailableDaysToShow = form.Amenities,
                    AvailableFrom = form.AvailableFrom?.Date,
                    AllowToContact = form.AllowToContact,
                    ContactNumber = form.ContactNumber,
                    HideAddress = form.HideAddress,
                    UnitFeatures = form.UnitFeatures,
                    SpecialFeatures = form.SpecialFeatures,

                });
                await _unitOfWork.SaveAsync();
            }
            else
            {
                propertyAdditionalInfo.PropertyId = property.PropertyId;
                propertyAdditionalInfo.PetPolicy = form.PetPolicy;
                propertyAdditionalInfo.PetDeposit = form.PetDeposit;
                propertyAdditionalInfo.PetRent = form.PetRent;
                propertyAdditionalInfo.PetRateNegotiable = form.PetRateNegotiable;
                propertyAdditionalInfo.MyPropLeaseTermserty = form.MyPropLeaseTermserty;
                propertyAdditionalInfo.LeaseDuration = form.LeaseDuration;
                propertyAdditionalInfo.Amenities = form.Amenities;
                propertyAdditionalInfo.AvailableDaysToShow = form.Amenities;
                propertyAdditionalInfo.AvailableFrom = form.AvailableFrom?.Date;
                propertyAdditionalInfo.AllowToContact = form.AllowToContact;
                propertyAdditionalInfo.ContactNumber = form.ContactNumber;
                propertyAdditionalInfo.HideAddress = form.HideAddress;
                propertyAdditionalInfo.UnitFeatures = form.UnitFeatures;
                propertyAdditionalInfo.SpecialFeatures = form.SpecialFeatures;

                propertyAdditionalInfo = _unitOfWork.PropertyAdditionalInfoRepository.Update(propertyAdditionalInfo);
                await _unitOfWork.SaveAsync();
                result.Data = propertyAdditionalInfo;

            }



            return propertyAdditionalInfo;

        }
    }
}
