using RealEstateAdmin.Core.Enums;

namespace RealEstateAdmin.Core.Helpers
{
    public class ServiceResult
    {
        public object? Data { get; set; }
        public string? Message { get; set; }
        public ServiceStatus ServiceStatus { get; set; } = ServiceStatus.Success;

        public bool Status
        {
            get => ServiceStatus == ServiceStatus.Success;
        }
    }
}
