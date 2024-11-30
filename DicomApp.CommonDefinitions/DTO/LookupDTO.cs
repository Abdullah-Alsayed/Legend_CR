using System.Collections.Generic;
using DicomApp.CommonDefinitions.Records;

namespace DicomApp.CommonDefinitions.DTO
{
    public class LookupDTO
    {
        public List<ZoneDTO> ZoneDTOs { get; set; } = new List<ZoneDTO>();
        public List<UserDTO> UserDTOs { get; set; } = new List<UserDTO>();
        public List<StatusDTO> StatusDTOs { get; set; } = new List<StatusDTO>();

        public List<UserDTO> VendorDTOs { get; set; } = new List<UserDTO>();
        public List<UserDTO> CourierDTOs { get; set; } = new List<UserDTO>();
        public List<UserDTO> EmployeeDTOs { get; set; } = new List<UserDTO>();
        public List<RoleDTO> RoleDTOs { get; set; } = new List<RoleDTO>();
        public List<CityDTO> AreaDTOs { get; set; } = new List<CityDTO>();
        public List<CategoryDTO> CategoryDTOs { get; set; } = new List<CategoryDTO>();
        public UserDTO UserDTO { get; set; } = new UserDTO();
        public List<GameDTO> GameDTOs { get; set; } = new List<GameDTO>();
        public List<CountryDTO> CountryDTOs { get; set; } = new List<CountryDTO>();
    }
}
