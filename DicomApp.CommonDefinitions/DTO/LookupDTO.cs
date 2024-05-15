using DicomApp.CommonDefinitions.Records;
using System.Collections.Generic;

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
        public List<PackingTypeDTO> PackingTypeDTOs { get; set; }
        public UserDTO UserDTO { get; set; }
    }
}