namespace Telemedicine.API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value ;}
        }

        public int UserId { get; set; }

        public bool Selectees { get; set; } = false;

        public bool Selectors { get; set; } = false;

        public bool DoctorRoleOnly { get; set; } = false;
        
    }
}