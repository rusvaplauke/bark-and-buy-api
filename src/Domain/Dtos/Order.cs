using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public record Order (int id, int statusId, int sellerId, int sserId, string orderedAt); // TODO: Dates must be passed as strings encoded according to ISO-8601. 
}
