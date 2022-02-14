using droneproject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace droneproject.Domain.Interface
{
    public interface IMedicationLoadedItem
    {
        public Task<Response> LoadedMedicationItem(string droneId);
    }
}
