using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityBot.Models;

namespace UtilityBot.Services
{
    internal class MessageService : IMessageHandler
    {
       
        
        public int Counting(string message, string nameFunction)
        {
            
            if (nameFunction == "count")
            {
                int sum = 0;
                for (int i = 0; i < message.Length; i++)
                {
                    if (message[i] != ' ')
                    {
                        sum += 1;
                    }
                }
                return sum;
            }
            else
            {
                int? sum = 0;

                string[] words = message.Split(' ');

                foreach (string word in words)
                {
                    int x = 0;
                    if (int.TryParse(word, out x)) sum += x;
                    else sum = null;
                }

                if (sum != null)
                {
                    return (int)sum;
                }
               
                return (int)sum;
            }



           
        }

       
    }
}
