using System.Collections.Generic;
using DicomApp.CommonDefinitions.Records;

namespace DicomApp.CommonDefinitions.DTO
{
    public class LookupDTO
    {
        public List<ZoneDTO> ZoneDTOs { get; set; }
        public List<UserDTO> UserDTOs { get; set; }
        public List<StatusDTO> StatusDTOs { get; set; }

        public List<UserDTO> VendorDTOs { get; set; }
        public List<UserDTO> CourierDTOs { get; set; }
        public List<UserDTO> EmployeeDTOs { get; set; }
        public List<RoleDTO> RoleDTOs { get; set; }
        public List<CityDTO> AreaDTOs { get; set; }
        public List<CategoryDTO> CategoryDTOs { get; set; }
        public UserDTO UserDTO { get; set; }
    }
}
