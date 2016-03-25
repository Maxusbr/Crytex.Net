using System;
using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models.GameServers;

namespace Crytex.Web.Models.JsonModels
{
    public class GameServerViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int? GameServerTariffId { get; set; }

        public int GameHostId { get; set; }
        [Required]
        public int? SlotCount { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public int ExpireMonthCount { get; set; }

        public Int32 GameId { get; set; }


        public DateTime DateExpire { get; set; }
   
        public GameServerState ServerState { get; set; }
        public bool AutoProlongation { get; set; }



        public GameServerTariffView GameServerTariff { get; set; }
    }
}