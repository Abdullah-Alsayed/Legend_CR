using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.DTO
{
    public class ViewModel<T>
    {
        public List<T> ObjDTOs { get; set; } = new List<T>();
        public T ObjDTO { get; set; }

        public LookupDTO Lookup { get; set; }
    }
}
