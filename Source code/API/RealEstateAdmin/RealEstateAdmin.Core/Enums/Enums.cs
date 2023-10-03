using System.Net;

namespace RealEstateAdmin.Core.Enums
{
    public enum UserStatus
    {
        Inactive = 0,
        Active = 1,
        Blocked = 2,
        Deleted = 3,
    }



    public enum ServiceStatus
    {
        Success = HttpStatusCode.OK,
        NoRecordFound = HttpStatusCode.NotFound,
        RecordAlreadyExists = HttpStatusCode.Conflict,
        ServerError = HttpStatusCode.InternalServerError,
        InvalidRequest = HttpStatusCode.BadRequest,
        TokenExpired = HttpStatusCode.Unauthorized,
        ReferenceExists = HttpStatusCode.FailedDependency,
    }
    public enum UnitCategory
    {
        Studio = 1,
        OneBed = 2,
        TwoBed = 3,
        ThreeBed = 4,
        Other = 5,
    }

    public enum EnquiryType
    {
        RequestTour = 1,
        RequestBuy = 2,
        RequestRent = 3,

    }

    public enum CategoryType
    {
        Rent = 1,
        Buy = 2,

    }
    public enum EnquiryStatus
    {
        Pending = 1,
        Accepted = 2,
        Rejected = 3,
        Completed = 4,
    }

    public enum CategoryStatus
    {
        Active = 1,
        Inactive = 2,
        Blocked = 3,
        Deleted = 4,
    }

    public enum PropertyStatus
    {
        Inactive = 0,
        Active = 1,
        Occupied = 2,
        SoldOut = 3,
        Deleted = 4
    }

}
