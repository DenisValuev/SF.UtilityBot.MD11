using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.UtilityBot.Module11.Models;

namespace SF.UtilityBot.Module11.Services
{
    public interface IStorage
    {
        /// <summary>
        /// Получение сессии пользователя по идентификатору
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        Session GetSession(long chatId);
    }
}
