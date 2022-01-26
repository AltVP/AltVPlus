using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVPlus
{
    internal class AltVScript : IScript
    {
        [AsyncScriptEvent(ScriptEventType.PlayerConnect)]
        public static async Task PlayerConnect(IPlayer player, string reason)
        {
            await player.SetModelAsync(PedModel.BallasogCutscene);
            await player.SpawnAsync(AltVDev.LegionSquare);

            await player.GetHealthAsync()
                        .LogAsync(a => a.Log($"Player health: {a}"));

            await player.GetPingAsync()
                        .LogAsync(a => a.Log($"Player ping: {a}"));

            await player.SetArmorAsync(50)
                        .LogAsync("Armor changed to 50");

            await player.LoopAsync(async () =>
            {
                await player.GetPositionAsync()
                            .LogAsync(a => a.Log($"Position: X:{a.X} Y:{a.Y} Z:{a.Z}"));
            });
        }

        [AsyncClientEvent("Console")]
        public static async Task OnConsoleCommand(IPlayer player, string command, string[] args)
        {
            if (command == "respawn")
                await player.RespawnAsync();
        }
    }
}
