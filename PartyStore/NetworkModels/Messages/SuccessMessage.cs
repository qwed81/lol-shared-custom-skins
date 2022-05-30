using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyStore.NetworkModels.Messages
{
    internal class SuccessMessage
    {

        public Guid SuccessfulMessageId { get; set; }

        public SuccessMessage(Guid successfulMessageId)
        {
            SuccessfulMessageId = successfulMessageId;
        }

    }
}
