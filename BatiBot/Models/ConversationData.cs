using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatiBot.Models
{
    public class ConversationData
    {
        public string TimeStamps { get; set; }
        public string ChannelId { get; set; }
        public bool PromptedToUser { get; set; } = false;
    }
}
