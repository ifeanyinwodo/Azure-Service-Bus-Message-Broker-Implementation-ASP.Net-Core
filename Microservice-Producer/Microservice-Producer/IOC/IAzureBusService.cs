using ItemModel_Nugget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Producer.IOC
{
    public interface IAzureBusService
    {
        public Task SendMessageAsync(Item serviceBusMessage);
    }
}
