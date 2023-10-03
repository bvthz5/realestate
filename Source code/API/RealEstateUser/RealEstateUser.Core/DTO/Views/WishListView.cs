using RealEstate.Domain.Data.Entities;
using RealEstateUser.Core.Enums;

namespace RealEstateUser.Core.DTO.Views
{

    public class WishListView : PropertyDetailView
    {
        public PropertyAdditionalInfo Info { get; set; } = null!;
        public byte PropertyStatus { get; }

        public WishListView(Property property, PropertyAdditionalInfo? info, string? thumbnail) : base(property, info, thumbnail)
        {
            PropertyStatus = ((byte)((UserStatus)property.Status));
        }
    }

}

