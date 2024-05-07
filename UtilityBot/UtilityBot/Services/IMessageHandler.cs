using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityBot.Services
{
    internal interface IMessageHandler
    {
        int Counting(string message, string nameFunction);
    }
}
