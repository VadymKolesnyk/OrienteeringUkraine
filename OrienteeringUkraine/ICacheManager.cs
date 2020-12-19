using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine
{
    /// <summary>
    /// Интерфейс для работы с кешированием
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Кеширование списка регионов
        /// </summary>
        /// <param name="list">Список регионов</param>
        public void SetRegions(IEnumerable<Region> list);
        /// <summary>
        /// Получить список регионов из кеша
        /// </summary>
        /// <returns>Список регионов</returns>
        public IEnumerable<Region> GetRegions();
        /// <summary>
        /// Кеширование списка клубов
        /// </summary>
        /// <param name="list">Список клубов</param>
        public void SetClubs(IEnumerable<Club> list);
        /// <summary>
        /// Получить список клубов из кеша
        /// </summary>
        /// <returns>Список клубов</returns>
        public IEnumerable<Club> GetClubs();
    }
}
